using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPA
{
    public class Items
    {
        public int item_id { get; set; }
        public string item_name { get; set; }
        public int item_price { get; set; }
        public int sale_percent { get; set; }

        public Items()
        {

        }
        public Items(int item_id, string item_name,int item_price,int sale_percent)
        {
            this.item_id = item_id;
            this.item_name = item_name;
            this.item_price = item_price;
            this.sale_percent = sale_percent;
        }
    }
}
