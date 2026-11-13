using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Entities.Enums;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class FlightExecutionAddForm : Form
    {
        private readonly IFlightExecutionRepository _repository;
        private TextBox txtFlightId, txtAirplaneId, txtSeats;
        private DateTimePicker dtpDate;
        private Button btnSave;

        public FlightExecutionAddForm(IFlightExecutionRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Text = "Добавить выполнение полета";
            this.Width = 420;
            this.Height = 280;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblFlight = new Label { Text = "ID Рейса:", Left = 20, Top = 20, Width = 120 };
            txtFlightId = new TextBox { Left = 150, Top = 20, Width = 220 };

            var lblPlane = new Label { Text = "ID Самолета:", Left = 20, Top = 60, Width = 120 };
            txtAirplaneId = new TextBox { Left = 150, Top = 60, Width = 220 };

            var lblSeats = new Label { Text = "Занято мест:", Left = 20, Top = 100, Width = 120 };
            txtSeats = new TextBox { Left = 150, Top = 100, Width = 220 };

            var lblDate = new Label { Text = "Дата полета:", Left = 20, Top = 140, Width = 120 };
            dtpDate = new DateTimePicker { Left = 150, Top = 140, Width = 220, Format = DateTimePickerFormat.Short };

            btnSave = new Button { Text = "Сохранить", Left = 150, Top = 190, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (!int.TryParse(txtFlightId.Text, out int fId) || !int.TryParse(txtAirplaneId.Text, out int aId) || !int.TryParse(txtSeats.Text, out int seats))
                {
                    MessageBox.Show("Проверьте числовые поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int newId = new Random().Next(10, 1000);
                var execution = FlightExecution.CreateOperation(newId, fId, aId, DaysOfWeekFlag.Monday, seats, dtpDate.Value);
                _repository.CreateFlightExecution(execution);

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblFlight);
            this.Controls.Add(txtFlightId);
            this.Controls.Add(lblPlane);
            this.Controls.Add(txtAirplaneId);
            this.Controls.Add(lblSeats);
            this.Controls.Add(txtSeats);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(btnSave);
        }
    }
}