using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.WEB.Models;

namespace TestAppStudy.WEB.Controllers
{
    [Authorize(Roles = "Администратор, Студент")]
    public class StudentController : ApiController
    {        
        private IStudentService StudentService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<IStudentService>();
            }
        }
        private ISubjectService SubjectService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ISubjectService>();
            }
        }

        private INoteService NoteService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<INoteService>();
            }
        }

        [HttpPost]
        public ICollection<SubjectDTO> GetSubjectsByStudent([FromBody] string username)
        {
            if (username != null)
                return SubjectService.GetSubjectsByStudent(username);

            return null;
        }

        [HttpPost]
        public ICollection<NoteDTO> GetSubjectNotes([FromBody] SubjectNotesModel model)
        {
            if (ModelState.IsValid)
            {
                return NoteService.GetNotesBySubject(model.Title, model.UserName);
            }
            return null;
        }
    }
}
