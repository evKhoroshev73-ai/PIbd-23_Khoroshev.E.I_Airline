using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Entities.Enums;
using AirlineManagement.Repositories.Interfaces;
using Unity;

namespace AirlineManagement.Forms
{
    public partial class EmployeesForm : Form
    {
        private readonly IEmployeeRepository _repository;
        private DataGridView grid;
        private Button btnAdd, btnEdit, btnDelete;

        public EmployeesForm(IEmployeeRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
            Shown += (_, _) => LoadData();
        }

        [MemberNotNull(nameof(grid), nameof(btnAdd), nameof(btnEdit), nameof(btnDelete))]
        private void SetupLayout()
        {
            this.Text = "Справочник: Сотрудники";

            grid = new DataGridView { AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            grid.CellFormatting += (_, e) =>
            {
                if (e.ColumnIndex >= 0 && grid.Columns[e.ColumnIndex].DataPropertyName == nameof(Employee.Post)
                    && e.Value is EmployeePost post)
                {
                    e.Value = FormAction.EmployeePostLabel(post);
                    e.FormattingApplied = true;
                }
            };
            btnAdd = new Button { Text = "Добавить" };
            btnEdit = new Button { Text = "Редактировать" };
            btnDelete = new Button { Text = "Удалить" };

            btnAdd.Click += (s, e) => OpenEditor(null);
            btnEdit.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is Employee item)
                    OpenEditor(item);
            };
            btnDelete.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is not Employee item)
                    return;
                if (MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение удаления",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
                if (FormAction.Try(this, "Не удалось удалить запись. Возможно, она используется в других записях.",
                    () => _repository.DeleteEmployee(item.Id)))
                    LoadData();
            };

            FormLayout.ConfigureList(this, grid, btnAdd, btnEdit, btnDelete);
        }

        private void OpenEditor(Employee? item)
        {
            bool saved = false;
            if (FormAction.Try(this, "Не удалось открыть форму записи.", () =>
            {
                using var form = Program.Container.Resolve<EmployeeEditForm>();
                if (item != null)
                    form.SetEntry(item);
                saved = form.ShowDialog(this) == DialogResult.OK;
            }) && saved)
                LoadData();
        }

        private void LoadData()
        {
            FormAction.Try(this, "Не удалось загрузить записи. Проверьте подключение к базе данных.",
                () =>
                {
                    grid.DataSource = _repository.ReadEmployees().ToList();
                    FormAction.SetColumnLabels(grid, (nameof(Employee.Id), "Код"),
                        (nameof(Employee.FullName), "ФИО"), (nameof(Employee.Post), "Должность"));
                });
        }
    }
}
