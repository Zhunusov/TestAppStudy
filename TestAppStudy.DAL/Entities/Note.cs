using System.ComponentModel.DataAnnotations.Schema;

namespace TestAppStudy.DAL.Entities
{
    public class Note
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }

        [ForeignKey("Subject")]
        public string SubjectTitle { get; set; }
        public Subject Subject { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

}
