using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAppStudy.DAL.Entities
{
    public class Subject
    {        
        [Key]
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Student> Students { get; set; } 
        public Subject()
        {
            Notes = new List<Note>();
            Teachers = new List<Teacher>();
            Students = new List<Student>();
        }
    }
}

