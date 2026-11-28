using System;
using System.Diagnostics;
using System.Windows.Forms;
using AirlineManagement.Entities.Enums;

namespace AirlineManagement.Forms
{
    internal static class FormAction
    {
        public static void SetColumnLabels(DataGridView grid, params (string Property, string Label)[] columns)
        {
            foreach (var (property, label) in columns)
                if (grid.Columns[property] is DataGridViewColumn column)
                    column.HeaderText = label;
        }

        public static string EmployeePostLabel(EmployeePost post) => post switch
        {
            EmployeePost.Pilot => "Пилот",
            EmployeePost.CoPilot => "Второй пилот",
            EmployeePost.FlightAttendant => "Бортпроводник",
            _ => "Не указана"
        };

        public static bool Try(IWin32Window owner, string message, Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.ToString());
                MessageBox.Show(owner, message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
