using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTrackerMVC.Data;
using MovieTrackerMVC.Models;
using MovieTrackerMVC.Services;

namespace MovieTrackerMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MediaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly StorageService _storage;
        private readonly ILogger<MediaController> _logger;

        public MediaController(ApplicationDbContext context, StorageService storage, ILogger<MediaController> logger)
        {
            _context = context;
            this._storage = storage;
            this._logger = logger;
        }

        // GET: Media
        public async Task<IActionResult> Index()
        {

            return _context.Media != null ?
                        View(await _context.Media.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Media'  is null.");
        }

        // GET: Media/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Media == null)
            {
                return NotFound();
            }

            var media = await _context.Media
                .FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // GET: Media/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Media/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Media media, IFormFile? cover)
        {
            if (!ModelState.IsValid)
            {
                var firstErrorOfEachModel = ModelState.Where(x => x.Value?.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    .Select(x => new CreateMediaErrorDTO { id = x.Key, text = x.Value?.Errors.FirstOrDefault()?.ErrorMessage });

                return Json(new CreateMediaModelError(success: false, modelErrors: firstErrorOfEachModel));
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Add(media);
                await _context.SaveChangesAsync();

                if (cover == null)
                {
                    transaction.Commit();
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Media") });
                }

                var saved = await _storage.UploadCover(media.Id.ToString(), cover, _logger);
                if (saved)
                {
                    transaction.Commit();
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Media") });
                }

                // cover wasn't saved
                transaction.Rollback();
                _logger.LogError("StorageService.UploadCover unsuccessful in MediaController.Create. Changes were rolled back.");
                var coverError = new CreateMediaErrorDTO()
                {
                    id = "Cover",
                    text = "The cover could not be saved. Retry, use another cover or try to resize the current one."
                };
                return Json(new CreateMediaModelError(success: false, modelErrors: [coverError]));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("Exception raised while trying to save a media; Exception: {Exception}", ex);
                return Json(new CreateMediaModelError(success: false, []));
            }
        }

        private struct CreateMediaErrorDTO
        {
            [JsonInclude]
            public string id;
            [JsonInclude]
            public string? text;
        }

        private class CreateMediaModelError
        {
            [JsonInclude]
            public readonly bool success;
            [JsonInclude]
            public readonly IEnumerable<CreateMediaErrorDTO> modelErrors;

            public CreateMediaModelError(bool success, IEnumerable<CreateMediaErrorDTO> modelErrors)
            {
                this.success = success;
                this.modelErrors = modelErrors;
            }
        }



        // GET: Media/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Media == null)
            {
                return NotFound();
            }

            var media = await _context.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }
            return View(media);
        }

        // POST: Media/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title")] Media media)
        {
            if (id != media.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(media);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaExists(media.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(media);
        }

        // GET: Media/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Media == null)
            {
                return NotFound();
            }

            var media = await _context.Media
                .FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Media == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Media'  is null.");
            }
            var media = await _context.Media.FindAsync(id);
            if (media != null)
            {
                _context.Media.Remove(media);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MediaExists(long id)
        {
            return (_context.Media?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
