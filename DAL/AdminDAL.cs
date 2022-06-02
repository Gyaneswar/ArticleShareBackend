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
    public class AdminDAL
    {
        public Boolean review(int articleid, string comments, Boolean approve)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "ArticleShare";
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd =
                        new SqlCommand("insert into articlelogs(articleid,articlestatus,statuschangedate,reviewcomments)" +
                        "values(@articleid,@articlestatus,@statuschangedate,@reviewcomments)", connection);
                    int userlevel = approve ? 4 : 3;
                    cmd.Parameters.AddWithValue("@articleid", articleid);
                    cmd.Parameters.AddWithValue("@articlestatus", userlevel);
                    cmd.Parameters.AddWithValue("@statuschangedate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@reviewcomments", comments);
                    try
                    {
                        cmd.ExecuteScalar();
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

        public List<Article> fetchArticles(Boolean dateAsc = false, Boolean dateDesc = false, string? name = "")
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
                        "on ad.articleid = al.articleid where al.articlestatus < 3";
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

        public List<Logs> fetchLogs(int articleId)
        {
            List<Logs> logs = new List<Logs>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "ArticleShare";
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string query = "select * from articlelogs where articleid = " + articleId;

                    connection.Open();
                    SqlCommand cmd =
                        new SqlCommand(query, connection);
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Logs log = new Logs();
                            log.articleid = reader.GetInt32("articleid");
                            log.articlestatus = reader.GetInt32("articlestatus");                                                        
                            if (reader["reviewcomments"] == DBNull.Value)
                            {
                                log.reviewComments = " ";
                            }                            
                            else
                            {
                                log.reviewComments = reader.GetString("reviewcomments");
                            }                        
                            log.articledate = reader.GetDateTime("statuschangedate");
                            logs.Add(log);
                        }
                        return logs;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return logs;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return logs;
            }
        }



    }
}
