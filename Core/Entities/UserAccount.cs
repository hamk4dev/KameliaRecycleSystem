using System;
using KameliaRecycleSystem.Core.Enums;

namespace KameliaRecycleSystem.Core.Entities
{
    public class UserAccount : BaseEntity
    {
        // REQUIRED PROPERTIES UNTUK AUTHENTICATION FLOW
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        
        // AUDIT & SECURITY (sesuai roadmap debug)
        public DateTime? LastLogin { get; set; }
        // OPTIONAL: Basic security (bisa di-develop nanti)
        public int LoginAttempt { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }
        
        // RELATIONSHIP DENGAN WARGA (optional - sesuai diskusi)
        public int? WargaId { get; set; }
        public virtual Warga Warga { get; set; }

        // CONSTRUCTOR UNTUK CONSISTENCY
        public UserAccount() { }
        
        public UserAccount(string username, string passwordHash, UserRole role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            CreatedDate = DateTime.Now;
        }
        
        // BASIC BUSINESS METHODS UNTUK AUTH FLOW
        public void UpdateLastLogin()
        {
            LastLogin = DateTime.Now;
            LoginAttempt = 0; // Reset attempts on successful login
        }
        
        public bool CanLogin()
        {
            return IsActive && 
                   (LockoutEnd == null || LockoutEnd < DateTime.Now);
        }
    }
}
