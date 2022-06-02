using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ArticleShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Register : ControllerBase
    {        
        DataAccess dataAccess;
        public Register()
        {
            dataAccess = new DataAccess();
        }
        [HttpPost]
        public Boolean register(User obj)
        {
            obj.userLevel = 1;
            if(!string.IsNullOrEmpty(obj.adminpassword) && obj.adminpassword.Equals("secret"))
            {
                obj.userLevel = 2;
            }
            if (!string.IsNullOrEmpty(obj.superadminpassword) && obj.superadminpassword.Equals("evenmoresecret"))
            {
                obj.userLevel = 3;
            }
            return dataAccess.RegisterUser(obj);            
        }
    }
}
