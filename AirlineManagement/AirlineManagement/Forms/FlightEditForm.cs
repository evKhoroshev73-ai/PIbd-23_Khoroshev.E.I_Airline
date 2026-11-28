using System.Diagnostics.CodeAnalysis;
using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class FlightEditForm : Form
    {
        private readonly IFlightRepository _repository;
        private int _id;
        private TextBox txtNumber, txtDestination;
        private DateTimePicker dtpDeparture, dtpArrival;
        private Button btnSave;

        public FlightEditForm(IFlightRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        public void SetEntry(Flight item)
        {
            _id = item.Id;
            txtNumber.Text = item.FlightNumber;
            txtDestination.Text = item.Destination;
            dtpDeparture.Value = DateTime.Today.Add(item.DepartureTime);
            dtpArrival.Value = DateTime.Today.Add(item.ArrivalTime);
        }

        [MemberNotNull(nameof(txtNumber), nameof(txtDestination), nameof(dtpDeparture), nameof(dtpArrival), nameof(btnSave))]
        private void SetupLayout()
        {
            Text = "Рейс (Добавление / Редактирование)";
            txtNumber = new TextBox();
            txtDestination = new TextBox();
            dtpDeparture = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true };
            dtpArrival = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true };
            btnSave = new Button { Text = "Сохранить" };
            btnSave.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtNumber.Text) || string.IsNullOrWhiteSpace(txtDestination.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!FormAction.Try(this, "Не удалось сохранить запись.", () =>
                {
                    var item = Flight.CreateEntity(_id, txtNumber.Text.Trim(), txtDestination.Text.Trim(), dtpDeparture.Value.TimeOfDay, dtpArrival.Value.TimeOfDay);
                    if (_id == 0)
                        _repository.CreateFlight(item);
                    else
                        _repository.UpdateFlight(item);
                }))
                    return;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            FormLayout.ConfigureEditor(this, new System.Drawing.Size(560, 320), btnSave,
                ("Номер рейса:", txtNumber),
                ("Пункт назначения:", txtDestination),
                ("Время вылета:", dtpDeparture),
                ("Время прилета:", dtpArrival));
        }
    }
}