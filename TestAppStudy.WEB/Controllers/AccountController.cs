using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.WEB.Models;

namespace TestAppStudy.WEB.Controllers
{
    [Authorize(Roles = "Администратор, Студент, Преподаватель")]
    public class AccountController : ApiController
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
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        private INoteService NoteService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<INoteService>();
            }
        }

        [HttpGet]        
        public IHttpActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return Ok();
        }               

        [HttpPost] 
        [AllowAnonymous]       
        public async Task<IHttpActionResult> Login([FromBody]LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {                
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await AdminService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("Account.login", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return Ok();
                }                
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register([FromBody]RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                switch (model.Role)
                {
                    case "Студент":
                        return await StudentRegistration(model);                       

                    case "Преподаватель":
                        return await TeacherRegistration(model);

                    default:
                        ModelState.AddModelError("Account.register", "Выбранный статус отсутствует.");
                        return BadRequest(ModelState);                
                }
                
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Save([FromBody] EditPersonalInfoModel model)
        {
            if (ModelState.IsValid)
            {
                switch (model.Role)
                {
                    case "Студент":
                        return await UpdateStudentInfo(model);

                    case "Преподаватель":
                        return await UpdateTeacherInfo(model);                        

                    case "Администратор":
                        return await UpdateAdminInfo(model);

                    default:
                        return BadRequest();
                }
            }
            return BadRequest(ModelState);
        }

        private async Task<IHttpActionResult> UpdateStudentInfo (EditPersonalInfoModel model)
        {
            StudentDTO student = new StudentDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                StudyStart = model.StudyStart,
                UserName = model.Email,
                Role = model.Role
            };
            OperationDetails operationDetails = await StudentService.Update(student);
            if (operationDetails.Succedeed)
            {
                return Ok();
            }
            return BadRequest();
        }

        private async Task<IHttpActionResult> UpdateAdminInfo(EditPersonalInfoModel model)
        {
            AdminDTO admin = new AdminDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,                
                UserName = model.Email,
                Role = model.Role
            };
            OperationDetails operationDetails = await AdminService.Update(admin);
            if (operationDetails.Succedeed)
            {
                return Ok();
            }
            return BadRequest();
        }

        private async Task<IHttpActionResult> UpdateTeacherInfo(EditPersonalInfoModel model)
        {
            TeacherDTO teacher = new TeacherDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                UserName = model.Email,
                Role = model.Role
            };
            OperationDetails operationDetails = await TeacherService.Update(teacher);
            if (operationDetails.Succedeed)
            {
                return Ok();
            }
            return BadRequest();
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
                ClaimsIdentity claim = await AdminService.Authenticate(studentDto);
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
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
                ClaimsIdentity claim = await AdminService.Authenticate(teacherDto);
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                return Ok();
            }
            else
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);

            return BadRequest(ModelState);
        }

        private async Task SetInitialDataAsync()
        {
            await AdminService.SetInitialData(new AdminDTO
            {
                Email = "test@mail.ru",
                UserName = "test@mail.ru",
                Password = "123456789",
                FirstName = "Артур",
                LastName = "Жунусов",
                Patronymic = "Кажиканович",                
                Role = "Администратор",
            }, new List<string> { "Студент", "Преподаватель", "Администратор" });
        }

        [HttpGet]               
        public async Task<UserDTO> GetUserInfo()
        {
            string role = await AdminService.GetRole(User.Identity.Name);
            if (role != null)
            {
                switch (role)
                {
                    case "Студент":
                        StudentDTO studentDto = new StudentDTO();
                        OperationDetails opStu = StudentService.GetPersonalInfo(User.Identity.Name, out studentDto);
                        if (opStu.Succedeed)
                        {
                            return studentDto;
                        }
                        break;

                    case "Преподаватель":
                        TeacherDTO teacherDto = new TeacherDTO();
                        OperationDetails opTeach = TeacherService.GetPersonalInfo(User.Identity.Name, out teacherDto);
                        if (opTeach.Succedeed)
                        {
                            return teacherDto;
                        }
                        break;

                    case "Администратор":
                        AdminDTO adminDto = new AdminDTO();
                        OperationDetails opAdm = AdminService.GetPersonalInfo(User.Identity.Name, out adminDto);
                        if (opAdm.Succedeed)
                        {
                            return adminDto;
                        }
                        break;

                    default: break;
                }
            }  
            return null;
        }

        [HttpGet]
        public async Task<string> GetUserRole()
        {
            return await AdminService.GetRole(User.Identity.Name);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddComment([FromBody] AddCommentModel model)
        {
            if (ModelState.IsValid)
            {
                NoteDTO noteDto = new NoteDTO
                {
                    Description = model.Description,
                    SubjectTitle = model.SubjectTitle,
                    IsPublic = model.IsPublic,
                    UserName = model.UserName
                };
                OperationDetails op = await NoteService.AddNote(noteDto);
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
    }
}
