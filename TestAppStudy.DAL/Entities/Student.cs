using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAppStudy.DAL.Entities
{
    public class Student
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime? StudyStart { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

        public Student()
        {
            Notes = new List<Note>();
            Subjects = new List<Subject>();
            Teachers = new List<Teacher>();
        }
    }
}
