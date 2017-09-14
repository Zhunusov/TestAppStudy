using System;
using System.Collections.Generic;

namespace TestAppStudy.BLL.DTO
{
    public class StudentDTO  : UserDTO
    {        
        public DateTime? StudyStart { get; set; } 
        public string StudyStart_Short { get; set; }
        public Dictionary<string,List<TeacherDTO>> SubjectsDTO { get; set; }        
        public StudentDTO()
        {
            SubjectsDTO = new Dictionary<string, List<TeacherDTO>>();
          
        }
    }
}
