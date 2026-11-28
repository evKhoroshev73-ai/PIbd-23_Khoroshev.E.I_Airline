using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using AirlineManagement.Repositories.Interfaces;
using Unity;

namespace AirlineManagement.Forms
{
    public partial class FlightExecutionsForm : Form
    {
        private readonly IFlightExecutionRepository _repository;
        private DataGridView grid;
        private Button btnAdd;

        public FlightExecutionsForm(IFlightExecutionRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
            Shown += (_, _) => LoadData();
        }

        [MemberNotNull(nameof(grid), nameof(btnAdd))]
        private void SetupLayout()
        {
            this.Text = "Операция: Выполнение полетов";

            grid = new DataGridView { AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            btnAdd = new Button { Text = "Добавить выполнение" };

            btnAdd.Click += (s, e) =>
            {
                bool saved = false;
                if (FormAction.Try(this, "Не удалось открыть форму добавления записи.", () =>
                {
                    using var form = Program.Container.Resolve<FlightExecutionAddForm>();
                    saved = form.ShowDialog(this) == DialogResult.OK;
                }) && saved)
                    LoadData();
            };

            FormLayout.ConfigureList(this, grid, btnAdd);
        }

        private void LoadData()
        {
            FormAction.Try(this, "Не удалось загрузить записи. Проверьте подключение к базе данных.", () =>
                grid.DataSource = _repository.ReadFlightExecutions().Select(execution => new
                {
                    Код = execution.Id,
                    Рейс = execution.FlightId,
                    Самолет = execution.AirplaneId,
                    Ответственный = execution.EmployeeId,
                    Экипаж = string.Join(", ", execution.CrewMembers.Select(member => member.EmployeeId).OrderBy(id => id)),
                    Мест = execution.OccupiedSeats,
                    Дата = execution.FlightDate.ToShortDateString()
                }).ToList());
        }
    }
}