using System;
using System.Linq;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Entities.Enums;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class FlightExecutionAddForm : Form
    {
        private readonly IFlightExecutionRepository _repository;
        private readonly IFlightRepository _flights;
        private readonly IAirplaneRepository _airplanes;
        private readonly IEmployeeRepository _employees;
        private ComboBox cmbFlight = null!, cmbAirplane = null!, cmbEmployee = null!;
        private CheckedListBox crew = null!;
        private TextBox txtSeats = null!;
        private DateTimePicker dtpDate = null!;
        private Button btnSave = null!;
        private Employee[] _crewChoices = Array.Empty<Employee>();

        public FlightExecutionAddForm(IFlightExecutionRepository repository, IFlightRepository flights,
            IAirplaneRepository airplanes, IEmployeeRepository employees)
        {
            InitializeComponent();
            _repository = repository;
            _flights = flights;
            _airplanes = airplanes;
            _employees = employees;
            SetupLayout();
            Shown += (_, _) => LoadChoices();
        }

        private void SetupLayout()
        {
            Text = "Добавить выполнение полета";
            cmbFlight = CreateChoice(nameof(Flight.FlightNumber));
            cmbAirplane = CreateChoice(nameof(Airplane.Model));
            cmbEmployee = CreateChoice(nameof(Employee.FullName));
            txtSeats = new TextBox();
            dtpDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            crew = new CheckedListBox
            {
                MinimumSize = new System.Drawing.Size(0, 105),
                Height = 115,
                CheckOnClick = true,
                DisplayMember = nameof(Employee.FullName)
            };
            cmbEmployee.SelectedIndexChanged += (_, _) => RefreshCrewChoices();
            btnSave = new Button { Text = "Сохранить", Enabled = false };
            btnSave.Click += (_, _) => Save();

            FormLayout.ConfigureEditor(this, new System.Drawing.Size(610, 440), btnSave,
                ("Рейс:", cmbFlight),
                ("Самолет:", cmbAirplane),
                ("Ответственный:", cmbEmployee),
                ("Занято мест:", txtSeats),
                ("Дата полета:", dtpDate),
                ("Доп. экипаж:", crew));
        }

        private static ComboBox CreateChoice(string displayMember) => new()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = displayMember
        };

        private void LoadChoices()
        {
            if (!FormAction.Try(this, "Не удалось загрузить рейсы, самолеты или сотрудников. Проверьте подключение к базе данных.", () =>
            {
                cmbFlight.DataSource = _flights.ReadFlights().ToList();
                cmbAirplane.DataSource = _airplanes.ReadAirplanes().ToList();
                _crewChoices = _employees.ReadEmployees().ToArray();
                cmbEmployee.DataSource = _crewChoices;
                RefreshCrewChoices();
            }))
                return;

            btnSave.Enabled = cmbFlight.Items.Count > 0 && cmbAirplane.Items.Count > 0 && cmbEmployee.Items.Count > 0;
            if (!btnSave.Enabled)
                MessageBox.Show(this, "Сначала добавьте хотя бы один рейс, самолет и сотрудника.", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshCrewChoices()
        {
            var primary = cmbEmployee.SelectedItem as Employee;
            var checkedIds = crew.CheckedItems.Cast<Employee>().Select(employee => employee.Id).ToHashSet();
            crew.Items.Clear();
            foreach (var employee in _crewChoices.Where(employee => employee.Id != primary?.Id))
                crew.Items.Add(employee, checkedIds.Contains(employee.Id));
        }

        private void Save()
        {
            if (cmbFlight.SelectedItem is not Flight flight || cmbAirplane.SelectedItem is not Airplane airplane ||
                cmbEmployee.SelectedItem is not Employee employee || !int.TryParse(txtSeats.Text, out var seats) ||
                seats < 0 || seats > airplane.Capacity)
            {
                MessageBox.Show(this, "Выберите рейс, самолет и ответственного. Число занятых мест не должно превышать вместимость самолета.",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var date = dtpDate.Value.Date;
            var days = (DaysOfWeekFlag)(1 << (((int)date.DayOfWeek + 6) % 7));
            var crewIds = crew.CheckedItems.Cast<Employee>().Select(member => member.Id).ToArray();
            if (!FormAction.Try(this, "Не удалось сохранить выполнение полета.", () =>
                _repository.CreateFlightExecution(FlightExecution.CreateOperation(0, flight.Id, airplane.Id,
                    employee.Id, days, seats, date, crewIds))))
                return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
