using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using TestAppStudy.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace TestAppStudy.DAL.Repositories
{
    public  class SubjectRepository : ISubjectRepository
    {
        private ApplicationContext Database;
        public SubjectRepository(ApplicationContext db)
        {
            Database = db;
        }
        public IEnumerable<Subject> GetAll()
        {
            return Database.Subjects.ToList();
        }
        public Subject Get(string title)
        {
            return Database.Subjects
                .Include(s => s.Teachers)
                .Include(s => s.Students)
                .FirstOrDefault(s => s.Title == title);
        }
        public IEnumerable<Subject> Find(Func<Subject, Boolean> predicate)
        {
            return Database.Subjects
                .Include(s => s.Teachers)
                .Include(s => s.Students)
                .Where(predicate)
                .ToList();
        }
        public void Create(Subject item)
        {
            Database.Subjects.Add(item);
        }
        public void Update(Subject item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }
        public bool Delete(string title)
        {
            Subject subject = Database.Subjects
                .FirstOrDefault(s => s.Title == title);
            if (subject != null)
            {
                Database.Subjects.Remove(subject);
                return true;
            }
            return false;   
        }        
    }
}
