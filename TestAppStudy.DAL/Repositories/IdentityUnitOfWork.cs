using TestAppStudy.DAL.EF;
using TestAppStudy.DAL.Entities;
using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;


namespace TestAppStudy.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private StudentManager studentManager;
        private TeacherManager teacherManager;
        private AdminManager adminManager;
        private NoteRepository noteRepository;
        private SubjectRepository subjectRepository;
        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if(userManager == null)
                    userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

                return userManager;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                if(roleManager== null)
                    roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));

                return roleManager;
            }
        }
        public IUserManager<Teacher> TeacherManager
        {
            get
            {
                if (teacherManager == null)
                    teacherManager = new TeacherManager(db);

                return teacherManager;
            }
        }

        public IUserManager<Student> StudentManager
        {
            get
            {
                if (studentManager == null)
                    studentManager = new StudentManager(db);

                return studentManager;
            }
        }
        public IUserManager<Admin> AdminManager
        {
            get
            {
                if (adminManager == null)
                    adminManager = new AdminManager(db);

                return adminManager;
            }
        }

        public INoteRepository NoteRepository
        {
            get
            {
                if (noteRepository == null)
                    noteRepository = new NoteRepository(db);

                return noteRepository;
            }
        }

        public ISubjectRepository SubjectRepository
        {
            get
            {
                if (subjectRepository == null)
                    subjectRepository = new SubjectRepository(db);

                return subjectRepository;
            }
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
