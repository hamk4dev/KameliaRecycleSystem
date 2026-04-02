using KameliaRecycleSystem.Core.Entities;
using KameliaRecycleSystem.Core.Enums;
using KameliaRecycleSystem.Core.Interfaces;
using KameliaRecycleSystem.Infrastructure.Data;
using KameliaRecycleSystem.Infrastructure.Data.Repositories;
using KameliaRecycleSystem.Infrastructure.Security.Authentication;
using KameliaRecycleSystem.Presentation.Forms;
using KameliaRecycleSystem.Presentation.Forms.Auth;
using KameliaRecycleSystem.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace KameliaRecycleSystem;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();

            using var context = CreateDbContext();
            SeedDevelopmentData(context);

            IUserRepository userRepository = new UserRepository(context);
            var jwtService = new JwtService();
            var userViewModel = new UserViewModel(jwtService);
            var loginService = new LoginService(userRepository, jwtService);

            using var loginForm = new KameliaRecycleSystem.Presentation.Forms.Auth.LoginForm(loginService, jwtService, userViewModel);
            var loginResult = loginForm.ShowDialog();

            if (loginResult == DialogResult.OK && !string.IsNullOrWhiteSpace(userViewModel.Token))
            {
                System.Windows.Forms.Application.Run(new MainDashboardForm(userViewModel));
            }
        }
        catch (Exception ex)
        {
            WriteStartupError(ex);
            System.Windows.Forms.MessageBox.Show(
                $"Aplikasi gagal dijalankan. Detail error disimpan di DataStorage\\Logs\\startup-error.log{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                "Kamelia Recycle System",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("KameliaRecycleSystem")
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static void SeedDevelopmentData(AppDbContext context)
    {
        if (context.UserAccounts.Any())
        {
            return;
        }

        var hasher = new PasswordHasher();
        var admin = new UserAccount("admin", hasher.HashPassword(KameliaRecycleSystem.Shared.Constants.SecurityConstants.DefaultAdminPassword), UserRole.SuperAdmin)
        {
            Id = 1,
            Email = "admin@kamelia.local",
            IsActive = true,
            CreatedBy = "Program"
        };

        context.UserAccounts.Add(admin);
        context.SaveChanges();
    }

    private static void WriteStartupError(Exception exception)
    {
        try
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "DataStorage", "Logs");
            Directory.CreateDirectory(logDirectory);

            var logPath = Path.Combine(logDirectory, "startup-error.log");
            var builder = new StringBuilder();
            builder.AppendLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            builder.AppendLine(exception.ToString());
            builder.AppendLine(new string('-', 80));

            File.AppendAllText(logPath, builder.ToString());
        }
        catch
        {
            // Ignore secondary logging failures.
        }
    }
}
