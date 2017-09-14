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
    [Authorize(Roles = "Администратор")]
    public class AdminController : ApiController
    {
        private IStudentService StudentService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<IStudentService>();
            }
        }
        private ITeacherService TeacherService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ITeacherService>();
            }
        }

        private IAdminService AdminService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<IAdminService>();
            }
        }

        private ISubjectService SubjectService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ISubjectService>();
            }
        }

        [HttpGet]
        public async Task<ICollection<TeacherDTO>> GetAllTeachers()
        {
            var teachers = await TeacherService.GetAll();
            return teachers;
        }

        [HttpPost]
        public async Task<UserDTO> GetUserInfo([FromBody]string username)
        {
            if (username != null)
            {
                var role = await AdminService.GetRole(username);
                if (role != null)
                {
                    switch (role)
                    {
                        case "Студент":
                            StudentDTO studentDto = new StudentDTO();
                            OperationDetails opStu = StudentService.GetPersonalInfo(username, out studentDto);
                            if (opStu.Succedeed)
                            {
                                return studentDto;
                            }
                            break;

                        case "Преподаватель":
                            TeacherDTO teacherDto = new TeacherDTO();
                            OperationDetails opTeach = TeacherService.GetPersonalInfo(username, out teacherDto);
                            if (opTeach.Succedeed)
                            {
                                return teacherDto;
                            }
                            break;

                        case "Администратор":
                            AdminDTO adminDto = new AdminDTO();
                            OperationDetails opAdm = AdminService.GetPersonalInfo(username, out adminDto);
                            if (opAdm.Succedeed)
                            {
                                return adminDto;
                            }
                            break;

                        default: break;
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                switch (model.Role)
                {
                    case "Студент":
                        return await StudentRegistration(model);

                    case "Преподаватель":
                        return await TeacherRegistration(model);
                }

            }
            return BadRequest(ModelState);
        }

        private async Task<IHttpActionResult> StudentRegistration(RegisterModel model)
        {
            StudentDTO studentDto = new StudentDTO
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                StudyStart = model.StudyStart,
                Role = model.Role
            };

            OperationDetails operationDetails = await StudentService.Create(studentDto);

            if (operationDetails.Succedeed)
            {
                return Ok();
            }
            else
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);

            return BadRequest(ModelState);
        }

        private async Task<IHttpActionResult> TeacherRegistration(RegisterModel model)
        {
            TeacherDTO teacherDto = new TeacherDTO
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Role = model.Role
            };

            OperationDetails operationDetails = await TeacherService.Create(teacherDto);

            if (operationDetails.Succedeed)
            {
                return Ok();
            }
            else
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IHttpActionResult> RemoveUser([FromBody] string username)
        {
            if (username != null)
            {
                var role = await AdminService.GetRole(username);
                if (role != null)
                {
                    switch (role)
                    {
                        case "Студент":
                            OperationDetails op_stu = await StudentService.RemoveUser(username);
                            if (op_stu.Succedeed)
                                return Ok();
                            break;

                        case "Преподаватель":
                            OperationDetails op_teach = await TeacherService.RemoveUser(username);
                            if (op_teach.Succedeed)
                                return Ok();
                            break;
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet]        
        public async Task<ICollection<StudentDTO>> GetAllStudents()
        {
            var students = await StudentService.GetAll();
            return students;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddSubject([FromBody]SubjectAddModel model)
        {
            if (ModelState.IsValid)
            {
                SubjectDTO subjectDto = new SubjectDTO
                {
                    Title = model.Title,
                    Description = model.Description
                };
                OperationDetails op = await SubjectService.CreateSubject(subjectDto);
                if (op.Succedeed)
                    return Ok();
                else
                {
                    ModelState.AddModelError(op.Property, op.Message);
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public ICollection<SubjectDTO> GetAllSubjects()
        {
            var subjects = SubjectService.GetAllSubjects();
            return subjects;
        }

        [HttpPost]
        public SubjectDTO GetSubject([FromBody]string title)
        {
            if (title != null)
            {
                var subjectDto = SubjectService.GetSubject(title);
                if (subjectDto != null)
                    return subjectDto;
            }
            return null;
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateSubject([FromBody]SubjectAddModel model)
        {
            if (ModelState.IsValid)
            {
                SubjectDTO subjectDto = new SubjectDTO
                {
                    Title = model.Title,
                    Description = model.Description
                };
                OperationDetails op = await SubjectService.UpdateSubject(subjectDto);
                if (op.Succedeed)
                    return Ok();
                else
                {
                    ModelState.AddModelError(op.Property, op.Message);
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IHttpActionResult> RemoveSubject([FromBody] string title)
        {
            if (title != null)
            {
                OperationDetails op = await SubjectService.RemoveSubject(title);
                if (op.Succedeed)
                    return Ok();
                else
                {
                    ModelState.AddModelError(op.Property, op.Message);
                    return BadRequest(ModelState);
                }
            }
            ModelState.AddModelError("", "Отсутствуют данные удаляемого предмета");
            return BadRequest(ModelState);
        }

        [HttpPost]        
        public async Task<IHttpActionResult> AddSubjectToTeacher([FromBody]SubjectAssignToTeacherModel model)
        {
            if (ModelState.IsValid)
            {
                OperationDetails op_teach = await TeacherService.AddSubjectToTeacher(model.Title, model.TeacherName);
                if (op_teach.Succedeed)
                    return Ok();
                else
                {
                    ModelState.AddModelError(op_teach.Property, op_teach.Message);
                    return BadRequest(ModelState);
                }

            }            
            return (BadRequest(ModelState));
        }

        [HttpPost]
        public async Task<IHttpActionResult> CancelAssignSubject([FromBody]SubjectAssignToTeacherModel model)
        {
            if (ModelState.IsValid)
            {
                OperationDetails op_teach = await TeacherService.CancelSubjectFromTeacher(model.Title, model.TeacherName);
                if (op_teach.Succedeed)
                    return Ok();
                else
                {
                    ModelState.AddModelError(op_teach.Property, op_teach.Message);
                    return BadRequest(ModelState);
                }

            }
            return (BadRequest(ModelState));
        }

        [HttpPost]
        public ICollection<TeacherDTO> TeachersOfSubject([FromBody] string subjectTitle)
        {
            if (subjectTitle != null)
            {
                return TeacherService.TeachersOfSubject(subjectTitle);
            }
            return null;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AssignTeacherToStudent ([FromBody] AssignTeacherModel model)
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
        public async Task<IHttpActionResult> CancelTeacherToStudent ([FromBody] AssignTeacherModel model)
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
