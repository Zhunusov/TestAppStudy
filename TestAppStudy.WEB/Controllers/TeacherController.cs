using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.WEB.Models;

namespace TestAppStudy.WEB.Controllers
{
    [Authorize(Roles = "Администратор, Преподаватель")]
    public class TeacherController : ApiController
    {        
        private ITeacherService TeacherService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ITeacherService>();
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
        private IStudentService StudentService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<IStudentService>();
            }
        }

        [HttpPost]
        public ICollection<SubjectDTO> GetSubjectsByTeacher([FromBody] string username)
        {
            if (username != null)
                return SubjectService.GetSubjectsByTeacher(username);

            return null;
        }

        [HttpPost]
        public ICollection<NoteDTO> GetSubjectNotes([FromBody] SubjectNotesModel model)
        {
            if(ModelState.IsValid)
            {
                return NoteService.GetNotesBySubject(model.Title, model.UserName);
            }
            return null;
        }        

        [HttpPost]
        public ICollection<StudentDTO> GetStudentsOfSubject([FromBody] string subjectTitle)
        {
            if(subjectTitle != null)
            {
                return TeacherService.StudentsOfSubject(subjectTitle);
            }
            return null;
        }

        [HttpGet]
        public async Task<ICollection<StudentDTO>> GetAllStudents()
        {
            var students = await StudentService.GetAll();
            return students;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AssignSubjectToStudent([FromBody] AssignTeacherModel model)
        {
            if (ModelState.IsValid)
            {
                OperationDetails op = await StudentService.AddTeacherToStudent(model.SubjectTitle, model.TeacherUserName, model.StudentUserName);
                if (op.Succedeed)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(op.Property, op.Message);
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IHttpActionResult> CancelSubjectFromStudent([FromBody] AssignTeacherModel model)
        {
            if (ModelState.IsValid)
            {
                OperationDetails op = await StudentService.CancelTeacherToStudent(model.SubjectTitle, model.TeacherUserName, model.StudentUserName);
                if (op.Succedeed)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(op.Property, op.Message);
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
