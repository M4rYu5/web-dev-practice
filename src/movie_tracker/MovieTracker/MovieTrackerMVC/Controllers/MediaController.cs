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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTrackerMVC.Controllers.CustomModels;
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
                var firstErrorOfEachModel = ModelError<ModelErrorDTO>.FromModelState(ModelState,
                    x => new ModelErrorDTO
                    {
                        Id = x.Key,
                        Text = x.Value?.Errors.FirstOrDefault()?.ErrorMessage
                    });

                return Json(firstErrorOfEachModel);
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
                var coverError = new ModelErrorDTO()
                {
                    Id = "Cover",
                    Text = "The cover could not be saved. Retry, use another cover or try to resize the current one."
                };
                return Json(new ModelError<ModelErrorDTO>(success: false, modelErrors: [coverError]));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("Exception raised while trying to save a media; Exception: {Exception}", ex);
                return Json(new ModelError<ModelErrorDTO>(success: false, []));
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



    namespace CustomModels
    {
        /// <summary>
        /// ModelError data transfer object <br/>
        /// Keeps data about the error provieded by the model.
        /// </summary>
        public readonly struct ModelErrorDTO
        {
            /// <summary>
            /// the model's id (usually name)
            /// </summary>
            [JsonInclude]
            [JsonPropertyName("id")]
            public string Id { get; init; }

            /// <summary>
            /// the error text
            /// </summary>
            [JsonInclude]
            [JsonPropertyName("text")]
            public string? Text { get; init; }
        }

        /// <summary>
        /// Return message, containing the success status, and a list of errors.
        /// </summary>
        public sealed class ModelError<T>
        {
            /// <summary>
            /// Status
            /// </summary>
            [JsonInclude]
            [JsonPropertyName("success")]
            public bool Success { get; init; }
            /// <summary>
            /// Keep a list of errors to be sent to client
            /// </summary>
            [JsonInclude]
            [JsonPropertyName("modelErrors")]
            public IEnumerable<T> ModelErrors { get; init; }

            /// <summary>
            /// Create a new ModelError response
            /// </summary>
            /// <param name="success"></param>
            /// <param name="modelErrors"></param>
            public ModelError(bool success, IEnumerable<T> modelErrors)
            {
                this.Success = success;
                this.ModelErrors = modelErrors;
            }


            public static ModelError<T> FromModelState(ModelStateDictionary ModelState, Func<KeyValuePair<string, ModelStateEntry?>, T> extractor)
            {
                var selected = ModelState.Where(x => x.Value?.ValidationState == ModelValidationState.Invalid).Select(extractor);
                return new ModelError<T>(success: false, modelErrors: selected);
            }

        }

    }



}
