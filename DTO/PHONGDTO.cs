using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PHONGDTO
    {
        [Required(ErrorMessage = "Mã phòng không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã phòng quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaPhong { get; set; }

        [StringLength(250, ErrorMessage = "Vị trí phòng quá dài, chỉ nhập tối đa 250 kí tự.")]
        public string ViTriP { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống!")]
        [StringLength(250, ErrorMessage = "Tên phòng quá dài, chỉ nhập tối đa 250 kí tự.")]
        public string TenP { get; set; }

        public int SoNguoiO { get; set; }

        [Required(ErrorMessage = "Số người ở tối đa không được để trống!")]
        public int SoNguoiTD { get; set; }
    }
}
