using TestAppStudy.DAL.Identity;
using TestAppStudy.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace TestAppStudy.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IUserManager<Teacher> TeacherManager { get; }
        IUserManager<Student> StudentManager { get; }
        IUserManager<Admin> AdminManager { get; }
        ApplicationRoleManager RoleManager { get; }
        INoteRepository NoteRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        Task SaveAsync();
    }
}
