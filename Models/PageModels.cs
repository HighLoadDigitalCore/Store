using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smoking.Models
{
    public class LastChangeModel
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string UserName { get; set; }
        public string UserLink { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string Arg
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return "c" + ID;
                    case 1:
                        return "p" + ID;
                    case 2:
                        return "x" + ID;
                    default:
                        return "r0";
                }
            }
        }
    }
}