using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public interface IPhieuThuDAO
    {
        PHIEUTHU ThemPT(PHIEUTHUDTO pt);
        PHIEUTHU XoaPT(String soPT);
        IEnumerable<PHIEUTHU> TimTatCaPT();
        IEnumerable<PHIEUTHU> TimPT(PHIEUTHUDTO pt);
        PHIEUTHU TimPTTheoSoPT(String soPT);
    }


    public class PhieuThuDAO : IPhieuThuDAO
    {
        public PHIEUTHU ThemPT(PHIEUTHUDTO pt)
        {
            try
            {
                // khai báo và khởi tạo đối tượng kết nối với database
                KTXEntities KTXe = new KTXEntities();
                //Thêm mới 
                PHIEUTHU add = new PHIEUTHU();
                add.NgayLap = pt.NgayLap;
                add.SoHD = pt.SoHD;
                add.SoPT = pt.SoPT;
                add.TienThu = pt.TienThu;
                PHIEUTHU result = KTXe.PHIEUTHUs.Add(add);
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

        public PHIEUTHU XoaPT(String  soPT)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHIEUTHU delete = KTXe.PHIEUTHUs.SingleOrDefault(x => x.SoPT == soPT);
                PHIEUTHU result = KTXe.PHIEUTHUs.Remove(delete);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<PHIEUTHU> TimTatCaPT()
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUTHU> result = KTXe.PHIEUTHUs.AsEnumerable();
            return result;

        }

        public IEnumerable<PHIEUTHU> TimPT(PHIEUTHUDTO pt)
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUTHU> result = KTXe.PHIEUTHUs.AsQueryable().Where(x =>
            (pt.SoHD == "" || x.SoHD == pt.SoHD) &&
            (pt.SoPT == "" || x.SoPT == pt.SoPT) &&
            (pt.NgayLap == null) || (x.NgayLap.Date == pt.NgayLap.Date));
            return result;

        }

        public PHIEUTHU TimPTTheoSoPT(String soPT)
        {

            KTXEntities KTXe = new KTXEntities();
            PHIEUTHU result = KTXe.PHIEUTHUs.SingleOrDefault(x => x.SoPT == soPT);
            return result;

        }
        public PHIEUTHU TimPTTheoSoHD(String soHD)
        {
            KTXEntities KTXe = new KTXEntities();
            PHIEUTHU findPT = KTXe.PHIEUTHUs.SingleOrDefault(x => x.SoHD == soHD);
            return findPT;
        }
    }
}

