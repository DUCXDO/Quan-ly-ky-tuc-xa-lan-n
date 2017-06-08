using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAO
{
    public interface ITaiSanDAO
    {
        TAISAN ThemTS(TAISANDTO ts);
        TAISAN SuaTS(TAISANDTO ts);
        TAISAN XoaTS(String MaTS);
        IEnumerable<TAISAN> TimTatCaTS();
        TAISAN TimTSTheoMaTS(String MaTS);
        IEnumerable<TAISAN> TimTS(TAISANDTO ts);
    }
    public class TaiSanDAO : ITaiSanDAO
    {
        public TAISAN ThemTS(TAISANDTO ts)
        {
            try
            {
                //Khai báo và khởi tạo đối tượng kết nối database
                KTXEntities KTXe = new KTXEntities();
                TAISAN addTS = new TAISAN();
                addTS.MaTS = ts.MaTS;
                addTS.SoLuong = ts.SoLuong;
                addTS.TenTS = ts.TenTS;
                addTS.TinhTrang = ts.TinhTrang;

                //Thêm SG mới
                TAISAN result = KTXe.TAISANs.Add(addTS);
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
        public TAISAN SuaTS(TAISANDTO ts)
        {
            try
            {
                //Khai báo kết nối data
                KTXEntities KTXe = new KTXEntities();
                TAISAN editTS = KTXe.TAISANs.SingleOrDefault(x => x.MaTS == ts.MaTS);
                editTS.MaTS = ts.MaTS;
                editTS.SoLuong = ts.SoLuong;
                editTS.TenTS = ts.TenTS;
                editTS.TinhTrang = ts.TinhTrang;
                
                int result = KTXe.SaveChanges();
                if (result == 1)
                {
                    return editTS;
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
        public TAISAN XoaTS(String MaTS)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                TAISAN delTS = KTXe.TAISANs.SingleOrDefault(x => x.MaTS == MaTS);
                TAISAN result = KTXe.TAISANs.Remove(delTS);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<TAISAN> TimTatCaTS()
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<TAISAN> result = KTXe.TAISANs.AsEnumerable();
            return result;
        }
        public TAISAN TimTSTheoMaTS(String MaTS)
        {
            KTXEntities KTXe = new KTXEntities();
            TAISAN findTS = KTXe.TAISANs.SingleOrDefault(x => x.MaTS == MaTS);
            return findTS;

        }
        public IEnumerable<TAISAN> TimTS(TAISANDTO ts)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<TAISAN> findTS = KTXe.TAISANs.AsQueryable().Where(x => (ts.MaTS == "" || x.MaTS == ts.MaTS) && (ts.SoLuong == 0 || x.SoLuong == ts.SoLuong) && (ts.TenTS == "" || x.TenTS == ts.TenTS) && (ts.TinhTrang == "" || x.TinhTrang == ts.TinhTrang));
            return findTS;
        }
    }
}

