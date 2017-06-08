using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SINHVIENDTO
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống!")]
        [StringLength(10, ErrorMessage = "Mã sinh viên quá dài, chỉ nhập tối đa 10 kí tự.")]
        public string MaSV { get; set; }

        [Required(ErrorMessage = "Tên sinh viên không được để trống!")]
        [StringLength(250, ErrorMessage = "Tên sinh viên quá dài, chỉ nhập tối đa 250 kí tự.")]
        public string TenSV { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống!")]
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không đúng định dạng (ngày/tháng/năm).")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public System.DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số chứng minh nhân dân không được để trống!")]
        public int SoCMND { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Số điện thoại không đúng định dạng.")]
        public string SoDT { get; set; }

        [StringLength(250, ErrorMessage = "Địa chỉ quá dài, chỉ nhập tối đa 250 kí tự.")]
        public string DiaChi { get; set; }
    }
}
