using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using TestAppStudy.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace TestAppStudy.DAL.Repositories
{
    public class TeacherManager : IUserManager<Teacher>
    {
        private ApplicationContext db;
        public TeacherManager(ApplicationContext context)
        {
            db = context;
        }

        public void Create(Teacher item)
        {
            db.Teachers.Add(item);            
        }

        public IEnumerable<Teacher> GetAll()
        {
            return db.Teachers
                .Include(t => t.ApplicationUser)
                .Include(t => t.Students)
                .Include(t => t.Subjects)
                .ToList();
        }

        public Teacher Get(string username)
        {
            return db.Teachers
                .Include(t => t.ApplicationUser)
                .Include(t => t.Subjects)
                .Include(t => t.Students)
                .FirstOrDefault(t => t.ApplicationUser.UserName == username);
        }
        public IEnumerable<Teacher> Find(Func<Teacher, Boolean> predicate)
        {
            return db.Teachers
                .Include(t => t.ApplicationUser)
                .Include(t => t.Subjects)
                .Include(t => t.Students)
                .Where(predicate)
                .ToList();
        }
        public void Update(Teacher item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public bool Delete(string username)
        {
            Teacher teacher = db.Teachers               
                .FirstOrDefault(t => t.ApplicationUser.UserName == username);

            if (teacher != null)
            {                
                db.Teachers.Remove(teacher);
                return true;
            }
            return false;
        }
    }
}
