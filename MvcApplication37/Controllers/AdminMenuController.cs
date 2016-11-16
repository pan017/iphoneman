using MvcApplication37.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcApplication37.Controllers
{
    public class AdminMenuController : Controller
    {
        public AdminMenuController()
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
        //
        // GET: /AdminMenu/
        private bool ValidateUser(string Login, string Password)
        {
            bool isValid = false;
 
            using (DataBaseContext _db = new DataBaseContext())
            {
                try
                {
                    var FindUser = _db.Admins
                    .Where(c => c.Login == Login)
                    .FirstOrDefault();

                    if (FindUser != null)
                    {
                        if (FindUser.Password == Password)
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DeleteSale(int id)
        {
            DataBaseContext db = new DataBaseContext();
            var EditSale = db.Sales
            .Where(c => c.id == id)
            .FirstOrDefault();
            db.Sales.Remove(EditSale);
            db.SaveChanges();

            return RedirectToAction("Menu");
 
        }
        public ActionResult LogOff()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Menu()
        {
            DataBaseContext db = new DataBaseContext();
            List<Sale> _saleList = new List<Sale>();
            _saleList = db.Sales.ToList();
            return View(_saleList);
        }
        public ActionResult Login(LoginModel model)
        {
            if (String.IsNullOrEmpty(model.Login) || String.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("ErrorLogin", "Заполните все поля");
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (ModelState.IsValid)
                {

                    if (ValidateUser(model.Login, Crypto.Hash(model.Password)))
                    {
                        //WebSecurity.Login(model.Login, );

                        //set auth cookie
                        FormsAuthentication.SetAuthCookie(model.Login, false);
                        return RedirectToAction("Menu");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Неправильный пароль или логин");
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                return RedirectToAction("Index", "Home");
            }
            catch 
            {
                return RedirectToAction("Index","Home");
            }
            
            //if (Model.Login == null || Model.Password == null)
            //    return RedirectToAction("Index", "Home");
            //bool q;
            //string s = Crypto.SHA256(Model.Password);
            //if (s == "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92")
            //{
            //    DataBaseContext db = new DataBaseContext();
            //    List<Product> ProductList = new List<Product>();
            //    ProductList = db.Products.ToList();
            //    ViewBag.ProductList = ProductList;
            //    return View();
            //}
            //else
                
        }
        [Authorize]
        public ActionResult AddProduct()
        {     
           List<string> color = new List<string>();
           color.Add("Черный");
           color.Add("Белый");
           color.Add("Серый");
           color.Add("Синий");
           color.Add("Фиолетовый");
           color.Add("Красный");
           color.Add("Зеленый");
           SelectList colors = new SelectList(color, 0);
           ViewBag.colors = colors;
           List<string> os = new List<string>();
           os.Add("IOS");
           os.Add("Android");
           os.Add("Microsoft");
           SelectList oss = new SelectList(os, 0);
           ViewBag.os = oss;
           List<string> screen = new List<string>();
           screen.Add("TFT");
           screen.Add("IPS");
           screen.Add("AMOLED");
           screen.Add("PVA (SLCD)");
           screen.Add("STN");
           screen.Add("TFD");
           screen.Add("3D LCD");
           SelectList screens = new SelectList(screen, 0);
           ViewBag.screen = screens;
           return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddProduct(Product model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    string Extention = System.IO.Path.GetExtension(upload.FileName);
                    string NewFileName = Crypto.HashPassword(fileName);
                    NewFileName = NewFileName.Remove(0, 20);
                    NewFileName += Extention;
                    NewFileName = NewFileName.Replace('/', 'w');
                    NewFileName = NewFileName.Replace('\\', 'a');
                    NewFileName = NewFileName.Replace('+', 't');
                    upload.SaveAs(Server.MapPath("~/Photos/" + NewFileName));
                    model.PhotoUrl = "/Photos/" + NewFileName;
                }
                DataBaseContext db = new DataBaseContext();
                db.Products.Add(model);
                db.SaveChanges();
                ModelState.AddModelError("add", "Товар добавлен");
                return RedirectToAction("AddProduct", "AdminMenu");

            }
            else
            {
                ModelState.AddModelError("add", "Ошбика ввода данных");
                return RedirectToAction("AddProduct", "AdminMenu");
            }

        }
        [Authorize]
        public ActionResult EditProductList()
        {
            DataBaseContext db = new DataBaseContext();
            ViewBag.ProductList = db.Products.ToList();
            return View();
        }
       
        public ActionResult EditItem(int id)
        {
            DataBaseContext db = new DataBaseContext();
            List<string> color = new List<string>();
            color.Add("Черный");
            color.Add("Белый");
            color.Add("Серый");
            color.Add("Синий");
            color.Add("Фиолетовый");
            color.Add("Красный");
            color.Add("Зеленый");
            SelectList colors ;
            List<string> os = new List<string>();
            os.Add("IOS");
            os.Add("Android");
            os.Add("Microsoft");
            SelectList oss ;
            SelectList screens;
            List<string> screen = new List<string>();
            screen.Add("TFT");
            screen.Add("IPS");
            screen.Add("AMOLED");
            screen.Add("PVA (SLCD)");
            screen.Add("STN");
            screen.Add("TFD");
            screen.Add("3D LCD");
            
            var EditProduct = db.Products
            .Where(c => c.id == id)
            .FirstOrDefault();
            for (int i = 0; i < color.Count; i++)
            {
                if (color[i] == EditProduct.Color)
                {
                    colors  = new SelectList(color, i);
                    ViewBag.colors = colors;
                    
                }
            }
            for (int i = 0; i < os.Count; i++)
            {
                if (os[i] == EditProduct.OS)
                {
                    oss = new SelectList(os, i);
                    ViewBag.os = oss;
                }
            }
            for (int i = 0; i < screen.Count; i++)
            {
                if (screen[i] == EditProduct.ScreenTechnology)
                {
                    screens = new SelectList(screen, i);
                    ViewBag.screen = screens;
                }
            }
            return View(EditProduct);
        }

        public ActionResult DeleteItem(int id)
        {
            DataBaseContext db = new DataBaseContext();
            List<Sale> _salesList = new List<Sale>();
            _salesList = db.Sales.ToList();
            for (int i = 0; i < _salesList.Count; i++)
            {
                var delSale = db.Sales
                    .Where(q => q.Product.id == id)
                    .FirstOrDefault();
                if (delSale != null)
                {
                    db.Sales.Remove(delSale);
                    db.SaveChanges();
                }
            }
            
            var EditProduct = db.Products
            .Where(c => c.id == id)
            .FirstOrDefault();
            if (EditProduct != null)
                db.Products.Remove(EditProduct);
            db.SaveChanges();
            ViewBag.ProductList = db.Products.ToList();
            return RedirectToAction("EditProductList");
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditItem(Product model, HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                string Extention = System.IO.Path.GetExtension(upload.FileName);
                string NewFileName = Crypto.HashPassword(fileName);
                NewFileName = NewFileName.Remove(0, 20);
                NewFileName += Extention;
                NewFileName = NewFileName.Replace('/', 'w');
                NewFileName = NewFileName.Replace('\\', 'a');
                NewFileName = NewFileName.Replace('+', 't');
                upload.SaveAs(Server.MapPath("~/Photos/" + NewFileName));
                model.PhotoUrl = "/Photos/" + NewFileName;
            }
            DataBaseContext db = new DataBaseContext();
            var EditProduct = db.Products
            .Where(c => c.id == model.id)
            .FirstOrDefault();
            EditProduct.Camera = model.Camera;
            EditProduct.ColoCount = model.ColoCount;
            EditProduct.Color = model.Color;
            EditProduct.Count = model.Count;
            EditProduct.DoubleSIM = model.DoubleSIM;
            EditProduct.FlasMemory = model.FlasMemory;
            EditProduct.Height = model.Height;
            EditProduct.Manufaturer = model.Manufaturer;
            EditProduct.Material = model.Material;
            EditProduct.OS = model.OS;
            EditProduct.OSVersion = model.OSVersion;
            EditProduct.PhoneModel = model.PhoneModel;
            
            EditProduct.Price = model.Price;
            EditProduct.Processor = model.Processor;
            EditProduct.ProcessorClockSpeed = model.ProcessorClockSpeed;
            EditProduct.RAM = model.RAM;
            EditProduct.Screen = model.Screen;
            EditProduct.ScreenSize = model.ScreenSize;
            EditProduct.ScreenTechnology = model.ScreenTechnology;
            EditProduct.Type = model.Type;
            EditProduct.Weight = model.Weight;
            EditProduct.Width = model.Width;

            if (upload != null)
                EditProduct.PhotoUrl = model.PhotoUrl;
            db.SaveChanges();
            return RedirectToAction("EditProductList");
        }

        [HttpPost]
        public JsonResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Photos/" + fileName));
                }
            }
            return Json("файл загружен");
        }
    }
}
