using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestAppStudy.DAL.Entities;

namespace TestAppStudy.DAL.EF
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }        
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
