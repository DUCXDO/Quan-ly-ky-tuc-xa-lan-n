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
    public class PhieuGhiDienNuocBUS
    {
        private readonly IPhieuGhiDienNuocDAO _pg;
        private readonly IPhongDAO _p;
        private readonly ISoGhiDienNuocDAO _sg;

        public PhieuGhiDienNuocBUS(PhieuGhiDienNuocDAO pg, PhongDAO p, SoGhiDienNuocDAO sg)
        {
            _pg = pg;
            _p = p;
            _sg = sg;
        }

        //Hàm check các thành phần hợp lệ
        public String kiemTraPhieuGhi(PHIEUGHIDIENNUOCDTO pg)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(pg, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(pg, validationContext, validationResults);
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

        //Hàm thêm phiếu ghi
        public String ThemPG(PHIEUGHIDIENNUOCDTO pg)
        {
            // Kiểm tra số phiếu ghi đã tồn tại chưa ?
            PHIEUGHIDIENNUOC kiemTraPGTonTai = _pg.TimPGDNTheoMaPGDN(pg.MaPhieuGhiDienNuoc);
            // Nếu mã chưa tồn tại
            if (kiemTraPGTonTai == null)
            {
                // Kiểm tra phiếu ghi có hợp lệ không ?
                String check = kiemTraPhieuGhi(pg);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm phiếu ghi
                    PHIEUGHIDIENNUOC result = _pg.ThemPGDN(pg);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm phiếu ghi điện nước, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu mã đã tồn tại
            else
            {
                return "Mã phiếu ghi đã tồn tại!";
            }
        }


        public String SuaPG(PHIEUGHIDIENNUOCDTO pg)
        {
            // Kiểm tra thông tin sửa phiếu ghi có hợp lệ không
            String check = kiemTraPhieuGhi(pg);
            // Không hợp lệ
            if (check != null)
            {
                return check;
            }
            // Hợp lệ
            else
            {
                // Kiểm tra phiếu ghi muốn sửa có tồn tại không ????
                PHIEUGHIDIENNUOC checkPGTonTai = _pg.TimPGDNTheoMaPGDN(pg.MaPhieuGhiDienNuoc);
                // Không tồn tại
                if (checkPGTonTai == null)
                {
                    return "Không tìm thấy thông tin cần sửa, xin vui lòng thử lại!";
                }
                // Tồn tại
                else
                {
                    // Sửa phiếu ghi
                     PHIEUGHIDIENNUOC result = _pg.SuaPGDN(pg);
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

        public String XoaPG(String maPG)
        {
            // Kiểm tra phiếu ghi có tồn tại không ?
            PHIEUGHIDIENNUOC check = _pg.TimPGDNTheoMaPGDN(maPG);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa phiếu ghi
                PHIEUGHIDIENNUOC result = _pg.XoaPGDN(check.MaPhieuGhiDienNuoc);
                // Kiểm tra kết quả của hàm xóa phiếu ghi
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa phiếu ghi điện nước, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<PHIEUGHIDIENNUOC> TimPG(PHIEUGHIDIENNUOCDTO pg)
        {
            // Hàm tìm phiếu ghi theo các điều kiện
            IEnumerable<PHIEUGHIDIENNUOC> result = _pg.TimPGDNKhongTheoLoaiPhieuGhi(pg);
            return result;
        }

        public IEnumerable<PHIEUGHIDIENNUOC> TimPGTheoLoaiPhieuGhi(PHIEUGHIDIENNUOCDTO pg)
        {
            // Hàm tìm phiếu ghi theo các điều kiện có cả loại phiếu ghi
            IEnumerable<PHIEUGHIDIENNUOC> result = _pg.TimPGDN(pg);
            return result;
        }
    }
}
