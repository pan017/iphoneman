using MvcApplication37.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication37.Controllers
{

    public class CartController : Controller
    {
        public CartController() : base()
        {
            DataBaseContext db = new DataBaseContext();
            List<Product> _productList = new List<Product>();
            _productList = db.Products.ToList();
            List<string> _tempList = new List<string>();
            List<string> _tempMaterialList = new List<string>();
            List<string> _TempcolorList = new List<string>();
            for (int i = 0; i < _productList.Count; i++)
            {
                _tempList.Add(_productList[i].Manufaturer);
                _TempcolorList.Add(_productList[i].Color);
                _tempMaterialList.Add(_productList[i].Material);
            }
            List<string> _colorList = new List<string>(_TempcolorList.Distinct());
            List<string> _categoryList = new List<string>(_tempList.Distinct());
            List<string> _materialList = new List<string>(_tempMaterialList.Distinct());
            ViewBag.colorList = _colorList;
            ViewBag.Category = _categoryList;
            ViewBag.Material = _materialList;
        }
        public ActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
        //
        // GET: /Cart/
        private IProductsRepository repository;
        public CartController(IProductsRepository repo)
        {
            repository = repo;
        
        }
        public ActionResult GetCartCount()
        {
            DataBaseContext db = new DataBaseContext();
            Carts _cart = GetCart();

            int CartCount = 0;
            for (int i = 0; i < _cart.lineCollection.Count; i++)
            {
                for (int j = 0; j < _cart.lineCollection[i].Quantity; j++)
                {
                    CartCount++;
                }
            }
            ViewBag.CartCount = CartCount;
            return View();
        }

        public ActionResult ThankYou()
        {
            DataBaseContext db = new DataBaseContext();
            Sale _sale = new Sale();
            
            
            Carts _cart = GetCart();
            if (_cart.lineCollection.Count == 0)
                return RedirectToAction("Index", "Home");
            List<Sale> _Sales = new List<Sale>();
            Client c = new Client();
            List<Client> q = db.Clients.ToList();
            c = q[q.Count - 1];
            for (int i = 0; i < _cart.lineCollection.Count; i++)
            {
                for (int j = 0; j < _cart.lineCollection[i].Quantity; j++)
                {
                    _sale = new Sale();
                    _sale.Date = DateTime.Now;
                    _sale.Product = _cart.lineCollection[i].Product;
                    _sale.Client =c;
                    _Sales.Add(_sale);
                }
            }
            GetCart().Clear();
            return View(_Sales);
        }
        public ActionResult Order()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Order(Client model)
        {
            if (model== null)
            {
                ModelState.AddModelError("Error", "Ошибка! Заполните все поля");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Adres) || String.IsNullOrEmpty(model.Name) || String.IsNullOrEmpty(model.Phone))
            {
                ModelState.AddModelError("Error", "Ошибка! Заполните все поля");
                return View(model);
            }
            DataBaseContext db = new DataBaseContext();
            db.Clients.Add(model);
            db.SaveChanges();
            Sale _sale = new Sale();
            _sale.Client = db.Clients.Local[db.Clients.Local.Count-1];
            _sale.Date = DateTime.Now;
            Carts _cart = GetCart();
            List<Product> _productList = new List<Product>();
            _productList = db.Products.ToList();

            for (int i = 0; i < _cart.lineCollection.Count; i++)
            {
                for (int j = 0; j < _cart.lineCollection[i].Quantity; j++)
                {
                    int id = _cart.lineCollection[i].Product.id;
                    Product product = db.Products
               .FirstOrDefault(g => g.id == id);
                    _sale.Product = product;
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    
                }
            }
            return RedirectToAction("ThankYou", "Cart");
        }
        public ActionResult AddToCart(int productId, string returnUrl)
        {
            DataBaseContext db = new DataBaseContext();
            Product product = db.Products
                .FirstOrDefault(g => g.id == productId);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            if (returnUrl == "/")
                return RedirectToAction("Index", "Home");
            else
            return RedirectToAction("ViewItem", "Home", new { id = productId });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.id == productId);

            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Carts GetCart()
        {
            Carts cart = (Carts)Session["Car"];
            if (cart == null)
            {
                cart = new Carts();
                Session["Car"] = cart;
            }
            return cart;
        }
	}
    public interface IProductsRepository
    {
        IEnumerable<Product> Products { get; }
    }
    
}
