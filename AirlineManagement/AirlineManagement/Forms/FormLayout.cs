using System.Drawing;
using System.Windows.Forms;

namespace AirlineManagement.Forms;

internal static class FormLayout
{
    public static void ConfigureMain(Form form) => ConfigureWindow(
        form, new Size(900, 560), new Size(500, 300), FormStartPosition.CenterScreen);

    public static void ConfigureList(Form form, DataGridView grid, params Button[] actions)
    {
        form.SuspendLayout();
        var layout = CreateBodyLayout();
        grid.Dock = DockStyle.Fill;
        grid.Margin = Padding.Empty;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        layout.Controls.Add(grid, 0, 0);
        layout.Controls.Add(CreateActions(FlowDirection.LeftToRight, actions), 0, 1);
        form.Controls.Add(layout);
        ConfigureWindow(form, new Size(880, 520), new Size(540, 300), FormStartPosition.CenterParent);
        form.ResumeLayout(performLayout: true);
    }

    public static void ConfigureEditor(Form form, Size clientSize, Button save,
        params (string Caption, Control Editor)[] fields)
    {
        form.SuspendLayout();
        var layout = CreateBodyLayout();
        var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Margin = Padding.Empty };
        var inputs = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            RowCount = fields.Length
        };
        inputs.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < fields.Length; i++)
        {
            var (caption, editor) = fields[i];
            inputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            inputs.Controls.Add(new Label
            {
                Text = caption,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 16, 8),
                TabIndex = i * 2
            }, 0, i);
            editor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            editor.Margin = new Padding(0, 4, 2, 8);
            editor.TabIndex = i * 2 + 1;
            inputs.Controls.Add(editor, 1, i);
        }

        var cancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel };
        var actions = CreateActions(FlowDirection.RightToLeft, cancel, save);
        save.TabIndex = 0;
        cancel.TabIndex = 1;
        actions.TabIndex = 1;
        scroll.Controls.Add(inputs);
        layout.Controls.Add(scroll, 0, 0);
        layout.Controls.Add(actions, 0, 1);
        form.Controls.Add(layout);
        form.AcceptButton = save;
        form.CancelButton = cancel;
        form.MinimizeBox = false;
        ConfigureWindow(form, clientSize, new Size(360, 195), FormStartPosition.CenterParent);
        form.ResumeLayout(performLayout: true);
    }

    private static TableLayoutPanel CreateBodyLayout()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(14)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        return layout;
    }

    private static FlowLayoutPanel CreateActions(FlowDirection direction, params Button[] buttons)
    {
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = direction,
            WrapContents = true,
            Margin = new Padding(0, 12, 0, 0)
        };
        foreach (var button in buttons)
        {
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.Padding = new Padding(12, 5, 12, 5);
            button.Margin = new Padding(0, 0, 8, 0);
            actions.Controls.Add(button);
        }
        return actions;
    }

    private static void ConfigureWindow(Form form, Size clientSize, Size minimumClientSize, FormStartPosition position)
    {
        form.AutoScaleMode = AutoScaleMode.Dpi;
        form.AutoScaleDimensions = new SizeF(96, 96);
        form.FormBorderStyle = FormBorderStyle.Sizable;
        form.StartPosition = position;
        form.ClientSize = clientSize;
        form.MinimumSize = new Size(minimumClientSize.Width + form.Width - form.ClientSize.Width,
            minimumClientSize.Height + form.Height - form.ClientSize.Height);
        form.Load += (_, _) =>
        {
            var workArea = Screen.FromControl(form.Owner ?? form).WorkingArea.Size;
            form.MinimumSize = Limit(form.MinimumSize, workArea);
            form.Size = Limit(form.Size, workArea);
        };
    }

    private static Size Limit(Size size, Size limit) =>
        new(Math.Min(size.Width, limit.Width), Math.Min(size.Height, limit.Height));
}
