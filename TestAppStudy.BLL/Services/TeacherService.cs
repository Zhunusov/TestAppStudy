using TestAppStudy.BLL.Interfaces;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace TestAppStudy.BLL.Services
{
    public class TeacherService : ITeacherService
    {
        IUnitOfWork Database { get; set; }

        public TeacherService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(TeacherDTO teacherDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(teacherDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = teacherDto.Email, UserName = teacherDto.Email };
                var result = await Database.UserManager.CreateAsync(user, teacherDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
               
                await Database.UserManager.AddToRoleAsync(user.Id, teacherDto.Role);
                
                Teacher teacher = new Teacher
                {
                    Id = user.Id,
                    FirstName = teacherDto.FirstName,
                    LastName = teacherDto.LastName,
                    Patronymic = teacherDto.Patronymic                    
                };

                Database.TeacherManager.Create(teacher);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<OperationDetails> Update(TeacherDTO teacherDto)
        {
            Teacher user = Database.TeacherManager.Get(teacherDto.UserName);
            if (user != null)
            {
                Database.TeacherManager.Update(user);
                user.FirstName = teacherDto.FirstName;
                user.LastName = teacherDto.LastName;
                user.Patronymic = teacherDto.Patronymic;                           

                await Database.SaveAsync();
                return new OperationDetails(true, "Данные пользователя обновлены", "");
            }
            else
            {
                return new OperationDetails(false, "Пользовательское имя не найдено", "Username");
            }
        }

        public OperationDetails GetPersonalInfo(string username, out TeacherDTO teacherDto)
        {
            teacherDto = new TeacherDTO();
            Teacher user = Database.TeacherManager.Get(username);
            if (user != null)
            {
                string roleID = user.ApplicationUser.Roles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).FirstOrDefault();
                ApplicationRole role = Database.RoleManager.FindById(roleID);
                if (role != null)
                {
                    teacherDto.Id = user.Id;
                    teacherDto.Email = user.ApplicationUser.Email;
                    teacherDto.FirstName = user.FirstName;
                    teacherDto.LastName = user.LastName;
                    teacherDto.Patronymic = user.Patronymic;
                    teacherDto.UserName = user.ApplicationUser.UserName;
                    teacherDto.Role = role.Name;

                    foreach (var s in user.Subjects)
                    {
                        SubjectDTO sDto = new SubjectDTO
                        {
                            Title = s.Title,
                            Description = s.Description
                        };
                        teacherDto.SubjectsDTO.Add(sDto);
                    }

                    return new OperationDetails(true, "Поиск пользователя произведен успешно", "");
                }
                else
                {
                    return new OperationDetails(false, "Пользовательская роль не найдена", "Role");
                }
            }
            else
            {
                return new OperationDetails(false, "Пользовательское имя не найдено", "Username");
            }
        }

        public async Task<string> GetRole(string username)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(username);
            if (user != null)
            {
                string roleID = user.Roles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).FirstOrDefault();
                ApplicationRole role = await Database.RoleManager.FindByIdAsync(roleID);
                if (role != null)
                    return role.Name;

            }
            return null;
        }
        
        public async Task<ICollection<TeacherDTO>> GetAll()
        {
            List<TeacherDTO> teachersDto = new List<TeacherDTO>();
            var teachers = Database.TeacherManager.GetAll();
            foreach (var teacher in teachers)
            {
                TeacherDTO teachDto = new TeacherDTO
                {
                    Id = teacher.Id,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Patronymic = teacher.Patronymic,
                    Email = teacher.ApplicationUser.Email,
                    UserName = teacher.ApplicationUser.UserName,
                    Role = await GetRole(teacher.ApplicationUser.UserName)
                };
                teachersDto.Add(teachDto);
            }
            return teachersDto;
        }

        public async Task<OperationDetails> RemoveUser(string username)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(username);
            if (user != null)
            {                
                if (Database.TeacherManager.Delete(username))
                {
                    var notes = Database.NoteRepository.Find(n => n.ApplicationUserId == user.Id);
                    foreach(var n in notes)
                    {
                        Database.NoteRepository.Delete(n.Id);
                    }

                    var result = Database.UserManager.Delete(user);

                    if (result.Errors.Count() > 0)
                        return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                    await Database.SaveAsync();
                    return new OperationDetails(true, "", "");
                }
            }
            return new OperationDetails(false, "Пользователь не найден", "");
        }

        public async Task<OperationDetails> AddSubjectToTeacher(string subjectTitle, string teacherName)
        {
            if (subjectTitle != null && teacherName != null)
            {
                var teacher = Database.TeacherManager.Get(teacherName);
                if (teacher != null)
                {                   
                    if(teacher.Subjects.FirstOrDefault(s => s.Title == subjectTitle) == null)
                    {
                        var subject = Database.SubjectRepository.Get(subjectTitle);
                        if (subject != null)
                        {
                            Database.TeacherManager.Update(teacher);
                            teacher.Subjects.Add(subject);
                            foreach(var stu in subject.Students)
                            {
                                stu.Teachers.Add(teacher);
                            }
                            await Database.SaveAsync();
                            return new OperationDetails(true, "Предмет успешно назначен преподавателю", "");
                        }
                        return new OperationDetails(false, "Предмет не найден", "");
                    }
                    return new OperationDetails(false, "Предмет уже назначен", "");
                }
            }
            return new OperationDetails(false, "Отсутствуют данные для назначения предмета", "");
        }

        public async Task<OperationDetails> CancelSubjectFromTeacher(string subjectTitle, string teacherName)
        {
            if (subjectTitle != null && teacherName != null)
            {
                var teacher = Database.TeacherManager.Get(teacherName);
                if (teacher != null)
                {
                    if (teacher.Subjects.FirstOrDefault(s => s.Title == subjectTitle) != null)
                    {
                        var subject = Database.SubjectRepository.Get(subjectTitle);
                        if (subject != null)
                        {                           
                            teacher.Subjects.Remove(subject);
                            foreach(var stu in subject.Students)
                            {
                                stu.Teachers.Remove(teacher);
                            }
                            
                            await Database.SaveAsync();
                            return new OperationDetails(true, "Предмет успешно отменен преподавателю", "");
                        }
                        return new OperationDetails(false, "Предмет не найден", "");
                    }
                    return new OperationDetails(false, "Предмет не назначен преподавателю", "");
                }
            }
            return new OperationDetails(false, "Отсутствуют данные для отмены предмета", "");
        }

        public ICollection<TeacherDTO> TeachersOfSubject(string subjectTitle)
        {
            List<TeacherDTO> teachersDto = new List<TeacherDTO>();
            if (subjectTitle != null)
            {
                List<Teacher> teachers = Database.TeacherManager
                    .Find(t => t.Subjects.FirstOrDefault(s => s.Title == subjectTitle)!=null)                    
                    .ToList();

                foreach (var t in teachers)
                {
                    teachersDto.Add(new TeacherDTO
                    {
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Patronymic = t.Patronymic,
                        UserName = t.ApplicationUser.UserName
                    });
                }
                return teachersDto;
            }
            return null;
        }

        public ICollection<StudentDTO> StudentsOfSubject(string title)
        {
            List<StudentDTO> studentsDto = new List<StudentDTO>();
            if (title != null)
            {                
                var subject = Database.SubjectRepository.Get(title);
                foreach (var s in subject.Students)
                {
                    studentsDto.Add(new StudentDTO
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Patronymic = s.Patronymic,
                        UserName = s.ApplicationUser.UserName
                    });
                }
                return studentsDto;
            }
            return null;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
