using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using DTO;
using System.ComponentModel.DataAnnotations;

namespace BUS
{
    public class SinhVienBUS
    {
        private readonly ISinhVienDAO _sv;
        private readonly IPhongDAO _p;
        private readonly IHopDongDAO _hd;

        public SinhVienBUS(SinhVienDAO sv, PhongDAO p, HopDongDAO hd)
        {
            _hd = hd;
            _sv = sv;
            _p = p;
        }
        
        //Hàm check cái thành phần hợp lệ
        public String kiemTraSinhVien(SINHVIENDTO sv)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(sv, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(sv, validationContext, validationResults);
            // Nếu hợp lệ
            if(isValid == true)
            {
                return null;
            }
            // Nếu không hợp lệ
            else
            {
                // Những lỗi không hợp lệ
                String result = String.Empty;
                foreach(var r in validationResults)
                {
                    result += r.ErrorMessage;
                }
                // Trả về những giá trị không hợp lệ
                return result;
            }
        }

        //Hàm thêm sinh viên
        public String ThemSV(SINHVIENDTO sv)
        {
            // Kiểm tra mã sinh viên đã tồn tại chưa ?
            SINHVIEN kiemTraSVTonTai = _sv.TimSVTheoMaSV(sv.MaSV);
            // Nếu mã chưa tồn tại
            if (kiemTraSVTonTai == null)
            {
                // Kiểm tra sinh viên có hợp lệ không ?
                String check = kiemTraSinhVien(sv);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm sinh viên
                    SINHVIEN result = _sv.ThemSV(sv);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm sinh viên, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Mã sinh viên đã tồn tại!";
            }
        }

        public String SuaSV(SINHVIENDTO sv)
        {
            // Kiểm tra thông tin sửa sinh viên có hợp lệ không
            String check = kiemTraSinhVien(sv);
            // Không hợp lệ
            if (check != null)
            {
                return check;
            }
            // Hợp lệ
            else
            {
                // Kiểm tra sinh viên muốn sửa có tồn tại không ????
                SINHVIEN checkSVTonTai = _sv.TimSVTheoMaSV(sv.MaSV);
                // Không tồn tại
                if (checkSVTonTai == null)
                {
                    return "Không tìm thấy thông tin cần sửa, xin vui lòng thử lại!";
                }
                // Tồn tại
                else
                {
                    // Sửa sinh viên
                    SINHVIEN result = _sv.SuaSV(sv);
                    // Kiểm tra kết quả của hàm sửa
                    if (result == null)
                    {
                        return "Đã xảy ra lỗi trong quá trình sửa thông tin, xin vui lòng thử lại!";
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public String XoaSV(String maSV)
        {
            // Kiểm tra sinh viên có tồn tại không ?
            SINHVIEN check = _sv.TimSVTheoMaSV(maSV);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa sinh viên
                SINHVIEN result = _sv.XoaSV(check.MaSV);
                // Kiểm tra kết quả của hàm xóa sinh viên
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa sinh viên, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<SINHVIEN> TimSV(SINHVIENDTO sv)
        {
            // Hàm tìm sinh viên theo các điều kiện
            IEnumerable<SINHVIEN> result = _sv.TimSV(sv);
            return result;
        }

        public IEnumerable<SINHVIEN> TimSVTheoPhong(String maP)
        {
            IEnumerable<HOPDONG> hopDong = _hd.TimHDTheoP(maP);
            List<SINHVIEN> result = new List<SINHVIEN>();
            foreach (var item in hopDong)
            {
                result.Add(_sv.TimSVTheoMaSV(item.MaSV));
            }
            return result.AsEnumerable();
            
        }

        public IEnumerable<SINHVIEN> TimTatCaSV()
        {
            return _sv.TimTatCaSV();
        }

        public IEnumerable<PHONG> DSTatCaPhong()
        {
            return _p.TimTatCaP();
        }

        public SINHVIENDTO chuyenDoiSVThanhSVDTO(SINHVIEN sv)
        {
            SINHVIENDTO result = new SINHVIENDTO();
            result.DiaChi = sv.DiaChi;
            result.MaSV = sv.MaSV;
            result.NgaySinh = sv.NgaySinh;
            result.SoCMND = sv.SoCMND;
            result.SoDT = sv.SoDT;
            result.TenSV = sv.TenSV;
            return result;
        }

        public SINHVIEN chuyenDoiSVDTOThanhSV(SINHVIENDTO sv)
        {
            SINHVIEN result = new SINHVIEN();
            result.DiaChi = sv.DiaChi;
            result.MaSV = sv.MaSV;
            result.NgaySinh = sv.NgaySinh;
            result.SoCMND = sv.SoCMND;
            result.SoDT = sv.SoDT;
            result.TenSV = sv.TenSV;
            return result;
        }
    }
}
