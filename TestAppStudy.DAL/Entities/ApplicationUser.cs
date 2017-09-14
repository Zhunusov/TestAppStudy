using Microsoft.AspNet.Identity.EntityFramework;

namespace TestAppStudy.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {        
        public virtual Teacher Teacher { get; set; }
        public virtual Student Student { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
