using System.Linq;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class TicketSaleItemAddForm : Form
    {
        private readonly ITicketSaleItemRepository _repository;
        private readonly IFlightRepository _flights;
        private readonly IEmployeeRepository _employees;
        private ComboBox cmbFlight = null!, cmbEmployee = null!;
        private TextBox txtCount = null!, txtPrice = null!;
        private Button btnSave = null!;

        public TicketSaleItemAddForm(ITicketSaleItemRepository repository, IFlightRepository flights,
            IEmployeeRepository employees)
        {
            InitializeComponent();
            _repository = repository;
            _flights = flights;
            _employees = employees;
            SetupLayout();
            Shown += (_, _) => LoadChoices();
        }

        private void SetupLayout()
        {
            Text = "Добавить продажу билетов";
            cmbFlight = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = nameof(Flight.FlightNumber) };
            cmbEmployee = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = nameof(Employee.FullName) };
            txtCount = new TextBox();
            txtPrice = new TextBox();
            btnSave = new Button { Text = "Сохранить", Enabled = false };
            btnSave.Click += (_, _) => Save();

            FormLayout.ConfigureEditor(this, new System.Drawing.Size(560, 320), btnSave,
                ("Рейс:", cmbFlight),
                ("Сотрудник:", cmbEmployee),
                ("Кол-во билетов:", txtCount),
                ("Цена за билет:", txtPrice));
        }

        private void LoadChoices()
        {
            if (!FormAction.Try(this, "Не удалось загрузить рейсы или сотрудников. Проверьте подключение к базе данных.", () =>
            {
                cmbFlight.DataSource = _flights.ReadFlights().ToList();
                cmbEmployee.DataSource = _employees.ReadEmployees().ToList();
            }))
                return;

            btnSave.Enabled = cmbFlight.Items.Count > 0 && cmbEmployee.Items.Count > 0;
            if (!btnSave.Enabled)
                MessageBox.Show(this, "Сначала добавьте хотя бы один рейс и сотрудника.", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Save()
        {
            if (cmbFlight.SelectedItem is not Flight flight || cmbEmployee.SelectedItem is not Employee employee ||
                !int.TryParse(txtCount.Text, out var tickets) || tickets < 0 ||
                !decimal.TryParse(txtPrice.Text, out var price) || price < 0)
            {
                MessageBox.Show(this, "Выберите рейс и сотрудника. Количество билетов и цена должны быть неотрицательными.",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!FormAction.Try(this, "Не удалось сохранить продажу билетов.", () =>
                _repository.CreateTicketSaleItem(TicketSaleItem.CreateElement(0, flight.Id, employee.Id, tickets, price))))
                return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
