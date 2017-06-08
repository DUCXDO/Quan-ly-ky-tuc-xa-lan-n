using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TAISANDTO
    {
        [Required(ErrorMessage = "Mã tài sản không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã tài sản quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaTS { get; set; }

        [Required(ErrorMessage = "Tên tài sản không được để trống!")]
        [StringLength(250, ErrorMessage = "Tên tài sản quá dài, chỉ nhập tối đa 250 kí tự.")]
        public string TenTS { get; set; }

        [StringLength(250, ErrorMessage = "Tình trạng tài sản quá dài, chỉ nhập tối đa 250 kí tự.")]
        public string TinhTrang { get; set; }

        [Required(ErrorMessage = "Số lượng của tài sản không được để trống!")]
        public int SoLuong { get; set; }
    }
}
