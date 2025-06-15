using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStack.ApiModels
{
    public class ProductSale
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> QTY { get; set; }
        public Nullable<double> Total { get; set; }
    }
}