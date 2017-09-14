
namespace TestAppStudy.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IStudentService CreateStudentService(string connection);
        ITeacherService CreateTeacherService(string connection);
        IAdminService CreateAdminService(string connection);
        ISubjectService CreateSubjectService(string connection);
        INoteService CreateNoteService(string connection);
    }
}
