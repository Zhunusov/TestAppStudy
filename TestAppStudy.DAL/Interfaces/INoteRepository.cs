using TestAppStudy.DAL.Entities;

namespace TestAppStudy.DAL.Interfaces
{
    public interface INoteRepository : IRepository<Note>
    {
        Note Get(int id);
        bool Delete(int id);
    }
}
