using Microsoft.AspNetCore.Identity;

namespace CatalogoApi.Domain
{
    public class AplicationUser : IdentityUser
    {

        public string? RefreshToken { get; set; }
        public DateTime RefreshTTokenExpiryTime { get; set; }
    }
}
