using System;
using System.Windows.Forms;
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
            LoadData();
        }

        private void SetupLayout()
        {
            this.Text = "Справочник: Сотрудники";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView { Dock = DockStyle.Top, Height = 330, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            btnAdd = new Button { Text = "Добавить", Left = 20, Top = 350, Width = 110, Height = 30 };
            btnEdit = new Button { Text = "Редактировать", Left = 140, Top = 350, Width = 130, Height = 30 };
            btnDelete = new Button { Text = "Удалить", Left = 280, Top = 350, Width = 110, Height = 30 };

            btnAdd.Click += (s, e) => {
                var form = Program.Container.Resolve<EmployeeEditForm>();
                form.ShowDialog();
            };

            btnEdit.Click += (s, e) => {
                var form = Program.Container.Resolve<EmployeeEditForm>();
                form.ShowDialog();
            };

            btnDelete.Click += (s, e) => {
                MessageBox.Show("Элемент удален (режим заглушки).", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
        }

        private void LoadData()
        {
            grid.DataSource = null;
            grid.DataSource = _repository.ReadEmployees();
        }
    }
}