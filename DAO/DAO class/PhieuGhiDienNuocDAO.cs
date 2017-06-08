using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAO
{
    public interface IPhieuGhiDienNuocDAO
    {
        PHIEUGHIDIENNUOC ThemPGDN(PHIEUGHIDIENNUOCDTO pg);
        PHIEUGHIDIENNUOC SuaPGDN(PHIEUGHIDIENNUOCDTO pg);
        PHIEUGHIDIENNUOC XoaPGDN(String maPG);
        IEnumerable<PHIEUGHIDIENNUOC> TimTatCaPGDN();
        PHIEUGHIDIENNUOC TimPGDNTheoMaPGDN(String maPG);
        PHIEUGHIDIENNUOC TimPGDNTheoMaP(String maP);
        PHIEUGHIDIENNUOC TimPGDNTheoMaS(String maS);
        IEnumerable<PHIEUGHIDIENNUOC> TimPGDN(PHIEUGHIDIENNUOCDTO pg);
        IEnumerable<PHIEUGHIDIENNUOC> TimPGDNKhongTheoLoaiPhieuGhi(PHIEUGHIDIENNUOCDTO pg);
    }
    public class PhieuGhiDienNuocDAO : IPhieuGhiDienNuocDAO
    {
        public PHIEUGHIDIENNUOC ThemPGDN(PHIEUGHIDIENNUOCDTO pg)
        {
            try
            {
                //Khai báo và khởi tạo đối tượng kết nối database
                KTXEntities KTXe = new KTXEntities();
                PHIEUGHIDIENNUOC addPG = new PHIEUGHIDIENNUOC();
                addPG.LoaiPhieuGhi = pg.LoaiPhieuGhi;
                addPG.MaPhong = pg.MaPhong;
                addPG.MaSo = pg.MaSo;
                addPG.NgayGhi = pg.NgayGhi;
                addPG.SoDienNuoc = pg.SoDienNuoc;
                //Thêm SG mới
                PHIEUGHIDIENNUOC result = KTXe.PHIEUGHIDIENNUOCs.Add(addPG);
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
        public PHIEUGHIDIENNUOC SuaPGDN(PHIEUGHIDIENNUOCDTO pg)
        {
            try
            {
                //Khai báo kết nối data
                KTXEntities KTXe = new KTXEntities();
                PHIEUGHIDIENNUOC editPG = KTXe.PHIEUGHIDIENNUOCs.SingleOrDefault(x => x.MaPhieuGhiDienNuoc == pg.MaPhieuGhiDienNuoc);
                editPG.MaPhieuGhiDienNuoc = pg.MaPhieuGhiDienNuoc;
                editPG.LoaiPhieuGhi = pg.LoaiPhieuGhi;
                editPG.MaPhong = pg.MaPhong;
                editPG.MaSo = pg.MaSo;
                editPG.NgayGhi = pg.NgayGhi;
                editPG.SoDienNuoc = pg.SoDienNuoc;
                int result = KTXe.SaveChanges();
                if (result == 1)
                {
                    return editPG;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public PHIEUGHIDIENNUOC XoaPGDN(String maPG)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHIEUGHIDIENNUOC delPG = KTXe.PHIEUGHIDIENNUOCs.SingleOrDefault(x => x.MaPhieuGhiDienNuoc == maPG);
                PHIEUGHIDIENNUOC result = KTXe.PHIEUGHIDIENNUOCs.Remove(delPG);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<PHIEUGHIDIENNUOC> TimTatCaPGDN()
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUGHIDIENNUOC> result = KTXe.PHIEUGHIDIENNUOCs.AsEnumerable();
            return result;
        }
        public PHIEUGHIDIENNUOC TimPGDNTheoMaPGDN(String maPG)
        {
            KTXEntities KTXe = new KTXEntities();
            PHIEUGHIDIENNUOC findPG = KTXe.PHIEUGHIDIENNUOCs.SingleOrDefault(x => x.MaPhieuGhiDienNuoc == maPG);
            return findPG;
        }
        public PHIEUGHIDIENNUOC TimPGDNTheoMaP(String maP)
        {
            KTXEntities KTXe = new KTXEntities();
            PHIEUGHIDIENNUOC findPG = KTXe.PHIEUGHIDIENNUOCs.SingleOrDefault(x => x.MaPhong == maP);
            return findPG;
        }
        public PHIEUGHIDIENNUOC TimPGDNTheoMaS(String maS)
        {
            KTXEntities KTXe = new KTXEntities();
            PHIEUGHIDIENNUOC findPG = KTXe.PHIEUGHIDIENNUOCs.SingleOrDefault(x => x.MaSo == maS);
            return findPG;
        }
        public IEnumerable<PHIEUGHIDIENNUOC> TimPGDN(PHIEUGHIDIENNUOCDTO pg)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUGHIDIENNUOC> findPG = KTXe.PHIEUGHIDIENNUOCs.AsQueryable().Where(x => (pg.MaPhieuGhiDienNuoc == "" || x.MaPhieuGhiDienNuoc == pg.MaPhieuGhiDienNuoc) && (x.LoaiPhieuGhi == pg.LoaiPhieuGhi) && (pg.MaPhong == "" || x.MaPhong == pg.MaPhong) && (pg.MaSo == "" || x.MaSo == pg.MaSo) && (pg.NgayGhi == null || x.NgayGhi == pg.NgayGhi) && (pg.SoDienNuoc == 0 || x.SoDienNuoc == pg.SoDienNuoc));
            return findPG;
        }

        public IEnumerable<PHIEUGHIDIENNUOC> TimPGDNKhongTheoLoaiPhieuGhi(PHIEUGHIDIENNUOCDTO pg)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUGHIDIENNUOC> findPG = KTXe.PHIEUGHIDIENNUOCs.AsQueryable().Where(x => (pg.MaPhieuGhiDienNuoc == "" || x.MaPhieuGhiDienNuoc == pg.MaPhieuGhiDienNuoc) && (pg.MaPhong == "" || x.MaPhong == pg.MaPhong) && (pg.MaSo == "" || x.MaSo == pg.MaSo) && (pg.NgayGhi == null || x.NgayGhi == pg.NgayGhi) && (pg.SoDienNuoc == 0 || x.SoDienNuoc == pg.SoDienNuoc));
            return findPG;
        }
    }
}

