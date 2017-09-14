using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.BLL.DTO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestAppStudy.BLL.Interfaces
{
    public interface ISubjectService : IDisposable
    {
        Task<OperationDetails> CreateSubject(SubjectDTO item);
        ICollection<SubjectDTO> GetAllSubjects();
        SubjectDTO GetSubject(string title);
        Task<OperationDetails> UpdateSubject(SubjectDTO subject);
        Task<OperationDetails> RemoveSubject(string title);
        ICollection<SubjectDTO> GetSubjectsByTeacher(string teacherUserName);
        ICollection<SubjectDTO> GetSubjectsByStudent(string studentUserName);
    }
}
