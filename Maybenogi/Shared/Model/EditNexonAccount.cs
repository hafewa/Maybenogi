using System;
using System.Security.Cryptography;

namespace Maybenogi.Shared.Model
{
    public class EditNexonAccount
    {
        public long UID { get; set; }

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ENexonAccountType AccountType { get; set; }
        public bool IsActive { get; set; }

        public string Description { get; set; }

        public DateTime LastSignedInTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        // temp values
        public bool IsDeleting { get; set; }

        public EditNexonAccount Import(NexonAccount modified)
        {
            UID = modified.UID;
            DisplayName = modified.DisplayName;
            Email = modified.Email;
            Password = modified.Password;
            AccountType = modified.AccountType;
            IsActive = modified.IsActive;
            Description = modified.Description;
            LastSignedInTime = modified.LastSignedInTime;
            LastModifiedTime = modified.LastModifiedTime;
            IsDeleting = modified.IsDeleting;

            return this;
        }
    }
}