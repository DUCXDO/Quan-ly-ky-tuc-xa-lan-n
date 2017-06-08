using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HOPDONGDTO
    {
        [Required(ErrorMessage = "Số hợp đồng không được để trống!")]
        [StringLength(10, ErrorMessage = "Số hợp đồng quá dài, chỉ nhập tối đa 10 kí tự.")]
        [Key]
        public string SoHD { get; set; }

        [Required(ErrorMessage = "Ngày lập hợp đồng không được để trống!")]
        [DataType(DataType.Date, ErrorMessage = "Ngày lập hợp đồng không đúng định dạng (ngày/tháng/năm).")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public System.DateTime NgayLap { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã sinh viên quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaSV { get; set; }

        [Required(ErrorMessage = "Mã phòng không được để trống!")]
        [StringLength(10, ErrorMessage = "Số phòng quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaPhong { get; set; }

        public Nullable<int> ThoiGianO { get; set; }
    }
}
