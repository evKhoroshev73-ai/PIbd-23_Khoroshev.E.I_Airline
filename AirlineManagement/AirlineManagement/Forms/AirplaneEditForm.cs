using System.Diagnostics.CodeAnalysis;
using System;
using System.Windows.Forms;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Forms
{
    public partial class AirplaneEditForm : Form
    {
        private readonly IAirplaneRepository _repository;
        private int _id;
        private TextBox txtModel;
        private TextBox txtCapacity;
        private Button btnSave;

        public AirplaneEditForm(IAirplaneRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            SetupLayout();
        }

        public void SetEntry(Airplane item)
        {
            _id = item.Id;
            txtModel.Text = item.Model;
            txtCapacity.Text = item.Capacity.ToString();
        }

        [MemberNotNull(nameof(txtModel), nameof(txtCapacity), nameof(btnSave))]
        private void SetupLayout()
        {
            Text = "Самолет (Добавление / Редактирование)";
            txtModel = new TextBox();
            txtCapacity = new TextBox();
            btnSave = new Button { Text = "Сохранить" };

            btnSave.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtModel.Text) || (!int.TryParse(txtCapacity.Text, out var capacity) || capacity <= 0))
                {
                    MessageBox.Show("Заполните корректно все поля!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!FormAction.Try(this, "Не удалось сохранить запись.", () =>
                {
                    var item = Airplane.CreateEntity(_id, txtModel.Text.Trim(), capacity);
                    if (_id == 0)
                        _repository.CreateAirplane(item);
                    else
                        _repository.UpdateAirplane(item);
                }))
                    return;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            FormLayout.ConfigureEditor(this, new System.Drawing.Size(530, 260), btnSave,
                ("Модель:", txtModel),
                ("Вместимость:", txtCapacity));
        }
    }
}