using MvcApplication37.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication37.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            DataBaseContext db = new DataBaseContext();
            //db.Database.Initialize(false);
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

        // GET: /Home/
        public ActionResult Filter (string Query)
        {
            String s = Query;
            String[] words = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            DataBaseContext db = new DataBaseContext();
            List<Product> _tempList = new List<Product>();
            _tempList = db.Products.ToList();
            List<Product> Model = new List<Product>();
            if (words[0] == "0" || words[1] == "0")
            {
                for (int i = 0; i < _tempList.Count; i++)
                     if (words.Length > 1)
                            for (int j = 2; j < words.Length; j++)
                                if (_tempList[i].Color == words[j])
                                    Model.Add(_tempList[i]);
                return View(Model);

            }
            if (words.Length == 2)
            {
                for (int i = 0; i < _tempList.Count; i++)
                    if (_tempList[i].Price >= int.Parse(words[0]) &&
                        _tempList[i].Price <= int.Parse(words[1]))
                        Model.Add(_tempList[i]);
                return View(Model);
            }
            for (int i = 0; i < _tempList.Count; i++)
                if (_tempList[i].Price >= int.Parse(words[0]) &&
                    _tempList[i].Price <= int.Parse(words[1]))
                    if (words.Length > 1)
                        for (int j = 2; j < words.Length; j++)
                            if (_tempList[i].Color == words[j])
                                Model.Add(_tempList[i]);


            return View(Model);
        }
        public ActionResult ViewCategory(string CategoryName)
        {
            DataBaseContext db = new DataBaseContext();
            List<Product> _tempList = new List<Product>();
            _tempList = db.Products.ToList();
            List<Product> _productList = new List<Product>();
      
            for (int i = 0; i < _tempList.Count; i++)
                if (_tempList[i].Manufaturer == CategoryName)
                    _productList.Add(_tempList[i]);
            ViewBag.CategoryName = CategoryName;
            return View(_productList);

        }
        public ActionResult Index()
        {
            DataBaseContext db = new DataBaseContext();

            ViewBag.ProductList = db.Products.ToList();
            return View();
        }

        public ActionResult Find(string Query )

        {
            if (Query == null)
                return RedirectToAction("Index");
            if (Query.Length < 1)
                return RedirectToAction("Index");
            DataBaseContext db = new DataBaseContext();
            List<Product> _tempList = new List<Product>();
            _tempList = db.Products.ToList();
            Query = Query.Trim().ToLower();
            List<Product> _productList = new List<Product>();
            for (int i = 0; i < _tempList.Count; i++)
            {
                if (_tempList[i].Manufaturer.Trim().ToLower() == Query || _tempList[i].PhoneModel.Trim().ToLower() == Query)
                    _productList.Add(_tempList[i]);
            }
            return View(_productList);
        }

        public ActionResult About()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            

            return View();
        }
        public ActionResult Clients()
        {
            

            return View();
        }
        public ActionResult ViewItem(int id)
        {
            DataBaseContext db = new DataBaseContext();
            var FindItem = db.Products
            .Where(c => c.id == id)
            .FirstOrDefault();
            return View(FindItem);
        }
    }
}
