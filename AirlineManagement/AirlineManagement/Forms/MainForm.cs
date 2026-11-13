using System;
using System.Windows.Forms;
using Unity;

namespace AirlineManagement.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupMenu();
        }

        private void SetupMenu()
        {
            this.Text = "Авиакомпания — Главное окно";
            this.Width = 850;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;

            var menuStrip = new MenuStrip();

            var dictionariesMenu = new ToolStripMenuItem("Справочники");
            var operationsMenu = new ToolStripMenuItem("Операции");
            var reportsMenu = new ToolStripMenuItem("Отчеты");

            dictionariesMenu.DropDownItems.Add(new ToolStripMenuItem("Самолеты", null, (s, e) => OpenForm<AirplanesForm>()));
            dictionariesMenu.DropDownItems.Add(new ToolStripMenuItem("Сотрудники", null, (s, e) => OpenForm<EmployeesForm>()));
            dictionariesMenu.DropDownItems.Add(new ToolStripMenuItem("Рейсы", null, (s, e) => OpenForm<FlightsForm>()));

            operationsMenu.DropDownItems.Add(new ToolStripMenuItem("Выполнение полетов", null, (s, e) => OpenForm<FlightExecutionsForm>()));
            operationsMenu.DropDownItems.Add(new ToolStripMenuItem("Продажа билетов", null, (s, e) => OpenForm<TicketSaleItemsForm>()));

            menuStrip.Items.Add(dictionariesMenu);
            menuStrip.Items.Add(operationsMenu);
            menuStrip.Items.Add(reportsMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void OpenForm<T>() where T : Form
        {
            var form = Program.Container.Resolve<T>();
            form.ShowDialog();
        }
    }
}