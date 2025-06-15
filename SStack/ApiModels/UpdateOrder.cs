using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStack.ApiModels
{
    public class UpdateOrder
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
    }
}