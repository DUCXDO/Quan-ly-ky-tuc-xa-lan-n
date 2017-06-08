using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HOADONDTO
    {
        [Required(ErrorMessage = "Số hóa đơn không được để trống!")]
        [StringLength(10, ErrorMessage = "Số hóa đơn quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string SoHoaDon { get; set; }

        [Required(ErrorMessage = "Ngày lập hóa đơn không được để trống!")]
        [DataType(DataType.Date, ErrorMessage = "Ngày lập hóa đơn không đúng định dạng (ngày/tháng/năm).")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public System.DateTime NgayLap { get; set; }

        [Required(ErrorMessage = "Số tiền đóng không được để trống!")]
        public decimal SoTien { get; set; }


        public string MaSo { get; set; }
        public string MaPhong { get; set; }
    }
}
