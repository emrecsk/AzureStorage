using AzureStrorageLibrary;
using Microsoft.AspNetCore.Mvc;
using WebApp.Common;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage? _blobStorage;

        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<IActionResult> Index() 
        {            
            TempData.Keep("message");
            ViewBag.message = TempData.Get<NotificationViewModel>("message");
            List<FileBlobViewModel> fileBlobViewModel = new();
            string blobURL = $"{_blobStorage!.BlobURL}/{EContainerName.pictures}";
            List<string> names = _blobStorage.GetNames(EContainerName.pictures);
            fileBlobViewModel = names.Select(name => new FileBlobViewModel
            {
                blobName = name,
                blobURL = $"{blobURL}/{name}"
            }).ToList();

            ViewBag.logs = await _blobStorage.GetLogAsync("log.txt");

            return await Task.FromResult(View(fileBlobViewModel));
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            NotificationViewModel notificationViewModel = new();
            if (file != null)                
            {
                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                if (await _blobStorage!.UploadBlobAsync(EContainerName.pictures, newFileName, file.OpenReadStream()))
                {                    
                    notificationViewModel.CssClass = "alert-success";
                    notificationViewModel.Message = $"File named {newFileName} uploaded successfully";
                    notificationViewModel.Title = "Success!";
                    await _blobStorage.SetLog($"File uploaded successfully: {newFileName}", "log.txt");
                }
                else
                {                    
                    notificationViewModel.CssClass = "alert-danger";
                    notificationViewModel.Message = $"File named {newFileName} uploaded failed";
                    notificationViewModel.Title = "Fail!";
                }                
            }
            else
            {                
                notificationViewModel.CssClass = "alert-warning";
                notificationViewModel.Message = $"File must be attached!!!";
                notificationViewModel.Title = "Warning!";                
                await _blobStorage!.SetLog("File must be attached!!!", "log.txt");
            }
            TempData.Put("message", notificationViewModel);            
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Download (string blobName)
        {
            var stream = await _blobStorage!.DownloadBlobAsync(EContainerName.pictures, blobName);
            return File(stream, "application/octet-stream", blobName);
        }
        public async Task<IActionResult> Delete(string blobName)
        {
            if (await _blobStorage!.DeleteBlobAsync(EContainerName.pictures, blobName))
            {
                TempData.Put("message", new NotificationViewModel() { CssClass = "alert-primary", Message = "File deleted successfully", Title = "Success!" });
                
                await _blobStorage.SetLog($"File deleted successfully: {blobName}", "log.txt");
            }
            else
            {
                TempData.Put("message", new NotificationViewModel() { CssClass = "alert-danger", Message = "File deletion failed", Title = "Fail!" });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}