using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStack.ApiModels
{
    public class StoreOrder
    {
        public int EntryId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int CustomerOrderId { get; set; }
        public System.DateTime EntryDate { get; set; }
        public double FinalPrice { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Id { get; set; }
        public string OrderDescription { get; set; }
        public string OrderStatus { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public double MaxDiscount { get; set; }
        public double MinDiscount { get; set; }
    }
}