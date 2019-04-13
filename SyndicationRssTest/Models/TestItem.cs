using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicationRssTest.Models
{
    public class TestItem
    {
        public int id { get; set; } = 0;
        public string title { get; set; } = "";
        public string desc { get; set; } = "";

        public TestItem() { }
        public TestItem(int nID, string stitle, string sdesc)
        {
            id = nID;
            desc = sdesc;
            title = stitle;
        }
    }
}
