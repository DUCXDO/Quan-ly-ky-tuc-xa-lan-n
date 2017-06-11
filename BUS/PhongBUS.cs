using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class PhongBUS
    {
        private readonly ISinhVienDAO _sv;
        private readonly IPhongDAO _p;

        public PhongBUS(SinhVienDAO sv, PhongDAO p)
        {
            _sv = sv;
            _p = p;
        }

        //Hàm check cái thành phần hợp lệ
        public String kiemTraPhong(PHONGDTO p)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(p, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(p, validationContext, validationResults);
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

        //Hàm thêm phòng
        public String ThemP(PHONGDTO p)
        {
            // Kiểm tra mã phòng đã tồn tại chưa ?
            PHONG kiemTraPhongTonTai = _p.TimPTheoMaP(p.MaPhong);
            // Nếu mã chưa tồn tại
            if (kiemTraPhongTonTai == null)
            {
                // Kiểm tra phòng có hợp lệ không ?
                String check = kiemTraPhong(p);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm phòng
                    PHONG result = _p.ThemP(p);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm phòng, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Mã phòng đã tồn tại!";
            }
        }


        public String SuaP(PHONGDTO p)
        {
            // Kiểm tra thông tin sửa phòng có hợp lệ không
            String check = kiemTraPhong(p);
            // Không hợp lệ
            if (check != null)
            {
                return check;
            }
            // Hợp lệ
            else
            {
                // Kiểm tra phòng muốn sửa có tồn tại không ????
                PHONG checkPTonTai = _p.TimPTheoMaP(p.MaPhong);
                // Không tồn tại
                if (checkPTonTai == null)
                {
                    return "Không tìm thấy thông tin cần sửa, xin vui lòng thử lại!";
                }
                // Tồn tại
                else
                {
                    // Sửa phòng
                    PHONG result = _p.SuaP(p);
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

        public String XoaP(String maP)
        {
            // Kiểm tra phòng có tồn tại không ?
            PHONG check = _p.TimPTheoMaP(maP);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa phòng
                PHONG result = _p.XoaP(check.MaPhong);
                // Kiểm tra kết quả của hàm xóa sinh viên
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa phòng, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<PHONG> TimP(PHONGDTO p)
        {
            // Hàm tìm phòng theo các điều kiện
            IEnumerable<PHONG> result = _p.TimP(p);
            return result;
        }
        //Tìm tất cả phòng
        public IEnumerable<PHONG> TimTatCaP()
        {
            IEnumerable<PHONG> result = _p.TimTatCaP();
            return result;
        }
    }
}
