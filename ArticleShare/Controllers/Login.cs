using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticleShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        DataAccess dataAccess;
        public Login()
        {
            dataAccess = new DataAccess();
        }
        [HttpGet]
        public int login(string email,string password)
        {
            return dataAccess.login(email, password);
        }
    }
}
