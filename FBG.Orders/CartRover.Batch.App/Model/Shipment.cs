using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App.Model
{
    public class Shipment
    {
        public string carrier { get; set; }
        public string method { get; set; }
        public string tracking_no { get; set; }
        public object tracking_no_secondary { get; set; }
        public object sscc_barcode { get; set; }
        public object bill_of_lading { get; set; }
        public string total_cost { get; set; }
        public object package_weight_lbs { get; set; }
        public object dim_weight_lbs { get; set; }
        public object zone { get; set; }
        public object delivery_surcharge_type { get; set; }
        public object whs_location { get; set; }
        public object box_length_in { get; set; }
        public object box_width_in { get; set; }
        public object box_height_in { get; set; }
        // Public Property date As DateTime
        public string tracking_url { get; set; }
        public object custom_1 { get; set; }
        public object custom_2 { get; set; }
        public object custom_3 { get; set; }
        public List<Item2> items { get; set; }
    }
}
