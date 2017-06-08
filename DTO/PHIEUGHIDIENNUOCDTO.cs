using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PHIEUGHIDIENNUOCDTO
    {
        [Required(ErrorMessage = "Mã phiếu ghi điện nước không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã phiếu ghi quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaPhieuGhiDienNuoc { get; set; }

        [Required(ErrorMessage = "Mã sổ không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã sổ quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaSo { get; set; }

        [Required(ErrorMessage = "Mã phòng không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã phòng quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaPhong { get; set; }

        [Required(ErrorMessage = "Số điện, nước không được để trống!")]
        public int SoDienNuoc { get; set; }

        [Required(ErrorMessage = "Ngày ghi không được để trống!")]
        [DataType(DataType.Date, ErrorMessage = "Ngày ghi không đúng định dạng (ngày/tháng/năm).")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public System.DateTime NgayGhi { get; set; }

        public bool LoaiPhieuGhi { get; set; }

    }
}
