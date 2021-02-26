using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App.Model
{
    public class Item
    {
        public string line_no { get; set; }
        public string item { get; set; }
        public object lot_number { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
        public string discount { get; set; }
        public string addl_discount { get; set; }
        public string extended_amount { get; set; }
        public string tax { get; set; }
        public string shipping_surcharge { get; set; }
        public string line_item_id { get; set; }
        public string line_comment { get; set; }
        public string Description { get; set; }
        public object alt_sku { get; set; }
        public string filtered_sku { get; set; }
        public string line_location { get; set; }
    }

}
