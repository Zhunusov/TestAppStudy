using System.Collections.Generic;

namespace TestAppStudy.BLL.DTO
{
    public class TeacherDTO : UserDTO
    {
        public ICollection<SubjectDTO> SubjectsDTO { get; set; }
        public TeacherDTO()
        {
            SubjectsDTO = new List<SubjectDTO>();
        }
    }
}
