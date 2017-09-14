using System;
using System.ComponentModel.DataAnnotations;

namespace TestAppStudy.WEB.Models
{
    public class EditPersonalInfoModel
    {        
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime? StudyStart { get; set; }
        public string Role { get; set; }

    }
}