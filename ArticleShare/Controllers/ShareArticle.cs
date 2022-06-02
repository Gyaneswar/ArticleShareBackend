using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel.DataAnnotations;

namespace ArticleShare.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareArticle : ControllerBase
    {
        ArticleDAL dataAccess;
        public ShareArticle()
        {
            dataAccess = new ArticleDAL();
        }
        [HttpPost]
        public Boolean createArticle(Article art)
        {
            string email = art.email, articlename = art.articlename;
            int articleid = dataAccess.createArticle(email,articlename);
            if (articleid == -1)
                return false;

            art.articleid = articleid;
            return dataAccess.postContent(art);

        }

        [HttpGet]
        public List<Article> fetchArticles(Boolean dateasc,Boolean datedesc, string? searchby = null)
        {            
            return dataAccess.fetchArticles(dateasc, datedesc, searchby);
        }
    }
}
