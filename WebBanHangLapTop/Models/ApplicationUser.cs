using Microsoft.AspNetCore.Identity;

namespace WebBanHangLapTop.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string? Address { get; set; }   
        public int? Age { get; set; }

        public string? FullName  { get; set; }
    }
}
