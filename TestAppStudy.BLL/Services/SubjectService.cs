using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestAppStudy.BLL.Services
{
    public class SubjectService : ISubjectService
    {
        IUnitOfWork Database { get; set; }

        public SubjectService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<OperationDetails> CreateSubject(SubjectDTO item)
        {
            if (item != null)
            {
                if (Database.SubjectRepository.Get(item.Title) == null)
                {
                    Subject subject = new Subject
                    {                        
                        Title = item.Title,
                        Description = item.Description
                    };

                    Database.SubjectRepository.Create(subject);
                    await Database.SaveAsync();
                    return new OperationDetails(true, "Предмет успешно создан", "");
                }
                return new OperationDetails(false, "Предмет с таким названием уже существует","");

            }
            return new OperationDetails(false, "Отсутствует создаваемый предмет", "");
        }

        public ICollection<SubjectDTO> GetAllSubjects()
        {
            List<SubjectDTO> subjectsDto = new List<SubjectDTO>();
            var subjects = Database.SubjectRepository.GetAll();
            foreach (var s in subjects)
            {
                SubjectDTO sDto = new SubjectDTO
                {
                    Title = s.Title,
                    Description = s.Description
                };
                subjectsDto.Add(sDto);
            }
            return subjectsDto;
        }

        public SubjectDTO GetSubject(string title)
        {
            if (title != null)
            {
                var subject = Database.SubjectRepository.Get(title);
                if(subject != null)
                {
                    SubjectDTO subjectDto = new SubjectDTO
                    {
                        Title = subject.Title,
                        Description = subject.Description
                    };
                    return subjectDto;
                }                     
            }
            return null;
        }

        public async Task<OperationDetails> UpdateSubject(SubjectDTO subjectDto)
        {
            if (subjectDto != null)
            {
                var subject = Database.SubjectRepository.Get(subjectDto.Title);
                if (subject != null)
                {
                    Database.SubjectRepository.Update(subject);
                    subject.Title = subjectDto.Title;
                    subject.Description = subjectDto.Description;

                    await Database.SaveAsync();
                    return new OperationDetails(true, "Предмет успешно обновлен", "");
                }
                return new OperationDetails(false, "Предмет не найден", "");
            }
            return new OperationDetails(false, "Отсутствуют данные обновляемого предмета","");
        }

        public async Task<OperationDetails> RemoveSubject(string title)
        {
            if (title != null)
            {
                var notes = Database.NoteRepository.Find(n => n.SubjectTitle == title);
                foreach (var n in notes)
                {
                    Database.NoteRepository.Delete(n.Id);
                }

                if (Database.SubjectRepository.Delete(title))
                {
                    await Database.SaveAsync();
                    return new OperationDetails(true, "Предмет успешно удален", "");
                }
                return new OperationDetails(false, "Ошибка при удалении предмета из базы", "");
            }
            return new OperationDetails(false, "Отсутствуют данные по удаляемому предмету", "");
        }

        public ICollection<SubjectDTO> GetSubjectsByTeacher(string teacherUserName)
        {
            if(teacherUserName != null)
            {
                List<SubjectDTO> subjectsDto = new List<SubjectDTO>();
                var teacher = Database.TeacherManager.Get(teacherUserName);

                foreach(var s in teacher.Subjects)
                {
                    subjectsDto.Add(new SubjectDTO
                    {
                        Title = s.Title,
                        Description = s.Description
                    });
                }
                return subjectsDto;
            }
            return null;
        }

        public ICollection<SubjectDTO> GetSubjectsByStudent(string studentUserName)
        {
            List<SubjectDTO> subjectsDto = new List<SubjectDTO>();
            if (studentUserName != null)
            {
                var student = Database.StudentManager.Get(studentUserName);

                foreach (var s in student.Subjects)
                {
                    subjectsDto.Add(new SubjectDTO
                    {
                        Title = s.Title,
                        Description = s.Description
                    });
                }
                return subjectsDto;
            }
            return null;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
