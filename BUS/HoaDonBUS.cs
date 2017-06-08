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
    public class HoaDonBUS
    {
        private readonly IHoaDonDAO _hd;
        private readonly ISoGhiDienNuocDAO _sg;
        private readonly IPhongDAO _p;
        private readonly ISinhVienDAO _sv;
        public HoaDonBUS(HoaDonDAO hd, SoGhiDienNuocDAO sg, PhongDAO p)
        {
            _hd = hd;
            _sg = sg;
            _p = p;
        }
        //Hàm kiểm tra hợp lệ
        public String kiemTraHoaDon(HOADONDTO hd)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(hd, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(hd, validationContext, validationResults);
            // Nếu hợp lệ
            if (isValid == true)
            {
                return null;
            }
            // Nếu không hợp lệ
            else
            {
                // Những lỗi không hợp lệ
                String result = String.Empty;
                foreach (var r in validationResults)
                {
                    result += r.ErrorMessage;
                }
                // Trả về những giá trị không hợp lệ
                return result;
            }
        }
        //Hàm thêm hóa đơn
        public String ThemHD(HOADONDTO hd)
        {
            // Kiểm tra mã Hóa đơn đã tồn tại chưa ?
            HOADON kiemTraHDTonTai = _hd.TimHDTheoMaHD(hd.SoHoaDon);
            // Nếu mã chưa tồn tại
            if (kiemTraHDTonTai == null)
            {
                // Kiểm tra Hóa đơn có hợp lệ không ?
                String check = kiemTraHoaDon(hd);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm Hóa đơn
                    HOADON result = _hd.ThemHD(hd);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm hóa đơn, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Số hóa đơn đã tồn tại!";
            }
        }
        public String XoaHD(String maHD)
        {
            // Kiểm tra hóa đơn có tồn tại không ?
            HOADON check = _hd.TimHDTheoMaHD(maHD);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa hóa đơn
                HOADON result = _hd.XoaHD(check.SoHoaDon);
                // Kiểm tra kết quả của hàm xóa hóa đơn
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa hóa đơn, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<HOADON> TimTatCaHD()
        {
            return _hd.TimTatCaHD();
        }
        public SINHVIEN TimHDTheoSinhVien(String maSV)
        {
            SINHVIEN sinhvien = _sv.TimSVTheoMaSV(maSV);
            return sinhvien;            
        }
        //Tìm hóa đơn theo các điều kiện! 
        public IEnumerable<HOADON> TimHD(HOADONDTO hd)
        {
            // Hàm tìm hóa đơn theo các điều kiện
            IEnumerable<HOADON> result = _hd.TimHD(hd);
            return result;
        }
    }
}
