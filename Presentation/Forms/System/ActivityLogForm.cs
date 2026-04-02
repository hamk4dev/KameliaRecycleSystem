using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Forms.System;

public class ActivityLogForm : Form
{
    public ActivityLogForm()
    {
        Text = "ActivityLogForm";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(960, 640);
        MinimumSize = new Size(720, 480);
    }
}
