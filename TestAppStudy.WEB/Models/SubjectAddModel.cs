using System.ComponentModel.DataAnnotations;

namespace TestAppStudy.WEB.Models
{
    public class SubjectAddModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class SubjectAssignToTeacherModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string TeacherName { get; set; }
    }
}