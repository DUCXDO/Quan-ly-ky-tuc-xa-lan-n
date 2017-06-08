using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAO;
using System.ComponentModel.DataAnnotations;

namespace BUS
{
    public class HopDongBUS
    {
        private readonly IHopDongDAO _hd;
        private readonly IPhongDAO _p;

        public HopDongBUS(HopDongDAO hd, PhongDAO p)
        {
            _hd = hd;
            _p = p;
        }

        //Hàm check cái thành phần hợp lệ
        public String kiemTraHopDong(HOPDONGDTO hd)
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
        //Hàm thêm hợp đồng
        public String ThemHD(HOPDONGDTO hd)
        {
            // Kiểm tra mã sinh viên đã tồn tại chưa ?
            HOPDONG kiemTraHDTonTai = _hd.TimHDTheoSoHD(hd.SoHD);
            // Nếu mã chưa tồn tại
            if (kiemTraHDTonTai == null)
            {
                // Kiểm tra sinh viên có hợp lệ không ?
                String check = kiemTraHopDong(hd);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm hợp đồng
                    HOPDONG result = _hd.ThemHD(hd);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm hợp đồng, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Mã hợp đồng đã tồn tại!";
            }
        }
        public String XoaHD(String soHD)
        {
            // Kiểm tra hợp đồng có tồn tại không ?
            HOPDONG check = _hd.TimHDTheoSoHD(soHD);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa hợp đồng
                HOPDONG result = _hd.XoaHD(check.SoHD);
                // Kiểm tra kết quả của hàm xóa sinh viên
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa hợp đồng, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<HOPDONG> TimTatCaHD()
        {
            return _hd.TimTatCaHD();
        }
        // Hàm tìm hợp đồng theo các điều kiện
        public IEnumerable<HOPDONG> TimHD(HOPDONGDTO hd)
        {
            // Hàm tìm hợp đồng theo các điều kiện
            IEnumerable<HOPDONG> result = _hd.TimHD(hd);
            return result;
        }

        public IEnumerable<HOPDONG> TimHDTheoPhong(String maP)
        {
            // Hàm tìm hợp đồng theo phòng
            IEnumerable<HOPDONG> result = _hd.TimHDTheoP(maP);
            return result;
        }
    }
}
