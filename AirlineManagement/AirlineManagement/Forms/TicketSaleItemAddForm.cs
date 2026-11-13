using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class TicketSaleItemAddForm : Form
    {
        private readonly ITicketSaleItemRepository _repository;
        private TextBox txtFlightId, txtCount, txtPrice;
        private Button btnSave;

        public TicketSaleItemAddForm(ITicketSaleItemRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Text = "Добавить продажу билетов";
            this.Width = 400;
            this.Height = 250;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblFlight = new Label { Text = "ID Рейса:", Left = 20, Top = 20, Width = 110 };
            txtFlightId = new TextBox { Left = 140, Top = 20, Width = 210 };

            var lblCount = new Label { Text = "Кол-во билетов:", Left = 20, Top = 60, Width = 110 };
            txtCount = new TextBox { Left = 140, Top = 60, Width = 210 };

            var lblPrice = new Label { Text = "Цена за билет:", Left = 20, Top = 100, Width = 110 };
            txtPrice = new TextBox { Left = 140, Top = 100, Width = 210 };

            btnSave = new Button { Text = "Сохранить", Left = 140, Top = 150, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (!int.TryParse(txtFlightId.Text, out int fId) || !int.TryParse(txtCount.Text, out int count) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Проверьте корректность числовых полей!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int newId = new Random().Next(10, 1000);
                var item = TicketSaleItem.CreateElement(newId, fId, count, price);
                _repository.CreateTicketSaleItem(item);

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblFlight);
            this.Controls.Add(txtFlightId);
            this.Controls.Add(lblCount);
            this.Controls.Add(txtCount);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(btnSave);
        }
    }
}