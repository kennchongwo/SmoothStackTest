using SStack.ApiModels;
using SStack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStack.ViewModel
{
    public class HomeVM
    {
        public List<StoreProduct> Products { get; set; }
        public List<StoreOrder> CustomerOrders { get; set; }
    }
}