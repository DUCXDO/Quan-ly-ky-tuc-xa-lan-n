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
    public class PhieuTrangBiBUS
    {
        private readonly ITaiSanDAO _ts;
        private readonly IPhongDAO _p;
        private readonly IPhieuTrangBiDAO _ptb;

        public PhieuTrangBiBUS(TaiSanDAO ts, PhongDAO p, PhieuTrangBiDAO ptb)
        {
            _ts = ts;
            _ptb = ptb;
            _p = p;
        }

        //Hàm check cái thành phần hợp lệ
        public String kiemTraPhieuTrangBi(PHIEUTRANGBIDTO ptb)
        {
            // Nội dung kiểm tra
            var validationContext = new ValidationContext(ptb, null, null);
            // Danh sách chứa kết quả kiểm tra
            var validationResults = new List<ValidationResult>();

            // Biến hợp lệ hay không
            var isValid = Validator.TryValidateObject(ptb, validationContext, validationResults);
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

        //Hàm thêm phiếu trang bị
        public String ThemPTB(PHIEUTRANGBIDTO ptb)
        {
            // Kiểm tra phiếu trang bị đã tồn tại chưa ?
            PHIEUTRANGBI kiemTraPTBTonTai = _ptb.TimPhieuTBTheoCaHaiMa(ptb.MaTS,ptb.MaPhong);
            // Nếu chưa tồn tại
            if (kiemTraPTBTonTai == null)
            {
                // Kiểm tra phiếu trang bị có hợp lệ không ?
                String check = kiemTraPhieuTrangBi(ptb);
                // Không hợp lệ
                if (check != null)
                {
                    return check;
                }
                // Hợp lệ
                else
                {
                    // Hàm thêm phiếu trang bị
                    PHIEUTRANGBI result = _ptb.ThemPhieuTB(ptb);
                    // Kiếm tra kết quả của hàm thêm
                    if (result != null)
                    {
                        return null;
                    }
                    else
                    {
                        return "Đã xảy ra lỗi trong quá trình thêm phiếu trang bị, xin vui lòng thử lại!";
                    }
                }
            }
            // Nếu phiếu trang bị đã tồn tại
            else
            {
                return "Phiếu trang bị đã tồn tại!";
            }
        }


        public String SuaPTB(PHIEUTRANGBIDTO ptb)
        {
            // Kiểm tra thông tin sửa phiếu trang bị có hợp lệ không
            String check = kiemTraPhieuTrangBi(ptb);
            // Không hợp lệ
            if (check != null)
            {
                return check;
            }
            // Hợp lệ
            else
            {
                // Kiểm tra phiếu trang bị muốn sửa có tồn tại không ????
                PHIEUTRANGBI checkPTBTonTai = _ptb.TimPhieuTBTheoCaHaiMa(ptb.MaTS, ptb.MaPhong);
                // Không tồn tại
                if (checkPTBTonTai == null)
                {
                    return "Không tìm thấy thông tin cần sửa, xin vui lòng thử lại!";
                }
                // Tồn tại
                else
                {
                    // Sửa phiếu trang bị
                    PHIEUTRANGBI result = _ptb.SuaPhieuTB(ptb);
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

        public String XoaPTB(String maTS,String maP)
        {
            // Kiểm tra phiếu trang bị có tồn tại không ?
            PHIEUTRANGBI check = _ptb.TimPhieuTBTheoCaHaiMa(maTS, maP);
            // Không tồn tại
            if (check == null)
            {
                return "Không tìm thấy dữ liệu cần xóa, xin vui lòng thử lại!";
            }
            // Tồn tại
            else
            {
                // Hàm xóa phiếu trang bị
                PHIEUTRANGBI result = _ptb.XoaPhieuTB(check.MaTS, check.MaPhong);
                // Kiểm tra kết quả của hàm xóa phiếu trang bị
                if (result == null)
                {
                    return "Đã xảy ra lỗi trong quá trình xóa phiếu trang bị, xin vui lòng thử lại!";
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<PHIEUTRANGBI> TimPTB(PHIEUTRANGBIDTO ptb)
        {
            // Hàm tìm phiếu trang bị theo các điều kiện
            IEnumerable<PHIEUTRANGBI> result = _ptb.TimPhieuTB(ptb);
            return result;
        }

        public IEnumerable<PHIEUTRANGBI> TimPTBTheoPhong(String maP)
        {
            IEnumerable<PHIEUTRANGBI> result = _ptb.TimPhieuTBTheoMaPhong(maP);
            return result;
        }

        public IEnumerable<PHIEUTRANGBI> TimPTBTheoTS(String maTS)
        {
            IEnumerable<PHIEUTRANGBI> result = _ptb.TimPhieuTBTheoMaTaiSan(maTS);
            return result;
        }

        public IEnumerable<PHIEUTRANGBI> TimTatCaSV()
        {
            return _ptb.TimTatCaPhieuTrangBi();
        }

        public IEnumerable<PHONG> DSTatCaPhong()
        {
            return _p.TimTatCaP();
        }

        public IEnumerable<TAISAN> DSTatCaTaiSan()
        {
            return _ts.TimTatCaTS();
        }
    }
}
