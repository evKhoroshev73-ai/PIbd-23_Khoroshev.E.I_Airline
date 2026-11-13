using System;
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
            LoadData();
        }

        private void SetupLayout()
        {
            this.Text = "Операция: Выполнение полетов";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView { Dock = DockStyle.Top, Height = 330, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            btnAdd = new Button { Text = "Добавить выполнение", Left = 20, Top = 350, Width = 180, Height = 30 };

            btnAdd.Click += (s, e) => {
                var addForm = Program.Container.Resolve<FlightExecutionAddForm>();
                if (addForm.ShowDialog() == DialogResult.OK) LoadData();
            };

            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
        }

        private void LoadData()
        {
            grid.DataSource = null;
            grid.DataSource = _repository.ReadFlightExecutions();
        }
    }
}