using System;
using System.Windows.Forms;
using AirlineManagement.Entities.Enums;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class EmployeeEditForm : Form
    {
        private readonly IEmployeeRepository _repository;
        private TextBox txtFullName;
        private ComboBox cmbPost;
        private Button btnSave;

        public EmployeeEditForm(IEmployeeRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        private void SetupLayout()
        {
            this.Text = "Сотрудник (Добавление / Редактирование)";
            this.Width = 400;
            this.Height = 250;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblName = new Label { Text = "ФИО:", Left = 20, Top = 20, Width = 100 };
            txtFullName = new TextBox { Left = 130, Top = 20, Width = 220 };

            var lblPost = new Label { Text = "Должность:", Left = 20, Top = 60, Width = 100 };
            cmbPost = new ComboBox { Left = 130, Top = 60, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPost.DataSource = Enum.GetValues(typeof(EmployeePost));

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 120, Width = 100, Height = 30 };
            btnSave.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Заполните ФИО сотрудника!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Данные успешно проверены (режим заглушки).", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblPost);
            this.Controls.Add(cmbPost);
            this.Controls.Add(btnSave);
        }
    }
}