using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;

namespace TestAppStudy.BLL.Interfaces
{
    public interface INoteService: IDisposable
    {
        ICollection<NoteDTO> GetNotesBySubject(string subjectTitle, string username);
        Task<OperationDetails> AddNote(NoteDTO noteDto);
    }
}
