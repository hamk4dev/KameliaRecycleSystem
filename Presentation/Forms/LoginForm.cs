using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Forms;

public class LoginForm : Form
{
    public LoginForm()
    {
        Text = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(960, 640);
        MinimumSize = new Size(720, 480);
    }
}
