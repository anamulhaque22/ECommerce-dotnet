using e_com.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_com.Models
{
    public class Item
    {
        public Product Product{ get; set; }
        public int Quantiy { get; set; }
    }
}