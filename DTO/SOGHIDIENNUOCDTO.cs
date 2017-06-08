using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SOGHIDIENNUOCDTO
    {
        [Required(ErrorMessage = "Mã sổ ghi không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã sổ ghi quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaSo { get; set; }

        [Required(ErrorMessage = "Tên sổ ghi không được để trống!")]
        [StringLength(10, ErrorMessage = "Tên sổ ghi quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string TenSo { get; set; }

        [Required(ErrorMessage = "Năm ghi sổ không được để trống!")]
        public int Nam { get; set; }
    }
}
