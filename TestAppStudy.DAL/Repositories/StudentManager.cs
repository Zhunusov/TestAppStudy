using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using TestAppStudy.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;


namespace TestAppStudy.DAL.Repositories
{
    public class StudentManager : IUserManager<Student>
    {
        private ApplicationContext db;
        public StudentManager(ApplicationContext context)
        {
            db = context;
        }

        public void Create(Student item)
        {
            db.Students.Add(item);
        }
        public IEnumerable<Student> GetAll()
        {
            return db.Students
                .Include(s => s.ApplicationUser)
                .Include(s => s.Subjects)
                .Include(s => s.Teachers)
                .ToList();
                
        }
        public Student Get(string username)
        {
            return db.Students
                .Include(s => s.ApplicationUser)
                .Include(s => s.Subjects)
                .Include(s => s.Teachers)
                .FirstOrDefault(s => s.ApplicationUser.UserName == username);
        }
        public IEnumerable<Student> Find(Func<Student, Boolean> predicate)
        {
            return db.Students
                .Include(s => s.ApplicationUser)
                .Where(predicate)
                .ToList();
        }       
        public void Update(Student item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public bool Delete(string username)
        {
            Student student = db.Students                
                .FirstOrDefault(s => s.ApplicationUser.UserName == username);

            if (student != null)
            {
                db.Students.Remove(student);
                return true;
            }
            return false;
        }
    }
}
