using TestAppStudy.BLL.Interfaces;
using TestAppStudy.BLL.DTO;
using System;
using TestAppStudy.DAL.Interfaces;
using TestAppStudy.BLL.Infrastructure;
using System.Threading.Tasks;
using TestAppStudy.DAL.Entities;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace TestAppStudy.BLL.Services
{
    public class StudentService : IStudentService
    {
        IUnitOfWork Database { get; set; }

        public StudentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(StudentDTO studentDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(studentDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email =studentDto.Email, UserName = studentDto.Email };
                var result = await Database.UserManager.CreateAsync(user, studentDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                
                await Database.UserManager.AddToRoleAsync(user.Id, studentDto.Role);
               
                Student student = new Student
                {
                    Id = user.Id,                    
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    Patronymic = studentDto.Patronymic,
                    StudyStart = studentDto.StudyStart                    
                };
               
                Database.StudentManager.Create(student);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<OperationDetails> Update(StudentDTO studentDto)
        {
            Student user = Database.StudentManager.Get(studentDto.UserName);
            if (user != null)
            {
                Database.StudentManager.Update(user);
                user.FirstName = studentDto.FirstName;
                user.LastName = studentDto.LastName;
                user.Patronymic = studentDto.Patronymic;
                user.StudyStart = studentDto.StudyStart;                

                await Database.SaveAsync();
                return new OperationDetails(true, "Данные пользователя обновлены", "");
            }
            else
            {
                return new OperationDetails(false, "Пользовательское имя не найдено", "Username");
            }
        }

        public OperationDetails GetPersonalInfo(string username, out StudentDTO studentDto)
        {
            studentDto = new StudentDTO();
            Student user = Database.StudentManager.Get(username);
            if (user != null)
            {
                string roleID = user.ApplicationUser.Roles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).FirstOrDefault();
                ApplicationRole role = Database.RoleManager.FindById(roleID);
                if (role != null)
                {
                    studentDto.Id = user.Id;
                    studentDto.Email = user.ApplicationUser.Email;
                    studentDto.FirstName = user.FirstName;
                    studentDto.LastName = user.LastName;
                    studentDto.Patronymic = user.Patronymic;
                    studentDto.UserName = user.ApplicationUser.UserName;                    
                    studentDto.StudyStart_Short = ((DateTime)user.StudyStart).ToShortDateString();
                    studentDto.Role = role.Name;

                    foreach (var s in user.Subjects)
                    {
                        List<TeacherDTO> listTeachDto = new List<TeacherDTO>();
                        var subject = Database.SubjectRepository.Get(s.Title);
                        foreach (var t in subject.Teachers)
                        {
                            if (user.Teachers.Contains(t))
                            {
                                listTeachDto.Add(new TeacherDTO
                                {
                                    FirstName = t.FirstName,
                                    LastName = t.LastName,
                                    Patronymic = t.Patronymic,
                                    UserName = t.ApplicationUser.UserName
                                });

                            }
                        }
                        studentDto.SubjectsDTO.Add(s.Title, listTeachDto);
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
        
        public async Task<ICollection<StudentDTO>> GetAll()
        {
            List<StudentDTO> studentsDto = new List<StudentDTO>();
            var students = Database.StudentManager.GetAll();
            foreach (var student in students)
            {
                StudentDTO stuDto = new StudentDTO
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Patronymic = student.Patronymic,
                    Email = student.ApplicationUser.Email,
                    UserName = student.ApplicationUser.UserName,
                    StudyStart = student.StudyStart,
                    StudyStart_Short = ((DateTime)student.StudyStart).ToShortDateString(),
                    Role = await GetRole(student.ApplicationUser.UserName)
                };
                studentsDto.Add(stuDto);
            }
            return studentsDto;
        }

        public async Task<OperationDetails> RemoveUser(string username)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(username);
            if (user != null)
            {
                if (Database.StudentManager.Delete(username))
                {
                    var notes = Database.NoteRepository.Find(n => n.ApplicationUserId == user.Id);
                    foreach (var n in notes)
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

        public async Task<OperationDetails> AddTeacherToStudent(string subjectTitle, string teacherUserName, string studentUserName)
        {
            if (subjectTitle != null && teacherUserName != null && studentUserName != null) 
            {
                var student = Database.StudentManager.Get(studentUserName);
                if (student.Subjects.FirstOrDefault(subj => subj.Title == subjectTitle) == null ||
                        student.Teachers.FirstOrDefault(t => t.ApplicationUser.UserName == teacherUserName) == null)
                {
                    
                    if (student != null)
                    {
                        Database.StudentManager.Update(student);
                        if(student.Subjects.FirstOrDefault(subj => subj.Title == subjectTitle)== null)
                        {
                            var subject = Database.SubjectRepository.Get(subjectTitle);
                            student.Subjects.Add(subject);
                        }
                        if (student.Teachers.FirstOrDefault(t => t.ApplicationUser.UserName == teacherUserName) == null)
                        {
                            var teacher = Database.TeacherManager.Get(teacherUserName);
                            student.Teachers.Add(teacher);
                        }

                        await Database.SaveAsync();
                        return new OperationDetails(true, "Преподаваетель успешно назначен", "");
                    }
                    return new OperationDetails(false, "Студент не найден", "");
                }
                return new OperationDetails(false, "Данный преподаватель уже назначен", "");
                
            }
            return new OperationDetails(false, "Отсутствуют все данные для назначения преподавателя", "");
        }

        public async Task<OperationDetails> CancelTeacherToStudent(string subjectTitle, string teacherName, string studentUserName)
        {
            if (subjectTitle != null && teacherName != null && studentUserName != null)
            {
                var student = Database.StudentManager.Get(studentUserName);
                if (student != null)
                {
                    if (student.Subjects.FirstOrDefault(subj => subj.Title == subjectTitle) != null)
                    {
                        var subject = Database.SubjectRepository.Get(subjectTitle);
                        student.Subjects.Remove(subject);
                        await Database.SaveAsync();
                    }
                                        
                    var teachers = Database.TeacherManager.Find(t => t.Subjects.FirstOrDefault(subj => subj.Title == subjectTitle)!= null);                    
                    foreach (var t in teachers)
                    {
                        bool delTeacher = true;
                        foreach( var subj in t.Subjects)
                        {
                            if (student.Subjects.Contains(subj))
                                delTeacher = false;
                        }
                        if (delTeacher)
                        {
                            var teacher = Database.TeacherManager.Get(t.ApplicationUser.UserName);
                            student.Teachers.Remove(teacher);
                            await Database.SaveAsync();
                        }
                    }
                    return new OperationDetails(true, "Преподаватель отменен", "");
                }
                return new OperationDetails(false, "Студент не найден", "");
            }
            return new OperationDetails(false, "Отсутствуют все данные для отмены преподавателя", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
