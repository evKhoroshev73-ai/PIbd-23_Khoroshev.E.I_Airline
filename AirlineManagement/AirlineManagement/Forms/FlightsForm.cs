using System;
using System.Windows.Forms;
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
            LoadData();
        }

        private void SetupLayout()
        {
            this.Text = "Справочник: Рейсы";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView { Dock = DockStyle.Top, Height = 330, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            btnAdd = new Button { Text = "Добавить", Left = 20, Top = 350, Width = 110, Height = 30 };
            btnEdit = new Button { Text = "Редактировать", Left = 140, Top = 350, Width = 130, Height = 30 };
            btnDelete = new Button { Text = "Удалить", Left = 280, Top = 350, Width = 110, Height = 30 };

            btnAdd.Click += (s, e) => {
                var form = Program.Container.Resolve<FlightEditForm>();
                if (form.ShowDialog() == DialogResult.OK) LoadData();
            };

            btnDelete.Click += (s, e) => {
                if (grid.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
                    _repository.DeleteFlight(id);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Выберите рейс для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
        }

        private void LoadData()
        {
            grid.DataSource = null;
            grid.DataSource = _repository.ReadFlights();
        }
    }
}