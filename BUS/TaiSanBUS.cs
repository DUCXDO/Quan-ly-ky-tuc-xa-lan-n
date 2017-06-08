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
    public class TaiSanBUS
    {
        private readonly ITaiSanDAO _ts;
        public TaiSanBUS(TaiSanDAO ts)
        {
            _ts = ts;
        }
        public String kiemTraTaiSan(TAISANDTO ts)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(ts, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(ts, validationContext, validationResults);
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
        public String ThemTS(TAISANDTO ts)
        {
            // Kiểm tra mã sinh viên đã tồn tại chưa ?
            TAISAN kiemTraTSTonTai = _ts.TimTSTheoMaTS(ts.MaTS);
            // Nếu mã chưa tồn tại
            if (kiemTraTSTonTai == null)
            {
                // Kiểm tra tài sản có hợp lệ không ?
                String check = kiemTraTaiSan(ts);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm tài sản
                    TAISAN result = _ts.ThemTS(ts);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm tài sản, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Mã tài sản đã tồn tại!";
            }
        }
        public String SuaTS(TAISANDTO ts)
        {
            // Kiểm tra thông tin sửa sinh viên có hợp lệ không
            String check = kiemTraTaiSan(ts);
            // Không hợp lệ
            if (check != null)
            {
                return check;
            }
            // Hợp lệ
            else
            {
                // Kiểm tra tài sản muốn sửa có tồn tại không ????
                TAISAN checkTSTonTai = _ts.TimTSTheoMaTS(ts.MaTS);
                // Không tồn tại
                if (checkTSTonTai == null)
                {
                    return "Không tìm thấy thông tin cần sửa, xin vui lòng thử lại!";
                }
                // Tồn tại
                else
                {
                    // Sửa tài sản
                    TAISAN result = _ts.SuaTS(ts);
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
        public String XoaTS(String maTS)
        {
            // Kiểm tra sinh viên có tồn tại không ?
            TAISAN check = _ts.TimTSTheoMaTS(maTS);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa tài sản
                TAISAN result = _ts.XoaTS(maTS);
                // Kiểm tra kết quả của hàm xóa tài sản
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa tài sản, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }
        // Hàm tìm tài sản theo các điều kiện
        public IEnumerable<TAISAN> TimTS(TAISANDTO ts)
        {
            // tìm sinh viên theo các điều kiện
            IEnumerable<TAISAN> result = _ts.TimTS(ts);
            return result;
        }
        public IEnumerable<TAISAN> TimTatCaSV()
        {
            return _ts.TimTatCaTS();
        }
    }
}
