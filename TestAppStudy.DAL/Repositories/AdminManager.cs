using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using TestAppStudy.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace TestAppStudy.DAL.Repositories
{
    public class AdminManager : IUserManager<Admin>
    {
        private ApplicationContext db;
        public AdminManager(ApplicationContext context)
        {
            db = context;
        }

        public void Create(Admin item)
        {
            db.Admins
                .Add(item);
        }
        public Admin Get(string username)
        {
            return db.Admins                
                .Include(a => a.ApplicationUser)
                .FirstOrDefault(a => a.ApplicationUser.UserName == username);
        }
        public IEnumerable<Admin> GetAll()
        {
            return db.Admins
                .Include(a => a.ApplicationUser)
                .ToList();
        }
       
        public IEnumerable<Admin> Find(Func<Admin, Boolean> predicate)
        {
            return db.Admins
                .Include(a => a.ApplicationUser)
                .Where(predicate)
                .ToList();
        }      
        public void Update(Admin item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public bool Delete(string username)
        {
            Admin admin = db.Admins                
                .FirstOrDefault(a => a.ApplicationUser.UserName == username);

            if (admin != null)
            {
                db.Admins.Remove(admin);
                return true;
            }
            return false;                
        }
    }
}
