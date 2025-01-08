using System.ComponentModel.DataAnnotations;

namespace CustomIdentity.ViewModels
{
    public class AccountVM
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
    }
}
