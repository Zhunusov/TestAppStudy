using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using TestAppStudy.BLL.DTO;
     

namespace TestAppStudy.BLL.Interfaces
{
    public interface IAdminService: IUserService<AdminDTO>
    {
        Task<ClaimsIdentity> Authenticate(UserDTO itemDto);
        Task SetInitialData(AdminDTO adminDto, List<string> roles);       
    }
}
