using AzureStrorageLibrary;
using AzureStrorageLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class TableStoragesController : Controller
    {
        private readonly INoSqlStorage<Product> _tableStorage;

        public TableStoragesController(INoSqlStorage<Product> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.products = _tableStorage.GetAll().ToList();    
            ViewBag.message = TempData["message"];            
            return await Task.FromResult(View());
        }
        public IActionResult Create()
        {            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            product.RowKey = Guid.NewGuid().ToString();            
            _tableStorage.Create(product);
            return RedirectToAction("Index");
        }        
        public async Task<IActionResult> Update(string partitionKey, string rowKey)
        {
            var product = await _tableStorage.Get(partitionKey, rowKey);            
            ViewBag.IsUpdate = true;
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            //product.ETag = "*"; // this is for overriding the existing data because of concurrency
            await _tableStorage.Update(product);
            TempData["message"] = "Product updated successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _tableStorage.Delete(partitionKey, rowKey);
            TempData["message"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Query(double price)
        {
            var products = _tableStorage.Query(p => p.Price > price).ToList();
            ViewBag.products = products;
            return await Task.FromResult(View("Index"));            
        }
    }
}
