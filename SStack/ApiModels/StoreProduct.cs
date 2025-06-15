using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStack.ApiModels
{
    public class StoreProduct
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }
        public double Price { get; set; }
        public double MaxDiscount { get; set; }
        public double MinDiscount { get; set; }

    }
}