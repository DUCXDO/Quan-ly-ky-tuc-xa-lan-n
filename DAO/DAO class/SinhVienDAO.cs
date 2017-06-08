using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public interface ISinhVienDAO
    {
        SINHVIEN ThemSV(SINHVIENDTO sv);
        SINHVIEN XoaSV(String maSV);
        SINHVIEN SuaSV(SINHVIENDTO sv);
        IEnumerable<SINHVIEN> TimSV(SINHVIENDTO sv);
        IEnumerable<SINHVIEN> TimTatCaSV();
        SINHVIEN TimSVTheoMaSV(String maSV);
    }

    public class SinhVienDAO : ISinhVienDAO
    {
        public SINHVIEN ThemSV(SINHVIENDTO sv)
        {
            try
            {
                // khai báo và khởi tạo đối tượng kết nối với database
                KTXEntities KTXe = new KTXEntities();

                SINHVIEN add = new SINHVIEN();
                add.DiaChi = sv.DiaChi;
                add.MaSV = sv.MaSV;
                add.NgaySinh = sv.NgaySinh;
                add.SoCMND = sv.SoCMND;
                add.SoDT = sv.SoDT;
                add.TenSV = sv.TenSV;
                //Thêm mới 
                SINHVIEN result = KTXe.SINHVIENs.Add(add);
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

        public SINHVIEN XoaSV(String maSV)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                SINHVIEN delete = KTXe.SINHVIENs.SingleOrDefault(x => x.MaSV == maSV);
                SINHVIEN result = KTXe.SINHVIENs.Remove(delete);
                KTXe.SaveChanges();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SINHVIEN SuaSV(SINHVIENDTO sv)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                SINHVIEN edit = KTXe.SINHVIENs.SingleOrDefault(x => x.MaSV == sv.MaSV);
                edit.NgaySinh = sv.NgaySinh;
                edit.SoCMND = sv.SoCMND;
                edit.SoDT = sv.SoDT;
                edit.TenSV = sv.TenSV;
                edit.DiaChi = sv.DiaChi;
                int result = KTXe.SaveChanges();
                if (result == 1)
                {
                    return edit;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public IEnumerable<SINHVIEN> TimTatCaSV()
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<SINHVIEN> result = KTXe.SINHVIENs.AsEnumerable();
            return result;

        }

        public SINHVIEN TimSVTheoMaSV(String maSV)
        {

            KTXEntities KTXe = new KTXEntities();
            SINHVIEN result = KTXe.SINHVIENs.SingleOrDefault(x => x.MaSV == maSV);
            return result;

        }

        public IEnumerable<SINHVIEN> TimSV(SINHVIENDTO sv)
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<SINHVIEN> result = KTXe.SINHVIENs.AsQueryable().Where(x =>
            (sv.MaSV == "" || x.MaSV == sv.MaSV) &&
            (sv.TenSV == "" || x.TenSV.Contains(sv.TenSV))&&
            (sv.SoCMND == 0) || (x.SoCMND == sv.SoCMND) &&
            (sv.SoDT == "") || (x.SoDT == sv.SoDT));
            return result;

        }
    }
}
