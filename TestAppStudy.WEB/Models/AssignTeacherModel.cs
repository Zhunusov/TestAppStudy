using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestAppStudy.WEB.Models
{
    public class AssignTeacherModel
    {
        [Required]
        public string SubjectTitle { get; set; }
        [Required]
        public string TeacherUserName { get; set; }
        [Required]
        public string StudentUserName { get; set; }
    }
}