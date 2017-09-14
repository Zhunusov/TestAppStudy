using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace TestAppStudy.WEB.Controllers
{
    public class CultureController : ApiController
    {        
        [HttpPost]      
        public IHttpActionResult ChangeCulture([FromBody]string lang)
        {  
            List<string> cultures = new List<string>()
            {
                "ru",
                "en"
            };

            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
                       
            HttpCookie cookie = HttpContext.Current.Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
            return Ok();
        }

    }
}
