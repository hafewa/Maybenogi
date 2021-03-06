using System;
using System.Security.Cryptography;

namespace Maybenogi.Shared.Model
{
    public class NexonAccount
    {
        public long UID { get; set; }

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ENexonAccountType AccountType { get; set; }
        public bool IsActive { get; set; }

        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime LastSignedInTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        // temp values
        public bool IsDeleting { get; set; }

        public void Import(EditNexonAccount acc)
        {
            DisplayName = acc.DisplayName;
            Email = acc.Email;
            Password = acc.Password;
            AccountType = acc.AccountType;
            IsActive = acc.IsActive;
            Description = acc.Description;
            LastSignedInTime = acc.LastSignedInTime;
            LastModifiedTime = acc.LastModifiedTime;
            IsDeleting = acc.IsDeleting;
        }
    }
}
