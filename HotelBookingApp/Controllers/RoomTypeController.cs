using HotelBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Numerics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly HotelDbContext _context;

        public RoomTypeController(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Models.RoomType> RoomTypeList = (IEnumerable<Models.RoomType>)_context.RoomTypes;
            return View(RoomTypeList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);

            if (roomType == null)
            {
                return NotFound();
            }

            var roomTypeImages = _context.RoomTypeImages
                                       .Where(rt => rt.RoomTypeId == id)
                                       .ToList();

            // Add the fetched images to the roomType object
            roomType.Images = roomTypeImages;

            // Set a flag indicating whether the list of images is empty
            ViewBag.HasImages = roomTypeImages.Any();

            return View(roomType);
        }

        // Shows form to create a new roomtype
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,BasePrice,Capacity")] RoomType roomType, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Room", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }
                        var imageUrl = $"/Images/Room/{fileName}"; // Ensure the path is correct
                        roomType.Images.Add(new RoomTypeImage { ImageUrl = imageUrl });
                    }
                }

                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var roomType = await _context.RoomTypes
                .Include(rt => rt.Images)
                .FirstOrDefaultAsync(rt => rt.RoomTypeId == id);

            if (roomType == null)
            {
                return NotFound();
            }

            var viewModel = new RoomTypeViewModel
            {
                RoomTypeId = roomType.RoomTypeId,
                Name = roomType.Name,
                Description = roomType.Description,
                BasePrice = roomType.BasePrice,
                Capacity = roomType.Capacity,
                Images = roomType.Images.Select(img => new RoomTypeImageViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl 
                }).ToList()
            };

            return View(viewModel);
        }

        ////Post
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(RoomType obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingRoomType = await _context.RoomTypes.FindAsync(obj.RoomTypeId);

        //        if (existingRoomType != null)
        //        {
        //            existingRoomType.RoomTypeName = obj.RoomTypeName;
        //            existingRoomType.RoomTypeDescription = obj.RoomTypeDescription;

        //            await _context.SaveChangesAsync();

        //            TempData["SuccessMessage"] = "Room Type edited successfully";

        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(obj);
        //}

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var roomType = await _context.RoomTypes
                    .Include(rt => rt.Images)
                    .FirstOrDefaultAsync(rt => rt.RoomTypeId == viewModel.RoomTypeId);

                if (roomType != null)
                {
                    roomType.Name = viewModel.Name;
                    roomType.Description = viewModel.Description;
                    roomType.BasePrice = viewModel.BasePrice;
                    roomType.Capacity = viewModel.Capacity;

                    // Handle new images
                    if (viewModel.NewImages != null && viewModel.NewImages.Count > 0)
                    {
                        foreach (var file in viewModel.NewImages)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Room", file.FileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            var imageUrl = $"/Images/Room/{file.FileName}";
                            roomType.Images.Add(new RoomTypeImage { ImageUrl = imageUrl });
                        }
                    }

                    // Handle image deletions
                    if (viewModel.ImagesToDelete != null)
                    {
                        foreach (var id in viewModel.ImagesToDelete)
                        {
                            var imageToDelete = roomType.Images.FirstOrDefault(img => img.Id == id);
                            if (imageToDelete != null)
                            {
                                _context.RoomTypeImages.Remove(imageToDelete);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Room Type edited successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Room Type not found.";
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var roomTypeFromDb = _context.RoomTypes
                .Include(rt => rt.Images)  // Assuming you want to show images in the delete view
                .FirstOrDefault(rt => rt.RoomTypeId == id);

            if (roomTypeFromDb == null)
            {
                return NotFound();
            }

            // Creating a ViewModel to pass to the view
            var roomTypeViewModel = new RoomTypeViewModel
            {
                RoomTypeId = roomTypeFromDb.RoomTypeId,
                Name = roomTypeFromDb.Name,
                Description = roomTypeFromDb.Description,
                BasePrice = roomTypeFromDb.BasePrice,
                Capacity = roomTypeFromDb.Capacity,
                Images = roomTypeFromDb.Images.Select(img => new RoomTypeImageViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl  // Assuming ImageUrl is the property you store
                }).ToList()
            };

            return View(roomTypeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePOST(int? id)
        {
            var obj = _context.RoomTypes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _context.RoomTypes.Remove(obj);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Room Type deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
