using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDShortURL.Models;

namespace UDShortURL.Controllers
{
    public class ShortUrlController : Controller
    {
        private readonly DsShortUrlContext _context;

        public ShortUrlController(DsShortUrlContext context)
        {
            _context = context;
        }

        // GET: Hiển thị trang nhập URL dài
        public IActionResult TrangTao()
        {
            return View();
        }

        // POST: Tạo URL ngắn từ URL dài
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string longUrl, string expirationOption, string expiredTime)
        {
            // Kiểm tra xem URL có hợp lệ không
            Uri uriResult;
            bool isUrlValid = Uri.TryCreate(longUrl, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isUrlValid)
            {
                ViewBag.ErrorMessage = "URL dài không hợp lệ hoặc không phải URL HTTP/HTTPS.";
                return View("TrangTao");
            }
            // Kiểm tra URL dài đã tồn tại
            var existingShortUrl = await _context.ShortUrls
                                                 .FirstOrDefaultAsync(s => s.LongUrl == longUrl);
            if (existingShortUrl != null)
            {
                // URL đã tồn tại, trả về URL ngắn cũ
                ViewBag.ExistingShortUrl = existingShortUrl.ShortUrl1;
                
                return View("TrangTao");
            }


            // Tạo ID ngắn từ GUID
            var shortId = GenerateShortId();

            // Khởi tạo thời gian hết hạn là null (mặc định)
            DateTime? expiredDate = null;

            // Kiểm tra thời gian hết hạn nếu người dùng nhập qua "datetime-local"
            if (!string.IsNullOrEmpty(expiredTime) && DateTime.TryParse(expiredTime, out DateTime parsedExpiredTime))
            {
                expiredDate = parsedExpiredTime;
            }
            else if (expirationOption == "1hour")
            {
                expiredDate = DateTime.Now.AddHours(1); // Thêm 1 giờ
            }
            else if (expirationOption == "1day")
            {
                expiredDate = DateTime.Now.AddDays(1); // Thêm 1 ngày
            }
            else if (expirationOption == "1week")
            {
                expiredDate = DateTime.Now.AddDays(7); // Thêm 1 tuần
            }

            // Tạo đối tượng ShortUrl
            var shortUrl = new DsShortUrl
            {
                ShortUrl1 = $"{Request.Scheme}://{Request.Host}/{shortId}",
                LongUrl = longUrl,
                CreatedAt = DateTime.Now,
                ExpiredTime = expiredDate,  // Lưu thời gian hết hạn nếu có
                RedirectCount = 0
            };

            // Lưu URL vào cơ sở dữ liệu
            
            try
            {
                _context.ShortUrls.Add(shortUrl); // Thao tác thêm dữ liệu
                await _context.SaveChangesAsync(); // Lưu thay đổi
            }
            catch (DbUpdateException dbEx)
            {
                // Lỗi cập nhật cơ sở dữ liệu
                ViewBag.ErrorMessage = "Không thể lưu URL vào cơ sở dữ liệu. Vui lòng thử lại.";
                Console.WriteLine($"Lỗi cơ sở dữ liệu: {dbEx.Message}");
                return View("TrangTao");
            }
            catch (Exception ex)
            {
                // Lỗi khác
                ViewBag.ErrorMessage = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.";
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                return View("TrangTao");
            }

            // Trả về URL ngắn cho người dùng
            ViewBag.ShortUrl = shortUrl.ShortUrl1;
            return View("TrangTao");
        }

        // Chuyển hướng đến URL gốc từ URL ngắn
        [HttpGet("{id}")]
        public IActionResult RedirectToOriginal(string id)
        {
            var shortUrl = _context.ShortUrls.FirstOrDefault(s => s.ShortUrl1.EndsWith(id));

            if (shortUrl != null)
            {
                // Kiểm tra xem URL đã hết hạn chưa
                if (shortUrl.ExpiredTime.HasValue && shortUrl.ExpiredTime.Value < DateTime.Now)
                {
                    return NotFound("URL ngắn này đã hết hạn.");
                }

                // Tăng số lượt truy cập
                shortUrl.RedirectCount++;
                _context.Update(shortUrl);
                _context.SaveChanges();

                // Chuyển hướng người dùng đến URL gốc
                return Redirect(shortUrl.LongUrl);
            }

            return NotFound("URL ngắn không tồn tại.");
        }

        // Hàm tạo ID ngắn từ GUID
        private string GenerateShortId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }
    }
}
