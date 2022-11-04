using Joe_Pizza.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Joe_Pizza.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly Joe_PizzaContext _context;

        public HomeController(Joe_PizzaContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            HttpContext.Session.SetInt32("UserId", 1);
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<PizzaModel>? t = JsonConvert.DeserializeObject<List<PizzaModel>> ((string)TempData["Cart"]);
                foreach (var item in t)
                {
                    x += item.Bill;

                }

                TempData["total"] = x;
            }
            TempData.Keep();
            return View(_context.Pizzas.OrderByDescending(x => x.ProductId).ToList());

        }


        public ActionResult Addtocart(int? Id)
        {

            Pizza? p = _context.Pizzas.Where(x => x.ProductId == Id).SingleOrDefault();
            return View(p);
        }

        List<PizzaModel> li = new List<PizzaModel>();

        [HttpPost]
        public ActionResult Addtocart(Pizza pi, string qty, int Id)
        {
            Pizza? p = _context.Pizzas.Where(x => x.ProductId == Id).SingleOrDefault();

            PizzaModel c = new PizzaModel();
            c.productid = p.ProductId;
            c.price = (float)p.ProductPrice;
            c.Quantity = Convert.ToInt32(qty);
            c.Bill = c.price * c.Quantity;
            c.productname = p.ProductName;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = JsonConvert.SerializeObject(li);

            }
            else
            {
                List<PizzaModel>? l = JsonConvert.DeserializeObject<List<PizzaModel>>((string)TempData["Cart"]);
                int flag = 0;
                foreach (var item in l)
                {
                    if (item.productid == c.productid)
                    {
                        item.Quantity += c.Quantity;
                        item.Bill += c.Bill;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    l.Add(c);
                }
                TempData["cart"] = JsonConvert.SerializeObject(l);
            }

            TempData.Keep();

            return RedirectToAction("ViewCart");
        }

        public ActionResult remove(int? id)
        {
            List<PizzaModel>? b = TempData["cart"] as List<PizzaModel>;
            PizzaModel? c = b.Where(ae => ae.productid == id).SingleOrDefault();
            b.Remove(c);
            float a = 0;
            foreach (var item in b)
            {
                a += item.Bill;
            }
            TempData["total"] = a;
            return RedirectToAction("ViewCart");
        }
        public ActionResult ViewCart()
        {
            TempData.Keep();


            return View();
        }
        [HttpPost]
        public ActionResult ViewCart(Order o)
        {
            List<PizzaModel>? list = JsonConvert.DeserializeObject<List<PizzaModel>>((string)TempData["Cart"]);
            Invoice i = new Invoice();
            i.InvoiceId = (int)HttpContext.Session.GetInt32("UserId")!;
            i.InvoiceDate = System.DateTime.Now;
            i.TotalBill = (float?)TempData["total"];
            _context.Invoices.Add(i);
            _context.SaveChanges();

            foreach (var item in list)
            {
                Order or = new Order();
                or.ProductId = item.productid;
                or.InvoiceNo = i.InvoiceId;
                or.OrderDate = System.DateTime.Now;
                or.OrderQty = item.Quantity;
                or.OrderUnitPrice = (int)item.price;
                or.OrderBill = item.Bill;
                _context.Orders.Add(or);
                _context.SaveChanges();

            }

            TempData.Remove("total");
            TempData.Remove("cart");

            TempData["msg"] = "Order Placed Successfully";
            TempData.Keep();


            return RedirectToAction("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}