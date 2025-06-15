using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SStack.Models;
using SStack.ViewModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SStack.ApiModels;
using System.Diagnostics;
using SStack.ApiServices;
using System.Web.Services.Description;
using System.Web.Configuration;

namespace SStack.Controllers
{
    public class HomeController : Controller
    {
        private SmoothStackEntities db = new SmoothStackEntities();
        public async Task<ActionResult> Index()
        {
            string apiBaseUrl = WebConfigurationManager.AppSettings["baseURL"];
            //string apiBaseUrl = "https://localhost:44366";
            var ordersService = new OrdersService(apiBaseUrl);

            var products = await ordersService.GetProductsAsync();
            var orders = await ordersService.GetOrdersAsync();


            HomeVM vm = new HomeVM { Products = products.ToList(), CustomerOrders = orders.ToList() };
            return View(vm);
        }
        public ActionResult AddProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Item Not Found");
            }

            var qryProduct = db.Products.Find(id);
            AddProductVM vm = new AddProductVM { Product = qryProduct };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(FormCollection frm)
        {
            string apiBaseUrl = WebConfigurationManager.AppSettings["baseURL"];
            int orderId = Convert.ToInt16(frm["hdnOrderId"]);

            Debug.WriteLine("                  &  & & OrderID: " + orderId);
            var service = new OrdersService(apiBaseUrl);

            var updateOrder = new UpdateOrder
            {
                 OrderId = orderId, Status="SHIPPED"
            };

            bool success = await service.UpdateOrderAsyc(updateOrder);

            Debug.WriteLine(success ? "Order updated successfully!" : "Failed to update order.");

            return RedirectToAction("");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReceiveGoods(FormCollection frm)
        {
            string apiBaseUrl = WebConfigurationManager.AppSettings["baseURL"];
            int orderId = Convert.ToInt16(frm["hdnOrderId2"]);

            Debug.WriteLine("                  &  & & OrderID: " + orderId);
            var service = new OrdersService(apiBaseUrl);

            var updateOrder = new UpdateOrder
            {
                OrderId = orderId,
                Status = "DELIVERED"
            };

            bool success = await service.UpdateOrderAsyc(updateOrder);

            Debug.WriteLine(success ? "Order updated successfully!" : "Failed to update order.");

            return RedirectToAction("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(FormCollection frm, int id)
        {
            string apiBaseUrl = WebConfigurationManager.AppSettings["baseURL"];
            //get QRT
            int qty = Convert.ToInt16(frm["QTY"]);
            //get loggedin user ID
            string userId = User.Identity.GetUserId();
            //product Id
            int productId = id;


            //call API to save Item on user Cart          
          

            Debug.WriteLine($" = = = = = = = = = =  occurred:");
            PostOrder order = new PostOrder { Id = userId, ProductId = productId, qty = qty };
            var service = new OrdersService(apiBaseUrl);
            bool success = await service.AddProductToOrder(order);

            Debug.WriteLine(success ? "Order updated successfully!" : "Failed to update order.");

            return RedirectToAction("");

            //return View();
        }

        public async Task<ActionResult> ProductSales()
        {
            string apiBaseUrl = WebConfigurationManager.AppSettings["baseURL"];
           
            var ordersService = new OrdersService(apiBaseUrl);

            var productsSales = await ordersService.GetProductsSales();     
            return View(productsSales);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}