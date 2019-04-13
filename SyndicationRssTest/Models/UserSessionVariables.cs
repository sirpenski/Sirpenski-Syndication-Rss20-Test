using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SyndicationRssTest.Models
{
    [Serializable]
    public class UserSessionVariables
    {
        public string LastReadUrl { get; set; } = "";
        public int LastReadOutputFormat { get; set; } = 0;
        public int LastCreateTestID { get; set; } = 0;
        public int LastCreateOutputFormat { get; set; } = 0;

        public UserSessionVariables() { }
    }
}
