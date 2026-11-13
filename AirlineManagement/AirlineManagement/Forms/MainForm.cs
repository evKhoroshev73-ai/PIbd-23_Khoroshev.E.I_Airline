using System;
using System.Windows.Forms;
using Unity;

namespace AirlineManagement.Forms
{
    public partial class MainForm : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem dictionariesMenuItem;
        private ToolStripMenuItem operationsMenuItem;
        private ToolStripMenuItem reportsMenuItem;

        private ToolStripMenuItem airplanesMenuItem;
        private ToolStripMenuItem employeesMenuItem;
        private ToolStripMenuItem flightsMenuItem;

        private ToolStripMenuItem flightExecutionsMenuItem;
        private ToolStripMenuItem ticketSalesMenuItem;

        public MainForm()
        {
            InitializeComponent();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            this.Text = "Авиакомпания — Главное окно";
            this.Width = 800;
            this.Height = 500;

            menuStrip = new MenuStrip();

            dictionariesMenuItem = new ToolStripMenuItem("Справочники");
            operationsMenuItem = new ToolStripMenuItem("Операции");
            reportsMenuItem = new ToolStripMenuItem("Отчеты");

            airplanesMenuItem = new ToolStripMenuItem("Самолеты", null, OnAirplanesClick);
            employeesMenuItem = new ToolStripMenuItem("Сотрудники", null, OnEmployeesClick);
            flightsMenuItem = new ToolStripMenuItem("Рейсы", null, OnFlightsClick);

            dictionariesMenuItem.DropDownItems.Add(airplanesMenuItem);
            dictionariesMenuItem.DropDownItems.Add(employeesMenuItem);
            dictionariesMenuItem.DropDownItems.Add(flightsMenuItem);

            flightExecutionsMenuItem = new ToolStripMenuItem("Выполнение полетов", null, OnFlightExecutionsClick);
            ticketSalesMenuItem = new ToolStripMenuItem("Продажа билетов", null, OnTicketSalesClick);

            operationsMenuItem.DropDownItems.Add(flightExecutionsMenuItem);
            operationsMenuItem.DropDownItems.Add(ticketSalesMenuItem);

            menuStrip.Items.Add(dictionariesMenuItem);
            menuStrip.Items.Add(operationsMenuItem);
            menuStrip.Items.Add(reportsMenuItem); 

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void OnAirplanesClick(object sender, EventArgs e)
        {
            MessageBox.Show("Открытие формы справочника: Самолеты");
        }

        private void OnEmployeesClick(object sender, EventArgs e)
        {
            MessageBox.Show("Открытие формы справочника: Сотрудники");
        }

        private void OnFlightsClick(object sender, EventArgs e)
        {
            MessageBox.Show("Открытие формы справочника: Рейсы");
        }

        private void OnFlightExecutionsClick(object sender, EventArgs e)
        {
            MessageBox.Show("Открытие формы операции: Выполнение полетов");
        }

        private void OnTicketSalesClick(object sender, EventArgs e)
        {
            MessageBox.Show("Открытие формы операции: Продажа билетов");
        }
    }
}