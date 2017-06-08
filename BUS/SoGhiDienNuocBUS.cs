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
    public class SoGhiDienNuocBUS
    {
        private readonly ISoGhiDienNuocDAO _sg;
        public SoGhiDienNuocBUS(SoGhiDienNuocDAO sg)
        {
            _sg = sg;
        }
        public String kiemTraSoGhiDienNuoc(SOGHIDIENNUOCDTO sg)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(sg, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(sg, validationContext, validationResults);
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
        public String ThemSoGhi(SOGHIDIENNUOCDTO sg)
        {
            // Kiểm tra sổ ghi đã tồn tại chưa ?
            SOGHIDIENNUOC kiemTraSGTonTai = _sg.TimSGDNTheoMaSGDN(sg.MaSo);
            // Nếu mã chưa tồn tại
            if (kiemTraSGTonTai == null)
            {
                // Kiểm tra sổ ghi có hợp lệ không ?
                String check = kiemTraSoGhiDienNuoc(sg);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm sổ ghi
                    SOGHIDIENNUOC result = _sg.ThemSGDN(sg);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm sổ ghi, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Mã sổ ghi đã tồn tại!";
            }
        }
        public String SuaSG(SOGHIDIENNUOCDTO sg)
        {
            // Kiểm tra thông tin sửa sổ ghi có hợp lệ không
            String check = kiemTraSoGhiDienNuoc(sg);
            // Không hợp lệ
            if (check != null)
            {
                return check;
            }
            // Hợp lệ
            else
            {
                // Kiểm tra sinh viên muốn sửa có tồn tại không ????
                SOGHIDIENNUOC checkSGTonTai = _sg.TimSGDNTheoMaSGDN(sg.MaSo);
                // Không tồn tại
                if (checkSGTonTai == null)
                {
                    return "Không tìm thấy thông tin cần sửa, xin vui lòng thử lại!";
                }
                // Tồn tại
                else
                {
                    // Sửa sổ ghi
                    SOGHIDIENNUOC result = _sg.SuaSGDN(sg);
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
        public String XoaSG(String maSG)
        {
            // Kiểm tra sổ ghi có tồn tại không ?
            SOGHIDIENNUOC check = _sg.TimSGDNTheoMaSGDN(maSG);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa sổ ghi
                SOGHIDIENNUOC result = _sg.XoaSGDN(maSG);
                // Kiểm tra kết quả của hàm xóa sổ ghi
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa sổ ghi, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<SOGHIDIENNUOC> TimSG(SOGHIDIENNUOCDTO sg)
        {
            // Hàm tìm sinh viên theo các điều kiện
            IEnumerable<SOGHIDIENNUOC> result = _sg.TimSGDN(sg);
            return result;
        }
        public IEnumerable<SOGHIDIENNUOC> TimTatCaSGDN()
        {
            return _sg.TimTatCaSGDN();
        }
    }
}
