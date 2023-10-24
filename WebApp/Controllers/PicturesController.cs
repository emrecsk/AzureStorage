using AzureStrorageLibrary;
using AzureStrorageLibrary.Models;
using AzureStrorageLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApp.Common;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PicturesController : Controller
    {
        private readonly INoSqlStorage<UserPicture> _storage;
        private readonly IBlobStorage _blobStorage;

        public PicturesController(INoSqlStorage<UserPicture> storage, IBlobStorage blobStorage)
        {
            _storage = storage;
            _blobStorage = blobStorage;
        }

        public string UserId { get; set; } = "1";
        public string City { get; set; } = "New York";

        public async Task<IActionResult> Index()
        {
            TempData.Keep("message");
            ViewBag.message = TempData.Get<NotificationViewModel>("message");
            ViewBag.UserId = UserId;
            ViewBag.City = City;
            List<FileBlobViewModel> fileblobs = new();
            var user = await _storage.Get(UserId,City);            
            if (user != null)
            {
                fileblobs.AddRange(user.Paths.Select(p => new FileBlobViewModel { blobName = p, blobURL = $"{_blobStorage.BlobURL}/{EContainerName.pictures}/{p}" }));
            }
            ViewBag.paths = fileblobs;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(IEnumerable<IFormFile> pictures)
        {
            List<string> imagepaths = new();
            NotificationViewModel notificationViewModel = new();
            foreach (var item in pictures)
            {
                var newname = $"{Guid.NewGuid()}{Path.GetExtension(item.FileName)}"; // generate a new name with extension
                
                await _blobStorage.UploadBlobAsync(EContainerName.pictures, newname, item.OpenReadStream()); // upload to blob storage
                imagepaths.Add(newname); // add to list
            }

            var isUser = await _storage.Get(UserId,City); // get user
            if (isUser == null)
            {
                await _storage.Create(new UserPicture { PartitionKey = UserId, RowKey = City, Paths = imagepaths }); // create user
            }
            else
            {
                imagepaths.AddRange(isUser.Paths); // add new paths to existing paths                
            }

            await _storage.Update(new UserPicture { PartitionKey = UserId, RowKey = City, Paths = imagepaths, ETag = "*" }); // update user
            notificationViewModel.CssClass = "alert-success";
            notificationViewModel.Message = $"File(s) uploaded successfully";
            notificationViewModel.Title = "Success!";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddWaterMark(PictureWaterMarkQueue pictureWaterMarkQueue)
        {
            var sjonstring = JsonConvert.SerializeObject(pictureWaterMarkQueue);
            string JsonStringBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(sjonstring));
            AzQueue azQueue = new AzQueue("watermark");
            await azQueue.SendMessageAsync(JsonStringBase64);

            return Ok();
        }
    }
}
