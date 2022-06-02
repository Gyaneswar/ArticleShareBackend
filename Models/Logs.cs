using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Logs
    {        
        public int? articleid { get; set; }
        public int? articlestatus { get; set; }             
        public DateTime articledate { get; set; }        
        public string? reviewComments { get; set; }
    }
}
