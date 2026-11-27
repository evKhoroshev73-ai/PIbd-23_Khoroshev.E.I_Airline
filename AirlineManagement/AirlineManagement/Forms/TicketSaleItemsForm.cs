using System;
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
            LoadData();
        }

        private void SetupLayout()
        {
            this.Text = "Операция: Продажа билетов";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView { Dock = DockStyle.Top, Height = 330, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            btnAdd = new Button { Text = "Добавить продажу", Left = 20, Top = 350, Width = 160, Height = 30 };

            btnAdd.Click += (s, e) => {
                var form = Program.Container.Resolve<TicketSaleItemAddForm>();
                form.ShowDialog();
            };

            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
        }

        private void LoadData()
        {
            grid.DataSource = null;
            grid.DataSource = _repository.ReadTicketSaleItems();
        }
    }
}