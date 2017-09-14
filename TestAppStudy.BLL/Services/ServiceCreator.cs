using TestAppStudy.BLL.Interfaces;
using TestAppStudy.DAL.Repositories;

namespace TestAppStudy.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IStudentService CreateStudentService(string connection)
        {
            return new StudentService(new IdentityUnitOfWork(connection));
        }

        public ITeacherService CreateTeacherService(string connection)
        {
            return new TeacherService(new IdentityUnitOfWork(connection));
        }

        public IAdminService CreateAdminService(string connection)
        {
            return new AdminService(new IdentityUnitOfWork(connection));
        }

        public ISubjectService CreateSubjectService(string connection)
        {
            return new SubjectService(new IdentityUnitOfWork(connection));
        }

        public INoteService CreateNoteService(string connection)
        {
            return new NoteService(new IdentityUnitOfWork(connection));
        }
    }
}
