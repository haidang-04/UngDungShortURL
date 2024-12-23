using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDShortURL.Models;

namespace UDShortURL.Controllers
{
    public class DsShortUrlController : Controller
    {
        private readonly DsShortUrlContext _context;

        public DsShortUrlController(DsShortUrlContext context)
        {
            _context = context;
        }

        // GET: DsShortUrl
        public async Task<IActionResult> Index1(int page = 1)
        {
            int totalRecords = await _context.ShortUrls.CountAsync();

            // Tính số bản ghi trên mỗi trang để chia thành 5 trang
            int pageSize = (int)Math.Ceiling((double)totalRecords / 5);

            // Tính tổng số trang (vẫn sẽ là 5)
            int totalPages = 5;

            // Lấy dữ liệu cho trang hiện tại
            var shortUrls = await _context.ShortUrls
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gửi dữ liệu vào View
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(shortUrls);
        }


        // GET: DsShortUrl/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dsShortUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dsShortUrl == null)
            {
                return NotFound();
            }

            return View(dsShortUrl);
        }

        // GET: DsShortUrl/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dsShortUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dsShortUrl == null)
            {
                return NotFound();
            }

            return View(dsShortUrl);
        }

        // POST: DsShortUrl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dsShortUrl = await _context.ShortUrls.FindAsync(id);
            if (dsShortUrl != null)
            {
                _context.ShortUrls.Remove(dsShortUrl);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index1));
        }

        private bool DsShortUrlExists(int id)
        {
            return _context.ShortUrls.Any(e => e.Id == id);
        }
    }
}
