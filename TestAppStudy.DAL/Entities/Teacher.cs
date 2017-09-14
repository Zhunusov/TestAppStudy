using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TestAppStudy.DAL.Entities
{
    public class Teacher
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<Student> Students { get; set; }
        public Teacher()
        {
            Notes = new List<Note>();
            Subjects = new List<Subject>();
            Students = new List<Student>();
        }
    }
}
