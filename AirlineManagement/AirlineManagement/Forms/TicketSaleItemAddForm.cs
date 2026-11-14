using System;
using System.Windows.Forms;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class TicketSaleItemAddForm : Form
    {
        private readonly ITicketSaleItemRepository _repository;
        private TextBox txtFlightId, txtEmployeeId, txtCount, txtPrice;
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
            this.Width = 420;
            this.Height = 290;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblFlight = new Label { Text = "ID Рейса:", Left = 20, Top = 20, Width = 120 };
            txtFlightId = new TextBox { Left = 150, Top = 20, Width = 220 };

            var lblEmployee = new Label { Text = "ID Сотрудника:", Left = 20, Top = 60, Width = 120 };
            txtEmployeeId = new TextBox { Left = 150, Top = 60, Width = 220 };

            var lblCount = new Label { Text = "Кол-во билетов:", Left = 20, Top = 100, Width = 120 };
            txtCount = new TextBox { Left = 150, Top = 100, Width = 220 };

            var lblPrice = new Label { Text = "Цена за билет:", Left = 20, Top = 140, Width = 120 };
            txtPrice = new TextBox { Left = 150, Top = 140, Width = 220 };

            btnSave = new Button { Text = "Сохранить", Left = 150, Top = 195, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (!int.TryParse(txtFlightId.Text, out _) ||
                    !int.TryParse(txtEmployeeId.Text, out _) ||
                    !int.TryParse(txtCount.Text, out _) ||
                    !decimal.TryParse(txtPrice.Text, out _))
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
            this.Controls.Add(lblEmployee);
            this.Controls.Add(txtEmployeeId);
            this.Controls.Add(lblCount);
            this.Controls.Add(txtCount);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(btnSave);
        }
    }
}