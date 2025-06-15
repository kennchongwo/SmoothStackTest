using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStack.ApiModels
{
    public class PostOrder
    {
        public string Id { get; set; }
        public int ProductId  { get; set; }
        public int qty { get; set; }
    }
}