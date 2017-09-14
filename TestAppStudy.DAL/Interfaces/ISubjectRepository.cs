using TestAppStudy.DAL.Entities;

namespace TestAppStudy.DAL.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Subject Get(string subjectTitle);
        bool Delete(string subjectTitle);
    }
}
