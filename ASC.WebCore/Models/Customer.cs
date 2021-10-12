using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.WebCore.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Avaiable { get; set; }
        public decimal Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public int TimeWork { get; set; }
    }
}
