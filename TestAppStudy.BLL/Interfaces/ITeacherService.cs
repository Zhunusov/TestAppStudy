using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;

namespace TestAppStudy.BLL.Interfaces
{
    public interface ITeacherService : IUserService<TeacherDTO>
    {
        Task<OperationDetails> AddSubjectToTeacher(string subjectTitle, string teacherName);
        Task<OperationDetails> CancelSubjectFromTeacher(string subjectTitle, string teacherName);
        ICollection<TeacherDTO> TeachersOfSubject(string subjectTitle);
        ICollection<StudentDTO> StudentsOfSubject(string title);
    }
}
