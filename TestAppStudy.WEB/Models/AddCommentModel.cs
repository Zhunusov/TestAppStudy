using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestAppStudy.WEB.Models
{
    public class AddCommentModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        [Required]
        public string SubjectTitle { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}