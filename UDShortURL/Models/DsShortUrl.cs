using System;
using System.ComponentModel.DataAnnotations;

namespace UDShortURL.Models
{
    public partial class DsShortUrl
    {
        // Id: không cần display vì nó thường được dùng làm khóa chính
        [Display(Name = "ID", Description = "Mã định danh duy nhất của URL rút gọn")]
        public int Id { get; set; }

        // LongUrl: Hiển thị với nhãn "URL Gốc"
        [Display(Name = "URL Gốc", Description = "Đây là URL ban đầu mà bạn muốn rút gọn.")]
        public string LongUrl { get; set; } = null!;

        // ShortUrl1: Hiển thị với nhãn "URL Rút Gọn"
        [Display(Name = "URL Rút Gọn", Description = "Đây là URL đã được rút gọn.")]
        public string ShortUrl1 { get; set; } = null!;

        // CreatedAt: Hiển thị với nhãn "Ngày Tạo"
        [Display(Name = "Ngày Tạo", Description = "Ngày và giờ URL này được tạo.")]
        public DateTime? CreatedAt { get; set; }

        // RedirectCount: Hiển thị với nhãn "Số Lần Chuyển Hướng"
        [Display(Name = "Số Lần Truy Cập")]
        public int? RedirectCount { get; set; }

        // Thêm cột ExpiredTime: Hiển thị với nhãn "Thời Gian Hết Hạn"
        [Display(Name = "Thời Gian Hết Hạn", Description = "Thời gian URL này hết hạn. Nếu không có thời gian hết hạn, để trống.")]
        public DateTime? ExpiredTime { get; set; }
    }
}
