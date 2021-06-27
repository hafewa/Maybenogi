using System.ComponentModel.DataAnnotations;

namespace Maybenogi.Shared.Model
{
    public class CreateNexonAccount
    {
        [Required] public string DisplayName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public ENexonAccountType AccountType { get; set; } = ENexonAccountType.Nexon;
        public string Description { get; set; }
    }
}