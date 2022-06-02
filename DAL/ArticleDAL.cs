using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ArticleDAL
    {
        public int createArticle(string email, string articlename)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "ArticleShare";
                int articleId = 0;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd =
                        new SqlCommand("INSERT INTO articles(email,articlename) OUTPUT INSERTED.articleid VALUES(@email,@articlename)", connection);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@articlename", articlename);
                    try
                    {
                        articleId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return -1;
                    }

                    SqlCommand cmd1 =
                        new SqlCommand("INSERT INTO articlelogs(articleid,articlestatus,statuschangedate) " +
                        "VALUES(@articleid,@status,@date)", connection);

                    cmd1.Parameters.AddWithValue("@articleid", articleId);
                    cmd1.Parameters.AddWithValue("@status", 1);
                    cmd1.Parameters.AddWithValue("@date", DateTime.Now);

                    try
                    {
                        cmd1.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return -1;
                    }
                }
                return articleId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

        public Boolean postContent(Article art)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "ArticleShare";
                int articleId = 0;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd =
                        new SqlCommand("INSERT INTO articledetails(articleid,author,articlename,articledate,articlecontent) " +
                        " VALUES(@articleid,@author,@articlename,@articledate,@articlecontent)", connection);
                    cmd.Parameters.AddWithValue("@articleid", art.articleid);
                    cmd.Parameters.AddWithValue("@author", art.author);
                    cmd.Parameters.AddWithValue("@articlename", art.articlename);
                    cmd.Parameters.AddWithValue("@articledate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@articlecontent", art.articlecontent);
                    try
                    {
                        cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return false;
                    }

                    SqlCommand cmd1 =
                        new SqlCommand("INSERT INTO articlelogs(articleid,articlestatus,statuschangedate) " +
                        "VALUES(@articleid,@status,@date)", connection);

                    cmd1.Parameters.AddWithValue("@articleid", art.articleid);
                    cmd1.Parameters.AddWithValue("@status", 2);
                    cmd1.Parameters.AddWithValue("@date", DateTime.Now);

                    try
                    {
                        cmd1.ExecuteScalar();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        public List<Article> fetchArticles(Boolean dateAsc = false, Boolean dateDesc =false, string? name = "")
        {
            List<Article> articles = new List<Article>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "ArticleShare";
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string query = "select * from articledetails ad inner join " +
                        "(select top 1 with ties id ,articleid ,statuschangedate,articlestatus " +
                        "from articlelogs " +
                        "order by row_number() over(partition by articleid order by statuschangedate desc)) al " +
                        "on ad.articleid = al.articleid where al.articlestatus > 3";
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query + " and ad.articlecontent like '%" + name + "%'";
                    }
                    if (dateAsc && !dateDesc)
                    {
                        query = query + " order by statuschangedate asc";
                    }
                    else if (!dateAsc && dateDesc)
                    {
                        query = query + " order by statuschangedate desc";
                    }                   

                    connection.Open();
                    SqlCommand cmd =
                        new SqlCommand(query, connection);
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Article article = new Article();
                            article.articleid = reader.GetInt32("articleid");
                            article.author = reader.GetString("author");
                            article.articlename = reader.GetString("articlename");
                            article.articledate = reader.GetDateTime("articledate");
                            article.articlecontent = reader.GetString("articlecontent");
                            article.articlestatus = reader.GetInt32("articlestatus");
                            articles.Add(article);
                        }
                        return articles;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return articles;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return articles;
            }
        }

        
    }
}
