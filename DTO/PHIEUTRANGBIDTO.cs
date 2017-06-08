using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PHIEUTRANGBIDTO
    {
        [Required(ErrorMessage = "Mã phiếu trang bị không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã phiếu trang bị quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaTS { get; set; }

        [Required(ErrorMessage = "Mã phòng không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã phòng quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaPhong { get; set; }

        [Required(ErrorMessage = "Số lượng trang bị cho phòng không được để trống!")]
        public int SoLuong { get; set; }
    }
}
