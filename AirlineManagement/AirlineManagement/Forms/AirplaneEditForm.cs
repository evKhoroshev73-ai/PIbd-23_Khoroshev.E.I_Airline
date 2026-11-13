using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class AirplaneEditForm : Form
    {
        private readonly IAirplaneRepository _repository;
        private TextBox txtModel;
        private TextBox txtCapacity;
        private Button btnSave;

        public AirplaneEditForm(IAirplaneRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Text = "Добавить самолет";
            this.Width = 400;
            this.Height = 250;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblModel = new Label { Text = "Модель:", Left = 20, Top = 20, Width = 100 };
            txtModel = new TextBox { Left = 130, Top = 20, Width = 220 };

            var lblCapacity = new Label { Text = "Вместимость:", Left = 20, Top = 60, Width = 100 };
            txtCapacity = new TextBox { Left = 130, Top = 60, Width = 220 };

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 120, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtModel.Text) || !int.TryParse(txtCapacity.Text, out int capacity))
                {
                    MessageBox.Show("Проверьте правильность введенных данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int newId = new Random().Next(10, 1000);
                var airplane = Airplane.CreateEntity(newId, txtModel.Text, capacity);
                _repository.CreateAirplane(airplane);

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblModel);
            this.Controls.Add(txtModel);
            this.Controls.Add(lblCapacity);
            this.Controls.Add(txtCapacity);
            this.Controls.Add(btnSave);
        }
    }
}