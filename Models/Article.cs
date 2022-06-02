using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Article
    {
        [Required]
        public string email { get; set; }        
        public int? articleid { get; set; }
        public int articlestatus { get; set; }
        [Required]
        public string author { get; set; }
        [Required]
        public string articlename { get; set; }        
        public DateTime articledate { get; set; }
        [Required]
        public string? articlecontent { get; set; }
    }
}
