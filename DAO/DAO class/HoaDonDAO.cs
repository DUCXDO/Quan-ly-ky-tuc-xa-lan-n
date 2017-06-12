using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAO
{
    public interface IHoaDonDAO
    {
        HOADON ThemHD(HOADONDTO hd);
        HOADON XoaHD(String MaHD);
        IEnumerable<HOADON> TimTatCaHD();
        HOADON TimHDTheoSoHD(String MaHD);
        IEnumerable<HOADON> TimHD(HOADONDTO hd);
        HOADON TimHDTheoMaPGDN(String maPGDN);
    }
    public class HoaDonDAO : IHoaDonDAO
    {
        public HOADON ThemHD(HOADONDTO hd)
        {
            try
            {
                //Khai báo và khởi tạo đối tượng kết nối database
                KTXEntities KTXe = new KTXEntities();
                HOADON addHD = new HOADON();
                addHD.MaPhieuGhiDienNuoc = hd.MaPhieuGhiDienNuoc;
                addHD.NgayLap = hd.NgayLap;
                addHD.SoHoaDon = hd.SoHoaDon;
                addHD.SoTien = hd.SoTien;

                //Thêm SG mới
                HOADON result = KTXe.HOADONs.Add(addHD);
                //Lưu thay đổi
                KTXe.SaveChanges();
                //Trả về đối tượng mới thêm
                return result;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public HOADON XoaHD(String MaHD)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                HOADON delHD = KTXe.HOADONs.SingleOrDefault(x => x.SoHoaDon == MaHD);
                HOADON result = KTXe.HOADONs.Remove(delHD);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public IEnumerable<HOADON> TimTatCaHD()
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<HOADON> result = KTXe.HOADONs.AsEnumerable();
            return result;
        }
        public HOADON TimHDTheoSoHD(String MaHD)
        {
            KTXEntities KTXe = new KTXEntities();
            HOADON findHD = KTXe.HOADONs.SingleOrDefault(x => x.SoHoaDon == MaHD);
            return findHD;
        }
        public IEnumerable<HOADON> TimHD(HOADONDTO hd)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<HOADON> findHD = KTXe.HOADONs.AsQueryable().Where(x => (hd.MaPhieuGhiDienNuoc == "" || x.MaPhieuGhiDienNuoc == hd.MaPhieuGhiDienNuoc) && (hd.SoHoaDon == "" || x.SoHoaDon == hd.SoHoaDon) && (hd.SoTien == 0 || x.SoTien == hd.SoTien));
            return findHD;
        }

        public HOADON TimHDTheoMaPGDN(String maPGDN)
        {
            KTXEntities KTXe = new KTXEntities();
            HOADON findHD = KTXe.HOADONs.SingleOrDefault(x => x.MaPhieuGhiDienNuoc == maPGDN);
            return findHD;
        }
    }
}

