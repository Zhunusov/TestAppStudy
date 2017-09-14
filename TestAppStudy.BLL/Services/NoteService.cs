using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;

namespace TestAppStudy.BLL.Services
{
    public class NoteService : INoteService
    {
        IUnitOfWork Database { get; set; }

        public NoteService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public ICollection<NoteDTO> GetNotesBySubject(string subjectTitle, string username = null)
        {
            if(subjectTitle != null)
            {
                List<NoteDTO> notesDto = new List<NoteDTO>();
                var notes = Database.NoteRepository.Find(n => n.SubjectTitle == subjectTitle);
                foreach(var n in notes)
                {
                    if (n.IsPublic)
                    {
                        notesDto.Add(new NoteDTO
                        {
                            Id = n.Id,
                            Description = n.Description,
                            IsPublic = n.IsPublic,
                            SubjectTitle = n.SubjectTitle,
                            UserName = n.ApplicationUser.UserName
                        });
                    }
                    if(username != null && !n.IsPublic && n.ApplicationUser.UserName == username)
                    {
                        notesDto.Add(new NoteDTO
                        {
                            Id = n.Id,
                            Description = n.Description,
                            IsPublic = n.IsPublic,
                            SubjectTitle = n.SubjectTitle,
                            UserName = n.ApplicationUser.UserName
                        });
                    }                    
                }
                return notesDto;
            }
            return null;
        }

        public async Task<OperationDetails> AddNote(NoteDTO noteDto)
        {
            if (noteDto != null)
            {
                var user = await Database.UserManager.FindByNameAsync(noteDto.UserName);
                Note note = new Note
                {
                    Description = noteDto.Description,
                    SubjectTitle = noteDto.SubjectTitle,
                    IsPublic = noteDto.IsPublic,
                    ApplicationUserId = user.Id
                };
                Database.NoteRepository.Create(note);
                await Database.SaveAsync();
                return new OperationDetails(true, "Комментарий успешно добавлен", "");
            }
            return new OperationDetails(false, "Отсутствуют данные для добавления комментария", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
