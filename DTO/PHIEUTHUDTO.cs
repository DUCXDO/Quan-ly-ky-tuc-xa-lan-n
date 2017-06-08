using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PHIEUTHUDTO
    {
        [Required(ErrorMessage = "Số phiếu thu không được để trống!")]
        [StringLength(10, ErrorMessage = "Số phiếu thu quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string SoPT { get; set; }

        [Required(ErrorMessage = "Ngày lập phiếu thu không được để trống!")]
        [DataType(DataType.Date, ErrorMessage = "Ngày lập phiếu thu không đúng định dạng (ngày/tháng/năm).")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public System.DateTime NgayLap { get; set; }

        [Required(ErrorMessage = "Số hợp đồng không được để trống!")]
        [StringLength(10, ErrorMessage = "Số hợp đồng quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string SoHD { get; set; }

        [Required(ErrorMessage = "Tiền thu theo hợp đồng không được để trống!")]
        public decimal TienThu { get; set; }
    }
}
