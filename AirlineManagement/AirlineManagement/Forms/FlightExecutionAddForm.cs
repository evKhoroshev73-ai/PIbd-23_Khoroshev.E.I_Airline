using System;
using System.Windows.Forms;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class FlightExecutionAddForm : Form
    {
        private readonly IFlightExecutionRepository _repository;
        private TextBox txtFlightId, txtAirplaneId, txtEmployeeId, txtSeats;
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
            this.Height = 320;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblFlight = new Label { Text = "ID Рейса:", Left = 20, Top = 20, Width = 120 };
            txtFlightId = new TextBox { Left = 150, Top = 20, Width = 220 };

            var lblPlane = new Label { Text = "ID Самолета:", Left = 20, Top = 60, Width = 120 };
            txtAirplaneId = new TextBox { Left = 150, Top = 60, Width = 220 };

            var lblEmployee = new Label { Text = "ID Сотрудника:", Left = 20, Top = 100, Width = 120 };
            txtEmployeeId = new TextBox { Left = 150, Top = 100, Width = 220 };

            var lblSeats = new Label { Text = "Занято мест:", Left = 20, Top = 140, Width = 120 };
            txtSeats = new TextBox { Left = 150, Top = 140, Width = 220 };

            var lblDate = new Label { Text = "Дата полета:", Left = 20, Top = 180, Width = 120 };
            dtpDate = new DateTimePicker { Left = 150, Top = 180, Width = 220, Format = DateTimePickerFormat.Short };

            btnSave = new Button { Text = "Сохранить", Left = 150, Top = 230, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (!int.TryParse(txtFlightId.Text, out _) ||
                    !int.TryParse(txtAirplaneId.Text, out _) ||
                    !int.TryParse(txtEmployeeId.Text, out _) ||
                    !int.TryParse(txtSeats.Text, out _))
                {
                    MessageBox.Show("Проверьте корректность числовых полей!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Данные успешно проверены (режим заглушки).", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblFlight);
            this.Controls.Add(txtFlightId);
            this.Controls.Add(lblPlane);
            this.Controls.Add(txtAirplaneId);
            this.Controls.Add(lblEmployee);
            this.Controls.Add(txtEmployeeId);
            this.Controls.Add(lblSeats);
            this.Controls.Add(txtSeats);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(btnSave);
        }
    }
}