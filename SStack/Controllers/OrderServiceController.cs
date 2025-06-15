using SStack.ApiModels;
using SStack.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SStack.Controllers
{
    /// <summary>
    /// API controller for Orders APP.
    /// </summary>
    public class OrderServiceController : ApiController
    {
        SmoothStackEntities db=new SmoothStackEntities();
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        /// <summary>
        /// Returns all the products in the db products table.
        /// </summary>         
        /// <returns>List of Products</returns>

        [HttpGet]
        [Route("api/OrderService/products")]
        [SwaggerResponse(HttpStatusCode.OK, "List of products", typeof(List<StoreProduct>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Server error")]
        public IHttpActionResult GetProducts()
        {
            List<StoreProduct> products = new List<StoreProduct>();
            foreach (var item in db.Products)
            {
                products.Add(new StoreProduct { MaxDiscount=item.MaxDiscount, ProductName=item.ProductName, MinDiscount=item.MinDiscount,ProductId=item.ProductId, Price=item.Price,ProductCategoryId=item.ProductCategoryId });  
            }

            Debug.WriteLine($" = = = = = = = = = =  occurred X");
            return Ok(products);
        }

        [HttpGet]
        [Route("api/OrderService/v2/products")]
        [SwaggerResponse(HttpStatusCode.OK, "List of products", typeof(List<StoreProduct>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Server error")]
        public IHttpActionResult GetProductss()
        {
            List<StoreProduct> products = new List<StoreProduct>();
            foreach (var item in db.Products)
            {
                products.Add(new StoreProduct { MaxDiscount = item.MaxDiscount, ProductName = item.ProductName, MinDiscount = item.MinDiscount, ProductId = item.ProductId, Price = item.Price, ProductCategoryId = item.ProductCategoryId });
            }

            Debug.WriteLine($" = = = = = = = = = =  occurred X");
            return Ok(products);
        }

        /// <summary>
        /// Returns all the orders in the db customer orders table.
        /// </summary>         
        /// <returns>List of Orders</returns>
        [HttpGet]
        [Route("api/OrderService/orders")]
        [SwaggerResponse(HttpStatusCode.OK, "List of Orders", typeof(List<StoreOrder>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Server error")]
        public IHttpActionResult GetOrders()
        {
            List<StoreOrder> orders= new List<StoreOrder>();
            foreach (var item in db.vwOrders)
            {
                orders.Add(new StoreOrder { MaxDiscount = item.MaxDiscount, ProductName = item.ProductName, MinDiscount = item.MinDiscount, ProductId = item.ProductId, Price = item.Price, Id=item.Id,EntryId=item.EntryId, EntryDate=item.EntryDate, FinalPrice=item.FinalPrice,OrderDate=item.OrderDate,OrderDescription=item.OrderDescription,OrderStatus=item.OrderStatus, Qty=item.Qty, CustomerOrderId=item.CustomerOrderId});
            }

            Debug.WriteLine($" = = = = = = = = = =  occurred X");
            return Ok(orders);
        }

        public double GetDiscountPercentage(string userId, int productId)// based on the location of the customer the method assigns discount percentage as setup in CustomerSegment setup table in the DB
        {
            var discount = 0.0;
            //GET THE PRODUCT FROM SETUP
            Product P = db.Products.Find(productId);
            //get customer
            AspNetUser user = db.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault();
            //get region to determine discount
            CustomerSegment segment = db.CustomerSegments.Find(user.CustomerSegmentId);
            if ((segment.Code == "USA" || segment.Code == "IND") && P.MaxDiscount > 0.0)
            {
                discount = P.MaxDiscount;
            }
            else if (segment.Code == "CND" && P.MaxDiscount > 0.0)
            {
                discount = P.MinDiscount;
            }
            else
            {
                //total price remains the same, No discount
                discount = 0.0;
            }
            return discount;
        }

        /// <summary>
        /// Saves customer Order
        /// </summary>         
        /// <returns>Succes or Fail Message</returns>
        [HttpPost]
        [Route("api/OrderService/PostOrder")]
        public IHttpActionResult Post([FromBody] PostOrder order)
        {
            if (order == null)
            {
                return BadRequest("Invalid request, Ensure valid values for the order record.");
            }
            //GET THE PRODUCT FROM SETUP
            Product P = db.Products.Find(order.ProductId);
            //get customer
            AspNetUser user=db.AspNetUsers.Where( u => u.Id== order.Id).FirstOrDefault();
            //get region to determine discount
            CustomerSegment segment=db.CustomerSegments.Find(user.CustomerSegmentId);
            //get discount

            //save Record
            //get an order that's still open, if not create a new customer and add item
            var qry = db.CustomerOrders.Where(c => c.Id == order.Id && c.OrderStatus == "OPEN");//ORDER STATUS, open -> open for items, (Placed, shipped & Delivered) -> not open for new items
            if (qry.Any())
            {

                CustomerOrder currentCustomerOrder = qry.First();

                //check if the productId being submited already exists in this order, if so increase quantity and update entry, if not create a new entry for the product being submitted
                var qryProduct = db.CustomerOrderItems.Where(o => o.CustomerOrderId == currentCustomerOrder.CustomerOrderId && o.ProductId == order.ProductId);
                if (qryProduct.Any())//update product -> increase quantity & update price
                {
                    CustomerOrderItem i = qryProduct.First();
                    i.Qty = i.Qty + order.qty;
                    //update price
                    var totalPrice = order.qty * P.Price;
                    totalPrice = totalPrice * ((100 - GetDiscountPercentage(order.Id, order.ProductId)))/100;

                    
                    i.FinalPrice = i.FinalPrice + totalPrice;
                    db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else//the product submitted does not exists is the current open order -> add new line
                {

                    var totalPrice = order.qty * P.Price;
                    totalPrice = totalPrice * ((100 - GetDiscountPercentage(order.Id, order.ProductId))) / 100;
                   
                    CustomerOrderItem customerOrderItem = new CustomerOrderItem { ProductId = order.ProductId, EntryDate = DateTime.Now, Qty = order.qty, CustomerOrderId = currentCustomerOrder.CustomerOrderId, FinalPrice = totalPrice };


                    db.CustomerOrderItems.Add(customerOrderItem);
                    db.SaveChanges();
                }
            }
            else
            {
                CustomerOrder customerOrder = new CustomerOrder { Id = order.Id, OrderDate = DateTime.Now, OrderStatus = "OPEN", OrderDescription = "-" };
                db.CustomerOrders.Add(customerOrder);
                db.SaveChanges();
                //add submitted product
                var totalPrice = order.qty * P.Price;
                totalPrice = totalPrice * ((100 - GetDiscountPercentage(order.Id, order.ProductId))) / 100;
               
                CustomerOrderItem customerOrderItem = new CustomerOrderItem { CustomerOrderId = customerOrder.CustomerOrderId, EntryDate = DateTime.Now, ProductId = order.ProductId, Qty = order.qty, FinalPrice = totalPrice };
                db.CustomerOrderItems.Add(customerOrderItem);
                db.SaveChanges();
            }

            //respond to the user
            string userName = "";
            var response = new
            {
                Message = $"Hello {userName}, age your order has been saved!"
            };

            return Ok(response);
        }
        /// <summary>
        /// Add a product to order or updates existing order if the order is still open
        /// </summary>         
        /// <returns>Succes oor fail message</returns>
        [HttpPost]
        [Route("api/OrderService/updateOrder")]
        public IHttpActionResult UpdateOrderStatus([FromBody] UpdateOrder updateOrder)
        {
            if (updateOrder == null || updateOrder.OrderId==0)
                return BadRequest("Invalid product.");

            // Update order Record
            CustomerOrder c=db.CustomerOrders.Find(updateOrder.OrderId);
            c.OrderStatus = updateOrder.Status;
            db.Entry(c).State=System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Ok(new { Message = "Order Updated" });            
        }
        /// <summary>
        /// Returns a summary of sales by product
        /// </summary>         
        /// <returns>List of product sales</returns>
        [HttpGet]
        [Route("api/OrderService/productsSales")]
        public IHttpActionResult GetProductsSales()
        {
            List<ProductSale> productsSales = new List<ProductSale>();
            foreach (var item in db.vwProductSales)
            {
                productsSales.Add(new ProductSale {ProductId=item.ProductId, ProductName=item.ProductName, QTY=item.QTY, Total=item.Total });
            }

            Debug.WriteLine($" = = = = = = = = = =  occurred Product Sales Retrieved");
            return Ok(productsSales);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}