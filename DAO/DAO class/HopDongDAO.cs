using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public interface IHopDongDAO
    {
        HOPDONG ThemHD(HOPDONGDTO hd);
        HOPDONG XoaHD(String soHD);
        IEnumerable<HOPDONG> TimTatCaHD();
        IEnumerable<HOPDONG> TimHD(HOPDONGDTO hd);
        HOPDONG TimHDTheoSoHD(String soHD);
        IEnumerable<HOPDONG> TimHDTheoP(String maP);
    }

    public class HopDongDAO : IHopDongDAO
    {
        public HOPDONG ThemHD(HOPDONGDTO hd)
        {
            try
            {
                // khai báo và khởi tạo đối tượng kết nối với database
                KTXEntities KTXe = new KTXEntities();
                //Thêm mới 
                HOPDONG add = new HOPDONG();
                add.MaPhong = hd.MaPhong;
                add.MaSV = hd.MaSV;
                add.NgayLap = hd.NgayLap;
                add.SoHD = hd.SoHD;
                add.ThoiGianO = hd.ThoiGianO;
                HOPDONG result = KTXe.HOPDONGs.Add(add);
                //Lưu thay đổi
                KTXe.SaveChanges();
                //trả về đối tượng mới thêm để xác định kết quả
                return result;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public HOPDONG XoaHD(String soHD)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                HOPDONG delete = KTXe.HOPDONGs.SingleOrDefault(x => x.SoHD == soHD);
                HOPDONG result = KTXe.HOPDONGs.Remove(delete);
                return result;
            }catch(Exception e)
            {
                return null;
            }
}


        public IEnumerable<HOPDONG> TimTatCaHD()
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<HOPDONG> result = KTXe.HOPDONGs.AsEnumerable();
            return result;

        }


        public IEnumerable<HOPDONG> TimHD(HOPDONGDTO hd)
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<HOPDONG> result = KTXe.HOPDONGs.AsQueryable().Where(x =>
            (hd.MaSV == "" || x.MaSV == hd.MaSV) &&
            (hd.SoHD == "" || x.SoHD == hd.SoHD) &&
            (hd.MaPhong == "") || (x.MaPhong == hd.MaPhong) &&
            (hd.NgayLap == null) || (x.NgayLap == hd.NgayLap));
            return result;

        }

        public HOPDONG TimHDTheoSoHD(String soHD)
        {
            KTXEntities KTXe = new KTXEntities();
            HOPDONG result = KTXe.HOPDONGs.SingleOrDefault(x => x.SoHD == soHD);
            return result;
        }

        public IEnumerable<HOPDONG> TimHDTheoP(String maP)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<HOPDONG> result = KTXe.HOPDONGs.AsQueryable().Where(x => x.MaPhong == maP);
            return result;
        }
    }
}
