using System.Drawing;
using System.Windows.Forms;
using KameliaRecycleSystem.Presentation.ViewModels;

namespace KameliaRecycleSystem.Presentation.Forms;

public class MainDashboardForm : Form
{
    private readonly UserViewModel _userViewModel;

    public MainDashboardForm(UserViewModel userViewModel)
    {
        _userViewModel = userViewModel ?? throw new ArgumentNullException(nameof(userViewModel));
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Kamelia Recycle System - Dashboard";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1100, 720);
        MinimumSize = new Size(860, 540);
        BackColor = Color.FromArgb(245, 247, 250);

        var headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 96,
            BackColor = Color.FromArgb(33, 37, 41),
            Padding = new Padding(24, 18, 24, 18)
        };

        var titleLabel = new Label
        {
            Text = "Dashboard TPS3R Kamelia",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 14)
        };

        var userLabel = new Label
        {
            Text = $"Masuk sebagai: {_userViewModel.GetUserInfo()}",
            ForeColor = Color.Gainsboro,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(22, 54)
        };

        var logoutButton = new Button
        {
            Text = "Keluar",
            Width = 110,
            Height = 36,
            BackColor = Color.FromArgb(220, 53, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(950, 28)
        };
        logoutButton.FlatAppearance.BorderSize = 0;
        logoutButton.Click += (_, _) => Close();

        headerPanel.Controls.Add(titleLabel);
        headerPanel.Controls.Add(userLabel);
        headerPanel.Controls.Add(logoutButton);

        var contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            ColumnCount = 2,
            RowCount = 2,
            BackColor = Color.Transparent
        };
        contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        contentPanel.Controls.Add(CreateCard("Manajemen Warga", "Kelola data warga, keanggotaan, dan relasi akun pengguna."), 0, 0);
        contentPanel.Controls.Add(CreateCard("Keuangan", "Pantau pemasukan, pengeluaran, dan ringkasan saldo operasional."), 1, 0);
        contentPanel.Controls.Add(CreateCard("Laporan", "Siapkan laporan keuangan, sampah, warga, dan audit aktivitas."), 0, 1);
        contentPanel.Controls.Add(CreateCard("Sistem", "Akses pengaturan, backup, dan utilitas administrasi sistem."), 1, 1);

        Controls.Add(contentPanel);
        Controls.Add(headerPanel);
    }

    private static Control CreateCard(string title, string description)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(12),
            BackColor = Color.White,
            Padding = new Padding(20)
        };

        var titleLabel = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.FromArgb(33, 37, 41),
            AutoSize = true,
            MaximumSize = new Size(420, 0),
            Location = new Point(18, 18)
        };

        var descriptionLabel = new Label
        {
            Text = description,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.FromArgb(90, 98, 104),
            AutoSize = true,
            MaximumSize = new Size(420, 0),
            Location = new Point(20, 64)
        };

        card.Controls.Add(titleLabel);
        card.Controls.Add(descriptionLabel);

        return card;
    }
}
