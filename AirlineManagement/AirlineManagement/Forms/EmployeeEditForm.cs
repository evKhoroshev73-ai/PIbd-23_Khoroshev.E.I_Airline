using System.Diagnostics.CodeAnalysis;
using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Entities.Enums;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class EmployeeEditForm : Form
    {
        private readonly IEmployeeRepository _repository;
        private int _id;
        private TextBox txtFullName;
        private ComboBox cmbPost;
        private Button btnSave;

        public EmployeeEditForm(IEmployeeRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        public void SetEntry(Employee item)
        {
            _id = item.Id;
            txtFullName.Text = item.FullName;
            cmbPost.SelectedItem = item.Post;
        }

        [MemberNotNull(nameof(txtFullName), nameof(cmbPost), nameof(btnSave))]
        private void SetupLayout()
        {
            Text = "Сотрудник (Добавление / Редактирование)";
            txtFullName = new TextBox();
            cmbPost = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true };
            cmbPost.Format += (_, e) =>
            {
                if (e.ListItem is EmployeePost post)
                    e.Value = FormAction.EmployeePostLabel(post);
            };
            foreach (var post in Enum.GetValues<EmployeePost>())
                cmbPost.Items.Add(post);
            cmbPost.SelectedIndex = 0;

            btnSave = new Button { Text = "Сохранить" };
            btnSave.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtFullName.Text) || cmbPost.SelectedItem is not EmployeePost post)
                {
                    MessageBox.Show("Заполните ФИО сотрудника!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!FormAction.Try(this, "Не удалось сохранить запись.", () =>
                {
                    var item = Employee.CreateEntity(_id, txtFullName.Text.Trim(), post);
                    if (_id == 0)
                        _repository.CreateEmployee(item);
                    else
                        _repository.UpdateEmployee(item);
                }))
                    return;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            FormLayout.ConfigureEditor(this, new System.Drawing.Size(530, 260), btnSave,
                ("ФИО:", txtFullName),
                ("Должность:", cmbPost));
        }
    }
}