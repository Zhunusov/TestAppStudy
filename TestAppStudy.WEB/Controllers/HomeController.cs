using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.WEB.Filters;

namespace TestAppStudy.WEB.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private IAdminService AdminService
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<IAdminService>();
            }
        }
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                string role = await AdminService.GetRole(User.Identity.Name);
                if (role != null)
                {
                    switch (role)
                    {
                        case "Студент":
                            return RedirectToAction("StudentArea");                            

                        case "Преподаватель":
                            return RedirectToAction("TeacherArea");                            

                        case "Администратор":
                            return RedirectToAction("AdminArea");                            

                        default: break;
                    }
                }
            }            
            return View();
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult AdminArea()
        {
            return View();
        }

        [Authorize(Roles = "Студент")]
        public ActionResult StudentArea()
        {
            return View();
        }

        [Authorize(Roles = "Преподаватель")]
        public ActionResult TeacherArea()
        {
            return View();
        }

    }
}
