using Microsoft.AspNetCore.Identity;

namespace WebBanHangLapTop.Models
{
    public class ApplicationUser :IdentityUser
    {
     
        public string? FullName  { get; set; }
    }
}
