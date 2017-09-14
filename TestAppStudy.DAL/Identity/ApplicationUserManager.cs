using TestAppStudy.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace TestAppStudy.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager (IUserStore<ApplicationUser> store)
            : base(store)
        {

        }
    }
}
