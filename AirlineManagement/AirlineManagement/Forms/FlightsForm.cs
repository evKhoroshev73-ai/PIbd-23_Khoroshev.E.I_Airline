using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;
using Unity;

namespace AirlineManagement.Forms
{
    public partial class FlightsForm : Form
    {
        private readonly IFlightRepository _repository;
        private DataGridView grid;
        private Button btnAdd, btnEdit, btnDelete;

        public FlightsForm(IFlightRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
            Shown += (_, _) => LoadData();
        }

        [MemberNotNull(nameof(grid), nameof(btnAdd), nameof(btnEdit), nameof(btnDelete))]
        private void SetupLayout()
        {
            this.Text = "Справочник: Рейсы";

            grid = new DataGridView { AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            btnAdd = new Button { Text = "Добавить" };
            btnEdit = new Button { Text = "Редактировать" };
            btnDelete = new Button { Text = "Удалить" };

            btnAdd.Click += (s, e) => OpenEditor(null);
            btnEdit.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is Flight item)
                    OpenEditor(item);
            };
            btnDelete.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is not Flight item)
                    return;
                if (MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение удаления",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
                if (FormAction.Try(this, "Не удалось удалить запись. Возможно, она используется в других записях.",
                    () => _repository.DeleteFlight(item.Id)))
                    LoadData();
            };

            FormLayout.ConfigureList(this, grid, btnAdd, btnEdit, btnDelete);
        }

        private void OpenEditor(Flight? item)
        {
            bool saved = false;
            if (FormAction.Try(this, "Не удалось открыть форму записи.", () =>
            {
                using var form = Program.Container.Resolve<FlightEditForm>();
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
                    grid.DataSource = _repository.ReadFlights().ToList();
                    FormAction.SetColumnLabels(grid, (nameof(Flight.Id), "Код"),
                        (nameof(Flight.FlightNumber), "Номер рейса"), (nameof(Flight.Destination), "Пункт назначения"),
                        (nameof(Flight.DepartureTime), "Вылет"), (nameof(Flight.ArrivalTime), "Прилёт"));
                });
        }
    }
}
