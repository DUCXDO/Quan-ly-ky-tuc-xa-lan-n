using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAO
{
    public interface ISoGhiDienNuocDAO
    {
        SOGHIDIENNUOC ThemSGDN(SOGHIDIENNUOCDTO sg);
        SOGHIDIENNUOC SuaSGDN(SOGHIDIENNUOCDTO sg);
        SOGHIDIENNUOC XoaSGDN(String MaSG);
        IEnumerable<SOGHIDIENNUOC> TimTatCaSGDN();
        SOGHIDIENNUOC TimSGDNTheoMaSGDN(String MaSG);
        IEnumerable<SOGHIDIENNUOC> TimSGDN(SOGHIDIENNUOCDTO sg);
    }
    public class SoGhiDienNuocDAO : ISoGhiDienNuocDAO
    {
        public SOGHIDIENNUOC ThemSGDN(SOGHIDIENNUOCDTO sg)
        {
            try
            {
                //Khai báo và khởi tạo đối tượng kết nối database
                KTXEntities KTXe = new KTXEntities();
                SOGHIDIENNUOC AddSG = new SOGHIDIENNUOC();
                AddSG.MaSo = sg.MaSo;
                AddSG.Nam = sg.Nam;
                AddSG.TenSo = sg.TenSo;
                //Thêm SG mới
                SOGHIDIENNUOC result = KTXe.SOGHIDIENNUOCs.Add(AddSG);
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
        public SOGHIDIENNUOC SuaSGDN(SOGHIDIENNUOCDTO sg)
        {
            try
            {
                //Khai báo kết nối data
                KTXEntities KTXe = new KTXEntities();
                SOGHIDIENNUOC editSG = KTXe.SOGHIDIENNUOCs.SingleOrDefault(x => x.MaSo == sg.MaSo);
                editSG.MaSo = sg.MaSo;
                editSG.Nam = sg.Nam;
                editSG.TenSo = sg.TenSo;
                int result = KTXe.SaveChanges();
                if (result == 1)
                {
                    return editSG;
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
        public SOGHIDIENNUOC XoaSGDN(String MaSG)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                SOGHIDIENNUOC delSG = KTXe.SOGHIDIENNUOCs.SingleOrDefault(x => x.MaSo == MaSG);
                SOGHIDIENNUOC result = KTXe.SOGHIDIENNUOCs.Remove(delSG);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<SOGHIDIENNUOC> TimTatCaSGDN()
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<SOGHIDIENNUOC> result = KTXe.SOGHIDIENNUOCs.AsEnumerable();
            return result;
        }
        public SOGHIDIENNUOC TimSGDNTheoMaSGDN(String MaSG)
        {
            KTXEntities KTXe = new KTXEntities();
            SOGHIDIENNUOC findSG = KTXe.SOGHIDIENNUOCs.SingleOrDefault(x => x.MaSo == MaSG);
            return findSG;
        }
        public IEnumerable<SOGHIDIENNUOC> TimSGDN(SOGHIDIENNUOCDTO sg)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<SOGHIDIENNUOC> findSG = KTXe.SOGHIDIENNUOCs.AsQueryable().Where(x => (sg.MaSo == "" || x.MaSo == sg.MaSo) & (sg.Nam == 0 || x.Nam == sg.Nam) & (sg.TenSo == "" || x.TenSo == sg.TenSo));
            return findSG;
        }
    }
}
