using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using KameliaRecycleSystem.Core.DTOs.Requests;
using KameliaRecycleSystem.Core.DTOs.Responses;
using KameliaRecycleSystem.Infrastructure.Security.Authentication;
using KameliaRecycleSystem.Presentation.ViewModels;

namespace KameliaRecycleSystem.Presentation.Forms.Auth
{
    /// <summary>
    /// Login form for user authentication
    /// Minimal UI integrated with LoginService and UserViewModel
    /// Perfectly follows Authentication Flow from roadmap
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly LoginService _loginService;
        private readonly UserViewModel _userViewModel;
        private readonly JwtService _jwtService;

        // UI Controls
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkRememberMe;
        private Button btnLogin;
        private Button btnCancel;
        private Label lblStatus;
        private Panel panelLogin;

        public LoginForm(LoginService loginService, JwtService jwtService, UserViewModel userViewModel)
        {
            _loginService = loginService;
            _jwtService = jwtService;
            _userViewModel = userViewModel;
            
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "Kamelia Recycle System - Login";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Main panel
            panelLogin = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            // Title label
            var lblTitle = new Label
            {
                Text = "LOGIN SISTEM",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                AutoSize = true,
                Location = new Point(100, 20)
            };

            // Username label and textbox
            var lblUsername = new Label
            {
                Text = "Username:",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(50, 80)
            };

            txtUsername = new TextBox
            {
                Location = new Point(50, 105),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9),
                PlaceholderText = "Masukkan username"
            };

            // Password label and textbox
            var lblPassword = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(50, 150)
            };

            txtPassword = new TextBox
            {
                Location = new Point(50, 175),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9),
                UseSystemPasswordChar = true,
                PlaceholderText = "Masukkan password"
            };

            // Remember me checkbox
            chkRememberMe = new CheckBox
            {
                Text = "Ingat saya",
                Font = new Font("Segoe UI", 8),
                AutoSize = true,
                Location = new Point(50, 210)
            };

            // Status label
            lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8),
                AutoSize = true,
                Location = new Point(50, 240),
                ForeColor = Color.Red,
                Visible = false
            };

            // Login button
            btnLogin = new Button
            {
                Text = "LOGIN",
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(140, 35),
                Location = new Point(50, 270),
                Cursor = Cursors.Hand
            };

            // Cancel button
            btnCancel = new Button
            {
                Text = "BATAL",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                Size = new Size(140, 35),
                Location = new Point(210, 270),
                Cursor = Cursors.Hand
            };

            // Add controls to panel
            panelLogin.Controls.AddRange(new Control[]
            {
                lblTitle, lblUsername, txtUsername, lblPassword, txtPassword,
                chkRememberMe, lblStatus, btnLogin, btnCancel
            });

            // Add panel to form
            this.Controls.Add(panelLogin);

            // Event handlers
            btnLogin.Click += BtnLogin_Click;
            btnCancel.Click += BtnCancel_Click;
            txtUsername.KeyPress += TxtUsername_KeyPress;
            txtPassword.KeyPress += TxtPassword_KeyPress;
            this.Load += LoginForm_Load;
        }

        private void SetupUI()
        {
            // Set initial focus
            txtUsername.Focus();

            // Enable enter key to trigger login
            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;
        }

        // ===== EVENT HANDLERS =====

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            await AttemptLogin();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                txtPassword.Focus();
            }
        }

        private async void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await AttemptLogin();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Clear any previous status
            ClearStatus();

            // Auto-focus username
            txtUsername.Focus();
        }

        // ===== LOGIN LOGIC =====

        private async Task AttemptLogin()
        {
            try
            {
                // Validate input
                if (!ValidateInput())
                    return;

                // Show loading state
                SetLoadingState(true);

                // Create login request
                var loginRequest = new LoginRequest
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text,
                    RememberMe = chkRememberMe.Checked
                };

                // Call LoginService for authentication
                var loginResponse = await _loginService.AuthenticateAsync(loginRequest);

                // Handle response
                await HandleLoginResponse(loginResponse);
            }
            catch (Exception ex)
            {
                ShowError($"Terjadi kesalahan sistem: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private bool ValidateInput()
        {
            ClearStatus();

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Username wajib diisi");
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Password wajib diisi");
                txtPassword.Focus();
                return false;
            }

            if (txtUsername.Text.Trim().Length < 3)
            {
                ShowError("Username minimal 3 karakter");
                txtUsername.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                ShowError("Password minimal 6 karakter");
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private async Task HandleLoginResponse(LoginResponse loginResponse)
        {
            if (loginResponse == null)
            {
                ShowError("Tidak ada respons dari server");
                return;
            }

            if (loginResponse.IsSuccess)
            {
                // Update UserViewModel with successful login data
                _userViewModel.UpdateFromLoginResponse(loginResponse);

                // Show success message
                ShowSuccess($"Login berhasil! Selamat datang {_userViewModel.DisplayName}");

                // Close form with OK result
                await Task.Delay(500); // Brief delay to show success message
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Show error message from LoginService
                ShowError(loginResponse.Message);
                
                // Clear password field for security
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // ===== UI HELPER METHODS =====

        private void SetLoadingState(bool isLoading)
        {
            btnLogin.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;
            txtUsername.Enabled = !isLoading;
            txtPassword.Enabled = !isLoading;
            chkRememberMe.Enabled = !isLoading;

            if (isLoading)
            {
                btnLogin.Text = "MEMPROSES...";
                btnLogin.BackColor = Color.Gray;
            }
            else
            {
                btnLogin.Text = "LOGIN";
                btnLogin.BackColor = Color.DarkGreen;
            }

            global::System.Windows.Forms.Application.DoEvents();
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Visible = true;
        }

        private void ShowSuccess(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.DarkGreen;
            lblStatus.Visible = true;
        }

        private void ClearStatus()
        {
            lblStatus.Text = "";
            lblStatus.Visible = false;
        }

        // ===== FORM OVERRIDES =====

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Ensure clean state if cancelled
            if (DialogResult != DialogResult.OK)
            {
                _userViewModel.Clear();
            }
            base.OnFormClosing(e);
        }
    }
}
