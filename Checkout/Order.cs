using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout
{
    class Order
    {
        public decimal id { get; set; }
        public int productId { get; set; }
        public int qty { get; set; }
    }
}
