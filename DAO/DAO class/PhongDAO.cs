using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public interface IPhongDAO
    {
        PHONG ThemP(PHONGDTO p);
        PHONG XoaP(String maP);
        PHONG SuaP(PHONGDTO p);
        IEnumerable<PHONG> TimTatCaP();
        PHONG TimPTheoMaP(String maP);
        IEnumerable<PHONG> TimP(PHONGDTO p);
    }

    public class PhongDAO : IPhongDAO
    {
        public PHONG ThemP(PHONGDTO p)
        {
            try
            {

                // khai báo và khởi tạo đối tượng kết nối với database
                KTXEntities KTXe = new KTXEntities();
                PHONG add = new PHONG();
                add.MaPhong = p.MaPhong;
                add.TenP = p.TenP;
                add.SoNguoiO = p.SoNguoiO;
                add.SoNguoiTD = p.SoNguoiTD;
                add.ViTriP = p.ViTriP;
                //Thêm mới
                PHONG result = KTXe.PHONGs.Add(add);
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

        public PHONG XoaP(String maP)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHONG delete = KTXe.PHONGs.SingleOrDefault(x => x.MaPhong == maP);
                PHONG result = KTXe.PHONGs.Remove(delete);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public PHONG SuaP(PHONGDTO p)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHONG edit = KTXe.PHONGs.SingleOrDefault(x => x.MaPhong == p.MaPhong);
                edit.SoNguoiTD = p.SoNguoiTD;
                edit.TenP = p.TenP;
                edit.ViTriP = p.ViTriP;
                int result = KTXe.SaveChanges();
                if (result == 1)
                { return edit; }
                else
                { return null; }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public IEnumerable<PHONG> TimTatCaP()
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHONG> result = KTXe.PHONGs.AsEnumerable();
            return result;
        }

        public PHONG TimPTheoMaP(String maP)
        {

            KTXEntities KTXe = new KTXEntities();
            PHONG result = KTXe.PHONGs.SingleOrDefault(x => x.MaPhong == maP);
            return result;

        }

        public IEnumerable<PHONG> TimP(PHONGDTO p)
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHONG> result = KTXe.PHONGs.AsQueryable().Where(x =>
            (p.MaPhong == "" || x.MaPhong == p.MaPhong) &&
            (p.TenP == "" || x.TenP == p.TenP));
            return result;

        }
    }
}
