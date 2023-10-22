using e_com.EF;
using e_com.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace e_com.Controllers
{
    public class CartController : Controller
    {
        E_CommEntities db  = new E_CommEntities();
        // GET: Cart
        public ActionResult Index()
        {
            string errorMessage = TempData["ErrorMessage"] as string;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.Notification = errorMessage; // Pass the error message to the view
            }
            return View();
        }

        public ActionResult AddToCart(int ProductId) {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item()
                {
                    Product = db.Products.Find(ProductId),
                    Quantiy = 1
                }); 
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = IsInCart(ProductId);
                if(index != -1)
                {
                    cart[index].Quantiy++;
                }
                else
                {
                    cart.Add(new Item()
                    {
                        Product = db.Products.Find(ProductId),
                        Quantiy = 1
                    });
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int ProductId) {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = IsInCart(ProductId);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
        public ActionResult PlaceOrder()
        {
            if (Session["UserEmail"] != null && Session["cart"] != null)
            {
                var cart = (List<Item>)Session["cart"];
                string customerEmail = (string)Session["UserEmail"];
                var customer = db.Customers.SingleOrDefault(c => c.Email == customerEmail);
                if (customer != null)
                {
                    var order = new Order {
                        Customer = customer,
                        OrderDate = DateTime.Now,
                        ShippingAddress = "Shipping Address",
                        OrderNotes = "Order Notes",
                        TotalAmount = (from item in cart
                                       select item.Product.Price * item.Quantiy).Sum(),
                        OrderStatus= "Pending"
                    };
                    db.Orders.Add(order);

                    for (int i = 0; i < cart.Count; i++)
                    {
                        var product = db.Products.Find(cart[i].Product.Id);
                        var orderDetails = new OrderDetail
                        {
                            Order = order,
                            Product = product,
                            Quantity = cart[i].Quantiy,
                            UnitPrice = cart[i].Product.Price,
                            TotalAmount = cart[i].Product.Price * cart[i].Quantiy
                        };
                        db.OrderDetails.Add(orderDetails);
                    }
                    int rowAffected = db.SaveChanges();
                    if (rowAffected > 0)
                    {
                        Session.Remove("cart");
                    }

                }
                else
                {
                    ViewBag.Notification = "Invalid Customer";
                    return RedirectToAction("Index", "Cart");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "To place order you must have to login!";
                return RedirectToAction("Index", "Cart");
            }
                return RedirectToAction("OrderList", "Customer");
        }
        public int IsInCart(int ProductId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == ProductId)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}