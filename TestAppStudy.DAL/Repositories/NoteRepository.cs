using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using TestAppStudy.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace TestAppStudy.DAL.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private ApplicationContext Database;
        public NoteRepository(ApplicationContext db)
        {
            Database = db;
        }
        public IEnumerable<Note> GetAll()
        {
          return Database.Notes
                .Include(n => n.Subject)
                .Include(n => n.ApplicationUser)
                .ToList();
        }
        public Note Get(int id)
        {
            return Database.Notes
                .Find(id);
        }
        public IEnumerable<Note> Find(Func<Note, Boolean> predicate)
        {
            return Database.Notes                
                .Include(n => n.Subject)
                .Include(n => n.ApplicationUser)
                .Where(predicate).ToList();
        }
        public void Create(Note item)
        {
            Database.Notes
                .Add(item);
        }
        public void Update(Note item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }
        public bool Delete(int id)
        {
            Note note = Database.Notes.Find(id);
            if (note != null)
            {
                Database.Notes.Remove(note);
                return true;
            }
            return false;
        }        
    }
}
