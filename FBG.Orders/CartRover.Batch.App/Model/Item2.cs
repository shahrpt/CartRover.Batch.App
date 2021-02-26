using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App.Model
{
    public class Item2
    {
        public string item { get; set; }
        public string quantity { get; set; }
        public object carton_code { get; set; }
        public object carton_num { get; set; }
        public object box_length_in { get; set; }
        public object box_width_in { get; set; }
        public object box_height_in { get; set; }
        public object package_weight_lbs { get; set; }
        public object lot_code { get; set; }
        public object custom_1 { get; set; }
    }
}
