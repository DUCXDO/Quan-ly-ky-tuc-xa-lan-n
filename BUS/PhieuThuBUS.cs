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
    public class PhieuThuBUS
    {
        private readonly IPhieuThuDAO _pt;
        private readonly IHopDongDAO _hd;

        public PhieuThuBUS(PhieuThuDAO pt, HopDongDAO hd)
        {
            _pt = pt;
            _hd = hd;
        }

        //Hàm check cái thành phần hợp lệ
        public String kiemTraPhieuThu(PHIEUTHUDTO pt)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(pt, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(pt, validationContext, validationResults);
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

        //Hàm thêm sinh viên
        public String ThemPT(PHIEUTHUDTO pt)
        {
            // Kiểm tra mã phiếu thu đã tồn tại chưa ?
            PHIEUTHU kiemTraPTTonTai = _pt.TimPTTheoSoPT(pt.SoPT);
            // Nếu mã chưa tồn tại
            if (kiemTraPTTonTai == null)
            {
                // Kiểm tra phiếu thu có hợp lệ không ?
                String check = kiemTraPhieuThu(pt);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm sinh viên
                    PHIEUTHU result = _pt.ThemPT(pt);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm phiếu thu, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Số phiếu thu đã tồn tại!";
            }
        }

        public String XoaPT(String maPT)
        {
            // Kiểm tra sinh viên có tồn tại không ?
            PHIEUTHU check = _pt.TimPTTheoSoPT(maPT);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa phiếu thu
                PHIEUTHU result = _pt.XoaPT(check.SoPT);
                // Kiểm tra kết quả của hàm xóa phiếu thu
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa phiếu thu, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<PHIEUTHU> TimPT(PHIEUTHUDTO pt)
        {
            // Hàm tìm sinh viên theo các điều kiện
            IEnumerable<PHIEUTHU> result = _pt.TimPT(pt);
            return result;
        }
    }
}
