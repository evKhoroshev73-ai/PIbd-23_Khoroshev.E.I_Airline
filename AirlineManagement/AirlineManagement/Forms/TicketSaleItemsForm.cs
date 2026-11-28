using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using AirlineManagement.Repositories.Interfaces;
using Unity;

namespace AirlineManagement.Forms
{
    public partial class TicketSaleItemsForm : Form
    {
        private readonly ITicketSaleItemRepository _repository;
        private DataGridView grid;
        private Button btnAdd;

        public TicketSaleItemsForm(ITicketSaleItemRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
            Shown += (_, _) => LoadData();
        }

        [MemberNotNull(nameof(grid), nameof(btnAdd))]
        private void SetupLayout()
        {
            this.Text = "Операция: Продажа билетов";

            grid = new DataGridView { AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            btnAdd = new Button { Text = "Добавить продажу" };

            btnAdd.Click += (s, e) =>
            {
                bool saved = false;
                if (FormAction.Try(this, "Не удалось открыть форму добавления записи.", () =>
                {
                    using var form = Program.Container.Resolve<TicketSaleItemAddForm>();
                    saved = form.ShowDialog(this) == DialogResult.OK;
                }) && saved)
                    LoadData();
            };

            FormLayout.ConfigureList(this, grid, btnAdd);
        }

        private void LoadData()
        {
            FormAction.Try(this, "Не удалось загрузить записи. Проверьте подключение к базе данных.",
                () => grid.DataSource = _repository.ReadTicketSaleItems().Select(sale => new
                {
                    Код = sale.Id,
                    Рейс = sale.FlightId,
                    Сотрудник = sale.EmployeeId,
                    Количество = sale.TicketsSold,
                    Цена = sale.PricePerTicket
                }).ToList());
        }
    }
}