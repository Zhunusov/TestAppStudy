using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;

namespace TestAppStudy.BLL.Interfaces
{
    public interface IStudentService : IUserService<StudentDTO>
    {
        Task<OperationDetails> AddTeacherToStudent(string subjectTitle, string teacherUserName, string studentUserName);
        Task<OperationDetails> CancelTeacherToStudent(string subjectTitle, string teacherUserName, string studentUserName);
        
    }
}
