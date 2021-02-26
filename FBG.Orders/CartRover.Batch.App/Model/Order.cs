using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartRover.Batch.App.Model
{
    public class Order
    {
        public bool success_code { get; set; }
        public List<Response> response { get; set; }
    }
}
