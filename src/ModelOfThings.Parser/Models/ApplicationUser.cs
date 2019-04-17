using Microsoft.AspNetCore.Identity;

namespace ModelOfThings.Parser.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
