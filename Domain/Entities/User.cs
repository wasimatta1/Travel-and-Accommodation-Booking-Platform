using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        //Navigation Properties

        public ICollection<Hotel>? Hotels { get; set; } = new List<Hotel>();

    }
}
