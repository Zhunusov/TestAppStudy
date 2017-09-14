using TestAppStudy.BLL.Interfaces;
using TestAppStudy.BLL.DTO;
using TestAppStudy.BLL.Infrastructure;
using TestAppStudy.DAL.Interfaces;
using TestAppStudy.DAL.Entities;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace TestAppStudy.BLL.Services
{
    public class AdminService : IAdminService
    {
        IUnitOfWork Database { get; set; }

        public AdminService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(AdminDTO adminDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(adminDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = adminDto.Email, UserName = adminDto.Email };
                var result = await Database.UserManager.CreateAsync(user, adminDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                
                await Database.UserManager.AddToRoleAsync(user.Id, adminDto.Role);
                
                Admin admin = new Admin
                {
                    Id = user.Id,
                    FirstName = adminDto.FirstName,
                    LastName = adminDto.LastName,
                    Patronymic = adminDto.Patronymic
                };

                Database.AdminManager.Create(admin);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }
        public async Task<OperationDetails> Update(AdminDTO adminDto)
        {
            Admin user = Database.AdminManager.Get(adminDto.UserName);
            if (user != null)
            {               
                Database.AdminManager.Update(user);
                user.FirstName = adminDto.FirstName;
                user.LastName = adminDto.LastName;
                user.Patronymic = adminDto.Patronymic;

                await Database.SaveAsync();
                return new OperationDetails(true, "Данные пользователя обновлены", "");
            }
            else
            {
                return new OperationDetails(false, "Пользовательское имя не найдено", "Username");
            }
        }

        public OperationDetails GetPersonalInfo(string username, out AdminDTO adminDto)
        {
            adminDto = new AdminDTO();
            Admin user = Database.AdminManager.Get(username);  
            if (user != null)
            {
                string roleID = user.ApplicationUser.Roles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).FirstOrDefault();
                ApplicationRole role = Database.RoleManager.FindById(roleID);
                if (role != null)
                {
                    adminDto.Id = user.Id;
                    adminDto.Email = user.ApplicationUser.Email;
                    adminDto.FirstName = user.FirstName;
                    adminDto.LastName = user.LastName;
                    adminDto.Patronymic = user.Patronymic;
                    adminDto.UserName = user.ApplicationUser.UserName;
                    adminDto.Role = role.Name;

                   ;

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

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;            
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
           
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }
       
        public async Task SetInitialData(AdminDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public async Task<ICollection<AdminDTO>> GetAll()
        {
           List<AdminDTO> adminsDto = new List<AdminDTO>();
            var admins = Database.AdminManager.GetAll();
            foreach (var admin in admins)
            {
                AdminDTO admDto = new AdminDTO
                {
                    Id = admin.Id,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Patronymic = admin.Patronymic,
                    Email = admin.ApplicationUser.Email,
                    UserName = admin.ApplicationUser.UserName,
                    Role = await GetRole(admin.ApplicationUser.UserName)
                };
                adminsDto.Add(admDto);
            }
            return adminsDto;
        }

        public async Task<OperationDetails> RemoveUser(string username)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(username);
            if (user != null)
            {
                if (Database.AdminManager.Delete(username))
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
            return new OperationDetails(false, "Пользователь не найден","");
        }        

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
