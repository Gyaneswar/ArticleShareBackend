using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ArticleShare.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Admin : ControllerBase
    {
        AdminDAL dataAccess;
        public Admin()
        {
            dataAccess = new AdminDAL();
        }
        [HttpGet]
        public Boolean review(int articleid, string comments, Boolean approve)
        {
            return dataAccess.review(articleid, comments, approve);
        }

        [HttpGet]
        public List<Article> fetchArticlesToBeReviewed(string email,string password)
        {
            DataAccess d = new DataAccess();
            int userlevel = d.login(email, password);

            if (userlevel <= 2)
                return new List<Article>();

            return dataAccess.fetchArticles();
        }

        [HttpGet]
        public List<Logs> fetchArticleLogs(int articleId)
        {
            return dataAccess.fetchLogs(articleId);
        }
    }
}
