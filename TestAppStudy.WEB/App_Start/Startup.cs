using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using TestAppStudy.BLL.Services;
using Microsoft.AspNet.Identity;
using TestAppStudy.BLL.Interfaces;
using TestAppStudy.BLL.DTO;

[assembly: OwinStartup(typeof(TestAppStudy.WEB.App_Start.Startup))]

namespace TestAppStudy.WEB.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(CreateStudentService);
            app.CreatePerOwinContext(CreateTeacherService);
            app.CreatePerOwinContext(CreateAdminService);
            app.CreatePerOwinContext(CreateSubjectService);
            app.CreatePerOwinContext(CreateNoteService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Index"),
            });
        }

        private IStudentService CreateStudentService()
        {
            return serviceCreator.CreateStudentService("Students");
        }
        private ITeacherService CreateTeacherService()
        {
            return serviceCreator.CreateTeacherService("Students");
        }

        private IAdminService CreateAdminService()
        {
            return serviceCreator.CreateAdminService("Students");
        }

        private ISubjectService CreateSubjectService()
        {
            return serviceCreator.CreateSubjectService("Students");
        }

        private INoteService CreateNoteService()
        {
            return serviceCreator.CreateNoteService("Students");
        }
    }
}