using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class FlightEditForm : Form
    {
        private readonly IFlightRepository _repository;
        private TextBox txtNumber, txtDestination;
        private DateTimePicker dtpDeparture, dtpArrival;
        private Button btnSave;

        public FlightEditForm(IFlightRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Text = "Добавить рейс";
            this.Width = 420;
            this.Height = 300;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblNum = new Label { Text = "Номер рейса:", Left = 20, Top = 20, Width = 120 };
            txtNumber = new TextBox { Left = 150, Top = 20, Width = 220 };

            var lblDest = new Label { Text = "Пункт назначения:", Left = 20, Top = 60, Width = 120 };
            txtDestination = new TextBox { Left = 150, Top = 60, Width = 220 };

            var lblDep = new Label { Text = "Время вылета:", Left = 20, Top = 100, Width = 120 };
            dtpDeparture = new DateTimePicker { Left = 150, Top = 100, Width = 220, Format = DateTimePickerFormat.Time, ShowUpDown = true };

            var lblArr = new Label { Text = "Время прилета:", Left = 20, Top = 140, Width = 120 };
            dtpArrival = new DateTimePicker { Left = 150, Top = 140, Width = 220, Format = DateTimePickerFormat.Time, ShowUpDown = true };

            btnSave = new Button { Text = "Сохранить", Left = 150, Top = 200, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtNumber.Text) || string.IsNullOrWhiteSpace(txtDestination.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int newId = new Random().Next(10, 1000);
                var flight = Flight.CreateEntity(newId, txtNumber.Text, txtDestination.Text, dtpDeparture.Value.TimeOfDay, dtpArrival.Value.TimeOfDay);
                _repository.CreateFlight(flight);

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblNum);
            this.Controls.Add(txtNumber);
            this.Controls.Add(lblDest);
            this.Controls.Add(txtDestination);
            this.Controls.Add(lblDep);
            this.Controls.Add(dtpDeparture);
            this.Controls.Add(lblArr);
            this.Controls.Add(dtpArrival);
            this.Controls.Add(btnSave);
        }
    }
}