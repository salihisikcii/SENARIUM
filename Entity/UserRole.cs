using Microsoft.AspNetCore.Identity;

namespace BlogApps.Entity
{
    public class UserRole : IdentityRole
    {
        // Parametresiz yapılandırma eklendi
        public UserRole() : base()
        {
        }

        public UserRole(string roleName) : base(roleName)
        {
        }
    }
}
