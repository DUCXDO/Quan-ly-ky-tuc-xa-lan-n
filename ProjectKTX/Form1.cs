using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAO;
using DTO;

namespace ProjectKTX
{
    public partial class Form1 : Form
    {
        // Khai báo các đối tượng bus
        private readonly SinhVienBUS SinhVienBUS;
        private readonly HopDongBUS HopDongBUS;
        private readonly PhieuThuBUS PhieuThuBUS;
        private readonly HoaDonBUS HoaDonBUS;
        private readonly PhieuGhiDienNuocBUS PhieuGhiDienNuocBUS;
        private readonly PhieuTrangBiBUS PhieuTrangBiBUS;
        private readonly PhongBUS PhongBUS;
        private readonly SoGhiDienNuocBUS SoGhiDienNuocBUS;
        private readonly TaiSanBUS TaiSanBUS;

        public Form1()
        {
            InitializeComponent();
            // Gán thực thể cho các đối tượng bus 
            SinhVienBUS = new SinhVienBUS(new SinhVienDAO(), new PhongDAO(), new HopDongDAO());
            HopDongBUS = new HopDongBUS(new HopDongDAO(), new PhongDAO());
            PhieuThuBUS = new PhieuThuBUS(new PhieuThuDAO(), new HopDongDAO());
            HoaDonBUS = new HoaDonBUS(new HoaDonDAO(), new SoGhiDienNuocDAO(), new PhongDAO(), new PhieuGhiDienNuocDAO());
            PhieuGhiDienNuocBUS = new PhieuGhiDienNuocBUS(new PhieuGhiDienNuocDAO(), new PhongDAO(), new SoGhiDienNuocDAO());
            PhieuTrangBiBUS = new PhieuTrangBiBUS(new TaiSanDAO(), new PhongDAO(), new PhieuTrangBiDAO());
            PhongBUS = new PhongBUS(new SinhVienDAO(), new PhongDAO());
            SoGhiDienNuocBUS = new SoGhiDienNuocBUS(new SoGhiDienNuocDAO());
            TaiSanBUS = new TaiSanBUS(new TaiSanDAO());
        }

        #region Sinh viên - Tìm kiếm

        // Load combo box khi vào tabpage tìm kiếm
        private void TabPage_Child_SinhVien_TimKiem_Enter(object sender, EventArgs e)
        {
            BindingList<PHONG> dataSource = new BindingList<PHONG>();

            foreach (var item in SinhVienBUS.DSTatCaPhong())
            {
                dataSource.Add(item);
            }
            // Đưa danh sách phòng thành datasource của combo box
            ComboBox_SinhVien_TimKiem_Phong.DataSource = dataSource;
            // Đặt giá trị hiện ra và giá trị chọn của combo box
            ComboBox_SinhVien_TimKiem_Phong.DisplayMember = "TenP";
            ComboBox_SinhVien_TimKiem_Phong.ValueMember = "MaPhong";

        }

        // Nút tìm kiếm được bấm
        private void Button_SinhVien_TimKiem_TimKiem_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng sinh viên mới
            SINHVIENDTO sv = new SINHVIENDTO();
            // Lấy mã sinh viên từ textbox
            sv.MaSV = TextBox_SinhVien_TimKiem_MaSV.Text;
            //Kiểm tra số CMND để lấy hay không
            int a;
            if (Int32.TryParse(TextBox_SinhVien_TimKiem_SoCMND.Text, out a) == true)
            {
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_TimKiem_SoCMND.Text);
            }
            // Lấy số điện thoại từ textbox
            sv.SoDT = TextBox_SinhVien_TimKiem_SoDT.Text;
            // Lấy tên sinh viên từ textbox
            sv.TenSV = TextBox_SinhVien_TimKiem_TenSV.Text;

            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in SinhVienBUS.TimSV(sv))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_SinhVien_TimKiem.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dataGridView_SinhVien_TimKiem.DataSource = dataSource;
            }
        }

        // Nút tìm theo phòng được bấm
        private void Button_SinhVien_TimKiem_TimTheoPhong_Click(object sender, EventArgs e)
        {
            BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
            foreach (var item in SinhVienBUS.TimSVTheoPhong(ComboBox_SinhVien_TimKiem_Phong.SelectedValue.ToString()))
            {
                dataSource.Add(item);
            }

            if (dataSource.Count == 0)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Hiện không có sinh viên nào ở phòng này!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
                dataGridView_SinhVien_TimKiem.DataSource = dataSource;
            }
        }

        // Nút xóa được bấm
        private void Button_SinhVien_TimKiem_Xoa_Click(object sender, EventArgs e)
        {

            if (dataGridView_SinhVien_TimKiem.SelectedRows.Count == 0)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Chưa có sinh viên nào được chọn để xóa!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
                SINHVIEN selected = dataGridView_SinhVien_TimKiem.SelectedRows[0].DataBoundItem as SINHVIEN;
                String result = SinhVienBUS.XoaSV(selected.MaSV);
                if (result == null)
                {
                    NotificationBox_SinhVien_TimKiem.Text = "Xóa sinh viên thành công!";
                    NotificationBox_SinhVien_TimKiem.Visible = true;
                    dataGridView_SinhVien_TimKiem.DataSource = new BindingList<SINHVIEN>();

                }
                else
                {
                    NotificationBox_SinhVien_TimKiem.Text = result;
                    NotificationBox_SinhVien_TimKiem.Visible = true;
                }
            }
        }

        // Nút sửa được bấm
        private void Button_SinhVien_TimKiem_Sua_Click(object sender, EventArgs e)
        {
            if (dataGridView_SinhVien_TimKiem.SelectedRows.Count == 0)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Chưa có sinh viên nào được chọn để sửa!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
                SINHVIEN selected = dataGridView_SinhVien_TimKiem.SelectedRows[0].DataBoundItem as SINHVIEN;
                // code để chuyển dữ liệu sang tabpage sửa
                TextBox_SinhVien_ThemSua_MaSV.Text = selected.MaSV;
                TextBox_SinhVien_ThemSua_DiaChi.Text = selected.DiaChi;
                TextBox_SinhVien_ThemSua_SoCMND.Text = selected.SoCMND.ToString();
                TextBox_SinhVien_ThemSua_SoDT.Text = selected.SoDT;
                TextBox_SinhVien_ThemSua_TenSV.Text = selected.TenSV;
                dateTimePicker_SinhVien_ThemSua_NgaySinh.Value = selected.NgaySinh;

                // Hiện tất cả sinh viên khi chuyển sang tabpage ThemSua
                BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
                foreach (var item in SinhVienBUS.TimTatCaSV())
                {
                    dataSource.Add(item);
                }
                dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                TabControl_Child_SinhVien.SelectedTab = TabPage_Child_SinhVien_ThemSua;
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_SinhVien_TimKiem_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_SinhVien_TimKiem.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_SinhVien_TimKiem.DataSource = new BindingList<SINHVIEN>();

            Control ctrl = TabPage_Child_SinhVien_TimKiem;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Sinh viên - Thêm mới/Sửa

        // Nút thêm mới được ấn
        private void Button_SinhVien_ThemSua_ThemMoi_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_SinhVien_ThemSua_SoCMND.Text, out a) == true)
            {
                SINHVIENDTO sv = new SINHVIENDTO();
                sv.MaSV = TextBox_SinhVien_ThemSua_MaSV.Text;
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_ThemSua_SoCMND.Text);
                sv.SoDT = TextBox_SinhVien_ThemSua_SoDT.Text;
                sv.TenSV = TextBox_SinhVien_ThemSua_TenSV.Text;
                sv.NgaySinh = dateTimePicker_SinhVien_ThemSua_NgaySinh.Value;
                sv.DiaChi = TextBox_SinhVien_ThemSua_DiaChi.Text;

                String result = SinhVienBUS.ThemSV(sv);
                if (result == null)
                {
                    NotificationBox_SinhVien_ThemSua.Text = "Thêm mới sinh viên thành công!";
                    NotificationBox_SinhVien_ThemSua.Visible = true;

                    BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
                    foreach (var item in SinhVienBUS.TimSV(sv))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_SinhVien_ThemSua.Text = result;
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                }
            }
            else
            {
                NotificationBox_SinhVien_ThemSua.Text = "Số chứng minh nhân dân không đúng định dạng!";
                NotificationBox_SinhVien_ThemSua.Visible = true;
            }
        }

        // Nút sửa được bấm
        private void Button_SinhVien_ThemSua_Sua_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_SinhVien_ThemSua_SoCMND.Text, out a) == true)
            {
                SINHVIENDTO sv = new SINHVIENDTO();
                sv.MaSV = TextBox_SinhVien_ThemSua_MaSV.Text;
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_ThemSua_SoCMND.Text);
                sv.SoDT = TextBox_SinhVien_ThemSua_SoDT.Text;
                sv.TenSV = TextBox_SinhVien_ThemSua_TenSV.Text;
                sv.NgaySinh = dateTimePicker_SinhVien_ThemSua_NgaySinh.Value;
                sv.DiaChi = TextBox_SinhVien_ThemSua_DiaChi.Text;

                String result = SinhVienBUS.SuaSV(sv);
                if (result == null)
                {
                    NotificationBox_SinhVien_ThemSua.Text = "Sửa sinh viên thành công!";
                    NotificationBox_SinhVien_ThemSua.Visible = true;

                    BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
                    foreach (var item in SinhVienBUS.TimSV(sv))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_SinhVien_ThemSua.Text = result;
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                }
            }
            else
            {
                NotificationBox_SinhVien_ThemSua.Text = "Số chứng minh nhân dân không đúng định dạng!";
                NotificationBox_SinhVien_ThemSua.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_SinhVien_ThemSua_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_SinhVien_ThemSua.SelectedRows.Count == 0)
            {
                NotificationBox_SinhVien_ThemSua.Text = "Chưa có sinh viên nào được chọn để xóa!";
                NotificationBox_SinhVien_ThemSua.Visible = true;
            }
            else
            {
                SINHVIEN selected = dataGridView_SinhVien_ThemSua.SelectedRows[0].DataBoundItem as SINHVIEN;
                String result = SinhVienBUS.XoaSV(selected.MaSV);
                if (result == null)
                {
                    NotificationBox_SinhVien_ThemSua.Text = "Xóa sinh viên thành công!";
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                    dataGridView_SinhVien_ThemSua.DataSource = new BindingList<SINHVIEN>();
                }
                else
                {
                    NotificationBox_SinhVien_ThemSua.Text = result;
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                }
            }
        }

        // Làm trắng textbox và datagrid khi chuyển tab
        private void TabPage_Child_SinhVien_ThemSua_Leave(object sender, EventArgs e)
        {
            NotificationBox_SinhVien_ThemSua.Visible = false;

            dataGridView_SinhVien_ThemSua.DataSource = new BindingList<SINHVIEN>();

            Control ctrl = TabPage_Child_SinhVien_ThemSua;

            ClearAllText(ctrl);
        }

        #endregion

        #region Hợp đồng - Tìm kiếm

        // Load combo box khi vào tabpage
        private void TabPage_Child_HopDong_TimKiem_Enter(object sender, EventArgs e)
        {
            BindingList<PHONG> dataSource = new BindingList<PHONG>();

            foreach (var item in SinhVienBUS.DSTatCaPhong())
            {
                dataSource.Add(item);
            }
            // Đưa danh sách phòng thành datasource của combo box
            ComboBox_HopDong_TimKiem_Phong.DataSource = dataSource;
            // Đặt giá trị hiện ra và giá trị chọn của combo box
            ComboBox_HopDong_TimKiem_Phong.DisplayMember = "TenP";
            ComboBox_HopDong_TimKiem_Phong.ValueMember = "MaPhong";
        }

        // Khi nút tìm kiếm được bấm
        private void Button_HopDong_TimKiem_TimKiem_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng hợp đồng mới
            HOPDONGDTO hd = new HOPDONGDTO();
            // Lấy mã sinh viên từ textbox
            hd.MaSV = TextBox_HopDong_TimKiem_MaSV.Text;
            // Lấy số hợp đồng từ textbox
            hd.SoHD = TextBox_HopDong_TimKiem_SoHD.Text;
            // Lấy mã phòng từ textbox
            hd.MaPhong = TextBox_HopDong_TimKiem_MaPhong.Text;

            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<HOPDONG> dataSource = new BindingList<HOPDONG>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in HopDongBUS.TimHD(hd))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_HopDong_TimKiem.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_HopDong_TimKiem.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dataGridView_HopDong_TimKiem.DataSource = dataSource;
            }
        }

        // Nút tìm theo phòng được bấm
        private void Button_HopDong_TimKiem_TimKiemTheoPhong_Click(object sender, EventArgs e)
        {
            BindingList<HOPDONG> dataSource = new BindingList<HOPDONG>();
            foreach (var item in HopDongBUS.TimHDTheoPhong(ComboBox_SinhVien_TimKiem_Phong.SelectedValue.ToString()))
            {
                dataSource.Add(item);
            }

            if (dataSource.Count == 0)
            {
                NotificationBox_HopDong_TimKiem.Text = "Hiện không có hợp đồng nào được lập với phòng này!";
                NotificationBox_HopDong_TimKiem.Visible = true;
            }
            else
            {
                dataGridView_HopDong_TimKiem.DataSource = dataSource;
            }
        }

        // Nút xóa được bấm
        private void Button_HopDong_TimKiem_Xoa_Click(object sender, EventArgs e)
        {

            if (dataGridView_HopDong_TimKiem.SelectedRows.Count == 0)
            {
                NotificationBox_HopDong_TimKiem.Text = "Chưa có hợp đồng nào được chọn để xóa!";
                NotificationBox_HopDong_TimKiem.Visible = true;
            }
            else
            {
                HOPDONG selected = dataGridView_HopDong_TimKiem.SelectedRows[0].DataBoundItem as HOPDONG;
                String result = HopDongBUS.XoaHD(selected.SoHD);
                if (result == null)
                {
                    NotificationBox_HopDong_TimKiem.Text = "Xóa hợp đồng thành công!";
                    NotificationBox_HopDong_TimKiem.Visible = true;
                    dataGridView_HopDong_TimKiem.DataSource = new BindingList<SINHVIEN>();

                }
                else
                {
                    NotificationBox_HopDong_TimKiem.Text = result;
                    NotificationBox_HopDong_TimKiem.Visible = true;
                }
            }
        }

        //Nút lập phiếu thu được bấm
        private void Button_HopDong_TimKiem_LapPhieuThu_Click(object sender, EventArgs e)
        {
            if (dataGridView_HopDong_TimKiem.SelectedRows.Count == 0)
            {
                NotificationBox_HopDong_TimKiem.Text = "Chưa có hợp đồng nào được chọn để lập phiếu thu!";
                NotificationBox_HopDong_TimKiem.Visible = true;
            }
            else
            {
                HOPDONG selected = dataGridView_HopDong_TimKiem.SelectedRows[0].DataBoundItem as HOPDONG;
                // code để chuyển dữ liệu sang tabpage thêm 
                TextBox_HopDong_LapPT_SoHD.Text = selected.SoHD;

                // Hiện tất cả phiếu thu khi chuyển sang tabpage LapPT
                BindingList<PHIEUTHU> dataSource = new BindingList<PHIEUTHU>();
                foreach (var item in PhieuThuBUS.TimTatCaPT())
                {
                    dataSource.Add(item);
                }
                dataGridView_HopDong_LapPT.DataSource = dataSource;
                TabControl_Child_HopDong.SelectedTab = TabPage_Child_HopDong_LapPT;
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_HopDong_TimKiem_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_HopDong_TimKiem.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_HopDong_TimKiem.DataSource = new BindingList<HOPDONG>();

            Control ctrl = TabPage_Child_HopDong_TimKiem;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Hợp đồng - Lập hợp đồng

        // Nút thêm mới được ấn
        private void Button_HopDong_LapHD_ThemMoi_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_HopDong_LapHD_ThoiGianO.Text, out a) == true)
            {
                HOPDONGDTO hd = new HOPDONGDTO();
                hd.MaSV = TextBox_SinhVien_ThemSua_MaSV.Text;
                hd.ThoiGianO = Int32.Parse(TextBox_HopDong_LapHD_ThoiGianO.Text);
                hd.SoHD = TextBox_HopDong_LapHD_SoHD.Text;
                hd.MaPhong = TextBox_HopDong_LapHD_MaP.Text;
                hd.NgayLap = DateTime.Now;

                String result = HopDongBUS.ThemHD(hd);
                if (result == null)
                {
                    NotificationBox_HopDong_LapHD.Text = "Lập hợp đồng thành công!";
                    NotificationBox_HopDong_LapHD.Visible = true;

                    BindingList<HOPDONG> dataSource = new BindingList<HOPDONG>();
                    foreach (var item in HopDongBUS.TimHD(hd))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_HopDong_LapHD.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_HopDong_LapHD.Text = result;
                    NotificationBox_HopDong_LapHD.Visible = true;
                }
            }
            else
            {
                NotificationBox_HopDong_LapHD.Text = "Thời gian ở không đúng định dạng!";
                NotificationBox_HopDong_LapHD.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_HopDong_LapHD_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_HopDong_LapHD.SelectedRows.Count == 0)
            {
                NotificationBox_HopDong_LapHD.Text = "Chưa có hợp đồng nào được chọn để xóa!";
                NotificationBox_HopDong_LapHD.Visible = true;
            }
            else
            {
                HOPDONG selected = dataGridView_HopDong_LapHD.SelectedRows[0].DataBoundItem as HOPDONG;
                String result = HopDongBUS.XoaHD(selected.SoHD);
                if (result == null)
                {
                    NotificationBox_HopDong_LapHD.Text = "Xóa hợp đồng thành công!";
                    NotificationBox_HopDong_LapHD.Visible = true;
                    dataGridView_HopDong_LapHD.DataSource = new BindingList<HOPDONG>();
                }
                else
                {
                    NotificationBox_HopDong_LapHD.Text = result;
                    NotificationBox_HopDong_LapHD.Visible = true;
                }
            }
        }

        // Làm trắng textbox và datagrid khi chuyển tab
        private void TabPage_Child_HopDong_LapHD_Leave(object sender, EventArgs e)
        {
            NotificationBox_HopDong_LapHD.Visible = false;

            dataGridView_HopDong_LapHD.DataSource = new BindingList<HOPDONG>();

            Control ctrl = TabPage_Child_HopDong_LapHD;

            ClearAllText(ctrl);
        }

        #endregion

        #region Hợp đồng - Tìm phiếu thu

        // Nút tìm kiếm được bấm
        private void Button_HopDong_TimPT_TimKiem_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng sinh viên mới
            PHIEUTHUDTO pt = new PHIEUTHUDTO();

            // Lấy số hợp đồng từ textbox
            pt.SoHD = TextBox_HopDong_TimPT_SoHD.Text;
            // Lấy số phiếu thu textbox
            pt.SoPT = TextBox_HopDong_TimPT_SoPT.Text;

            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<PHIEUTHU> dataSource = new BindingList<PHIEUTHU>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in PhieuThuBUS.TimPT(pt))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_HopDong_TimPT.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_HopDong_TimPT.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dataGridView_HopDong_TimPT.DataSource = dataSource;
            }
        }

        // Nút tìm theo ngày lập được bấm
        private void Button_HopDong_TimPT_TimTheoNgayLap_Click(object sender, EventArgs e)
        {
            BindingList<PHIEUTHU> dataSource = new BindingList<PHIEUTHU>();
            foreach (var item in PhieuThuBUS.TimPTTheoNgayLap(dateTimePicker_HopDong_TimPT_NgayLap.Value))
            {
                dataSource.Add(item);
            }

            if (dataSource.Count == 0)
            {
                NotificationBox_HopDong_TimPT.Text = "Không có phiếu thu nào được lập vào ngày đã chọn!";
                NotificationBox_HopDong_TimPT.Visible = true;
            }
            else
            {
                dataGridView_HopDong_TimPT.DataSource = dataSource;
            }
        }

        // Nút xóa được bấm
        private void Button_HopDong_TimPT_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_HopDong_TimPT.SelectedRows.Count == 0)
            {
                NotificationBox_HopDong_TimPT.Text = "Chưa có phiếu thu nào được chọn để xóa!";
                NotificationBox_HopDong_TimPT.Visible = true;
            }
            else
            {
                PHIEUTHU selected = dataGridView_HopDong_TimPT.SelectedRows[0].DataBoundItem as PHIEUTHU;
                String result = PhieuThuBUS.XoaPT(selected.SoPT);
                if (result == null)
                {
                    NotificationBox_HopDong_TimPT.Text = "Xóa phiếu thu thành công!";
                    NotificationBox_HopDong_TimPT.Visible = true;
                    dataGridView_HopDong_TimPT.DataSource = new BindingList<PHIEUTHU>();

                }
                else
                {
                    NotificationBox_HopDong_TimPT.Text = result;
                    NotificationBox_HopDong_TimPT.Visible = true;
                }
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_HopDong_TimPT_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_HopDong_TimPT.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_HopDong_TimPT.DataSource = new BindingList<SINHVIEN>();

            Control ctrl = TabPage_Child_HopDong_TimPT;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Hợp đồng - Thêm phiếu thu

        // Nút thêm mới được ấn
        private void Button_HopDong_LapPT_ThemMoi_Click(object sender, EventArgs e)
        {
            Decimal a;
            if (Decimal.TryParse(TextBox_HopDong_LapPT_TienThu.Text, out a) == true)
            {
                PHIEUTHUDTO pt = new PHIEUTHUDTO();
                pt.SoPT = TextBox_HopDong_LapPT_SoPT.Text;
                pt.TienThu = Decimal.Parse(TextBox_HopDong_LapPT_TienThu.Text);
                pt.SoHD = TextBox_HopDong_LapPT_SoHD.Text;
                pt.NgayLap = DateTime.Now;

                String result = PhieuThuBUS.ThemPT(pt);
                if (result == null)
                {
                    NotificationBox_HopDong_LapPT.Text = "Thêm phiếu thu thành công!";
                    NotificationBox_HopDong_LapPT.Visible = true;

                    BindingList<PHIEUTHU> dataSource = new BindingList<PHIEUTHU>();
                    foreach (var item in PhieuThuBUS.TimPT(pt))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_HopDong_LapPT.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_HopDong_LapPT.Text = result;
                    NotificationBox_HopDong_LapPT.Visible = true;
                }
            }
            else
            {
                NotificationBox_HopDong_LapPT.Text = "Số tiền thu không đúng định dạng!";
                NotificationBox_HopDong_LapPT.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_HopDong_LapPT_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_HopDong_LapPT.SelectedRows.Count == 0)
            {
                NotificationBox_HopDong_LapPT.Text = "Chưa có phiếu thu nào được chọn để xóa!";
                NotificationBox_HopDong_LapPT.Visible = true;
            }
            else
            {
                PHIEUTHU selected = dataGridView_HopDong_LapPT.SelectedRows[0].DataBoundItem as PHIEUTHU;
                String result = PhieuThuBUS.XoaPT(selected.SoPT);
                if (result == null)
                {
                    NotificationBox_HopDong_LapPT.Text = "Xóa phiếu thu thành công!";
                    NotificationBox_HopDong_LapPT.Visible = true;
                    dataGridView_HopDong_LapPT.DataSource = new BindingList<PHIEUTHU>();
                }
                else
                {
                    NotificationBox_HopDong_LapPT.Text = result;
                    NotificationBox_HopDong_LapPT.Visible = true;
                }
            }
        }

        // Làm trắng textbox và datagrid khi chuyển tab
        private void TabPage_Child_HopDong_LapPT_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_HopDong_LapPT.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_HopDong_LapPT.DataSource = new BindingList<SINHVIEN>();

            Control ctrl = TabPage_Child_HopDong_LapPT;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Điện nước - Lập hóa đơn

        // Nút tìm kiếm được bấm
        private void Button_DienNuoc_LapHDDN_ThemMoi_Click(object sender, EventArgs e)
        {
            Decimal a;
            if (Decimal.TryParse(TextBox_DienNuoc_LapHDDN_SoTienThu.Text, out a) == true)
            {
                HOADONDTO hd = new HOADONDTO();
                hd.SoHoaDon = TextBox_DienNuoc_LapHDDN_SoHD.Text;
                hd.SoTien = Decimal.Parse(TextBox_DienNuoc_LapHDDN_SoTienThu.Text);
                hd.MaPhieuGhiDienNuoc = TextBox_DienNuoc_LapHDDN_MaPG.Text;

                String result = HoaDonBUS.ThemHD(hd);
                if (result == null)
                {
                    NotificationBox_DienNuoc_LapHDDN.Text = "Thêm hóa đơn thành công!";
                    NotificationBox_DienNuoc_LapHDDN.Visible = true;

                    BindingList<HOADON> dataSource = new BindingList<HOADON>();
                    foreach (var item in HoaDonBUS.TimHD(hd))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_DienNuoc_LapHDDN.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_DienNuoc_LapHDDN.Text = result;
                    NotificationBox_DienNuoc_LapHDDN.Visible = true;
                }
            }
            else
            {
                NotificationBox_DienNuoc_LapHDDN.Text = "Số tiền thu không đúng định dạng!";
                NotificationBox_DienNuoc_LapHDDN.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_DienNuoc_LapHDDN_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_SinhVien_ThemSua.SelectedRows.Count == 0)
            {
                NotificationBox_DienNuoc_LapHDDN.Text = "Chưa có hóa đơn nào được chọn để xóa!";
                NotificationBox_DienNuoc_LapHDDN.Visible = true;
            }
            else
            {
                HOADON selected = dataGridView_DienNuoc_LapHDDN.SelectedRows[0].DataBoundItem as HOADON;
                String result = HoaDonBUS.XoaHD(selected.SoHoaDon);
                if (result == null)
                {
                    NotificationBox_DienNuoc_LapHDDN.Text = "Xóa hóa đơn thành công!";
                    NotificationBox_DienNuoc_LapHDDN.Visible = true;
                    dataGridView_DienNuoc_LapHDDN.DataSource = new BindingList<HOADON>();
                }
                else
                {
                    NotificationBox_DienNuoc_LapHDDN.Text = result;
                    NotificationBox_DienNuoc_LapHDDN.Visible = true;
                }
            }
        }

        //Làm trắng textbox và datagrid khi chuyển tab
        private void TabPage_Child_DienNuoc_LapHDDN_Leave(object sender, EventArgs e)
        {
            NotificationBox_DienNuoc_LapHDDN.Visible = false;

            dataGridView_DienNuoc_LapHDDN.DataSource = new BindingList<HOADON>();

            Control ctrl = TabPage_Child_DienNuoc_LapHDDN;

            ClearAllText(ctrl);
        }

        #endregion

        #region Điện nước - Lập phiếu ghi điện nước

        // Load combo box khi vào tabpage lập phiếu ghi
        private void TabPage_Child_DienNuoc_LapPGDN_Enter(object sender, EventArgs e)
        {
            BindingList<PHONG> dataSource = new BindingList<PHONG>();

            foreach (var item in SinhVienBUS.DSTatCaPhong())
            {
                dataSource.Add(item);
            }
            // Đưa danh sách phòng thành datasource của combo box
            ComboBox_DienNuoc_LapPGDN_Phong.DataSource = dataSource;
            // Đặt giá trị hiện ra và giá trị chọn của combo box
            ComboBox_DienNuoc_LapPGDN_Phong.DisplayMember = "TenP";
            ComboBox_DienNuoc_LapPGDN_Phong.ValueMember = "MaPhong";

            BindingList<SOGHIDIENNUOC> dataSource2 = new BindingList<SOGHIDIENNUOC>();

            foreach (var item in SoGhiDienNuocBUS.TimTatCaSGDN())
            {
                dataSource2.Add(item);
            }
            // Đưa danh sách sổ ghi thành datasource của combo box
            ComboBox_DienNuoc_LapPGDN_SoSGDN.DataSource = dataSource2;
            // Đặt giá trị hiện ra và giá trị chọn của combo box
            ComboBox_DienNuoc_LapPGDN_SoSGDN.DisplayMember = "TenSo";
            ComboBox_DienNuoc_LapPGDN_SoSGDN.ValueMember = "MaSo";
        }

        // Nút thêm mới được ấn
        private void Button_DienNuoc_LapPGDN_ThemMoi_Click(object sender, EventArgs e)
        {
            if (CheckBox_DienNuoc_LapPGDN_Dien.Checked == true && CheckBox_DienNuoc_LapPGDN_Nuoc.Checked == true)
            {
                NotificationBox_DienNuoc_LapPGDN.Text = "Không được check cả hai ô điện và nước!";
                NotificationBox_DienNuoc_LapPGDN.Visible = true;
            }
            else if(CheckBox_DienNuoc_LapPGDN_Dien.Checked == false && CheckBox_DienNuoc_LapPGDN_Nuoc.Checked == false)
            {
                NotificationBox_DienNuoc_LapPGDN.Text = "Loại phiếu ghi không được bỏ trống! Hãy chọn một trong hai!";
                NotificationBox_DienNuoc_LapPGDN.Visible = true;
            }
            else
            {
                int a;
                if (Int32.TryParse(TextBox_DienNuoc_LapPGDN_SoDN.Text, out a) == true)
                {
                    PHIEUGHIDIENNUOCDTO pg = new PHIEUGHIDIENNUOCDTO();
                    pg.MaPhieuGhiDienNuoc = TextBox_DienNuoc_LapPGDN_MaPG.Text;
                    pg.SoDienNuoc = Int32.Parse(TextBox_DienNuoc_LapPGDN_SoDN.Text);
                    pg.MaPhong = ComboBox_DienNuoc_LapPGDN_Phong.SelectedValue.ToString();
                    pg.MaSo = ComboBox_DienNuoc_LapPGDN_SoSGDN.SelectedValue.ToString();
                    pg.NgayGhi = DateTime.Now;
                    if (CheckBox_DienNuoc_LapPGDN_Dien.Checked)
                        pg.LoaiPhieuGhi = true;
                    if (CheckBox_DienNuoc_LapPGDN_Nuoc.Checked)
                        pg.LoaiPhieuGhi = false;

                    String result = PhieuGhiDienNuocBUS.ThemPG(pg);
                    if (result == null)
                    {
                        NotificationBox_DienNuoc_LapPGDN.Text = "Thêm phiếu ghi điện nước thành công!";
                        NotificationBox_DienNuoc_LapPGDN.Visible = true;

                        BindingList<PHIEUGHIDIENNUOC> dataSource = new BindingList<PHIEUGHIDIENNUOC>();
                        foreach (var item in PhieuGhiDienNuocBUS.TimPG(pg))
                        {
                            dataSource.Add(item);
                        }
                        dataGridView_DienNuoc_LapPGDN.DataSource = dataSource;
                    }
                    else
                    {
                        NotificationBox_DienNuoc_LapPGDN.Text = result;
                        NotificationBox_DienNuoc_LapPGDN.Visible = true;
                    }
                }
                else
                {
                    NotificationBox_DienNuoc_LapPGDN.Text = "Số điện/nước không đúng định dạng!";
                    NotificationBox_DienNuoc_LapPGDN.Visible = true;
                }
            }
        }

        // Nút sửa được bấm
        private void Button_DienNuoc_LapPGDN_Sua_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_DienNuoc_LapPGDN_SoDN.Text, out a) == true)
            {
                PHIEUGHIDIENNUOCDTO pg = new PHIEUGHIDIENNUOCDTO();
                pg.MaPhieuGhiDienNuoc = TextBox_DienNuoc_LapPGDN_MaPG.Text;
                pg.SoDienNuoc = Int32.Parse(TextBox_DienNuoc_LapPGDN_SoDN.Text);
                pg.MaPhong = ComboBox_DienNuoc_LapPGDN_Phong.SelectedValue.ToString();
                pg.MaSo = ComboBox_DienNuoc_LapPGDN_SoSGDN.SelectedValue.ToString();
                pg.NgayGhi = DateTime.Now;
                if (CheckBox_DienNuoc_LapPGDN_Dien.Checked)
                    pg.LoaiPhieuGhi = true;
                if (CheckBox_DienNuoc_LapPGDN_Nuoc.Checked)
                    pg.LoaiPhieuGhi = false;

                String result = PhieuGhiDienNuocBUS.SuaPG(pg);
                if (result == null)
                {
                    NotificationBox_DienNuoc_LapPGDN.Text = "Sửa phiếu ghi thành công thành công!";
                    NotificationBox_DienNuoc_LapPGDN.Visible = true;

                    BindingList<PHIEUGHIDIENNUOC> dataSource = new BindingList<PHIEUGHIDIENNUOC>();
                    foreach (var item in PhieuGhiDienNuocBUS.TimPG(pg))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_DienNuoc_LapPGDN.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_DienNuoc_LapPGDN.Text = result;
                    NotificationBox_DienNuoc_LapPGDN.Visible = true;
                }
            }
            else
            {
                NotificationBox_DienNuoc_LapPGDN.Text = "Số điện/nước không đúng định dạng!";
                NotificationBox_DienNuoc_LapPGDN.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_DienNuoc_LapPGDN_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_DienNuoc_LapPGDN.SelectedRows.Count == 0)
            {
                NotificationBox_DienNuoc_LapPGDN.Text = "Chưa có phiếu ghi nào được chọn để xóa!";
                NotificationBox_DienNuoc_LapPGDN.Visible = true;
            }
            else
            {
                PHIEUGHIDIENNUOC selected = dataGridView_DienNuoc_LapPGDN.SelectedRows[0].DataBoundItem as PHIEUGHIDIENNUOC;
                String result = PhieuGhiDienNuocBUS.XoaPG(selected.MaPhieuGhiDienNuoc);
                if (result == null)
                {
                    NotificationBox_DienNuoc_LapPGDN.Text = "Xóa phiếu ghi thành công!";
                    NotificationBox_DienNuoc_LapPGDN.Visible = true;
                    dataGridView_DienNuoc_LapPGDN.DataSource = new BindingList<PHIEUGHIDIENNUOC>();

                }
                else
                {
                    NotificationBox_DienNuoc_LapPGDN.Text = result;
                    NotificationBox_DienNuoc_LapPGDN.Visible = true;
                }
            }
        }

        // Format cột loại phiếu ghi để hiện thị 
        private void dataGridView_DienNuoc_LapPGDN_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex].Name == "LapPGDN_LoaiPhieuGhi" &&
                e.RowIndex >= 0 &&
                dgv["LapPGDN_LoaiPhieuGhi", e.RowIndex].Value is bool)
            {
                switch ((bool)dgv["TargetColumnName", e.RowIndex].Value)
                {
                    case true:
                        e.Value = "Điện";
                        e.FormattingApplied = true;
                        break;
                    case false:
                        e.Value = "Nước";
                        e.FormattingApplied = true;
                        break;
                }
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_DienNuoc_LapPGDN_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_DienNuoc_LapPGDN.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_DienNuoc_LapPGDN.DataSource = new BindingList<PHIEUGHIDIENNUOC>();

            Control ctrl = TabPage_Child_DienNuoc_LapPGDN;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Điện nước - Tìm phiếu ghi điện nước

        // Load combo box khi vào tabpage tìm kiếm
        private void TabPage_Child_DienNuoc_TimPGDN_Enter(object sender, EventArgs e)
        {
            BindingList<PHONG> dataSource = new BindingList<PHONG>();

            foreach (var item in SinhVienBUS.DSTatCaPhong())
            {
                dataSource.Add(item);
            }
            // Đưa danh sách phòng thành datasource của combo box
            ComboBox_DienNuoc_TimPGDN_Phong.DataSource = dataSource;
            // Đặt giá trị hiện ra và giá trị chọn của combo box
            ComboBox_DienNuoc_TimPGDN_Phong.DisplayMember = "TenP";
            ComboBox_DienNuoc_TimPGDN_Phong.ValueMember = "MaPhong";
        }

        // Nút tìm kiếm được bấm
        private void Button_DienNuoc_TimPGDN_TimKiem_Click(object sender, EventArgs e)
        {
            // Tạo phiếu ghi mới
            PHIEUGHIDIENNUOCDTO pg = new PHIEUGHIDIENNUOCDTO();

            pg.MaPhieuGhiDienNuoc = TextBox_DienNuoc_TimPGDN_MaPG.Text;
            pg.MaSo = TextBox_DienNuoc_TimPGDN_MaSGDN.Text;
            pg.MaPhong = TextBox_DienNuoc_TimPGDN_MaP.Text;
            if (CheckBox_DienNuoc_TimPGDN_Dien.Checked)
            {
                pg.LoaiPhieuGhi = true;
            }
            if (CheckBox_DienNuoc_TimPGDN_Nuoc.Checked)
            {
                pg.LoaiPhieuGhi = false;
            }
            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<PHIEUGHIDIENNUOC> dataSource = new BindingList<PHIEUGHIDIENNUOC>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            // Nếu cả hai ô checkbox đều được check
            if (CheckBox_DienNuoc_TimPGDN_Dien.Checked && CheckBox_DienNuoc_TimPGDN_Nuoc.Checked)
            {
                foreach (var item in PhieuGhiDienNuocBUS.TimPG(pg))
                {
                    dataSource.Add(item);
                }
            }
            else if (CheckBox_DienNuoc_TimPGDN_Dien.Checked || CheckBox_DienNuoc_TimPGDN_Nuoc.Checked)
            {
                foreach (var item in PhieuGhiDienNuocBUS.TimPGTheoLoaiPhieuGhi(pg))
                {
                    dataSource.Add(item);
                }
            }
            else
            {
                foreach (var item in PhieuGhiDienNuocBUS.TimPG(pg))
                {
                    dataSource.Add(item);
                }
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_DienNuoc_TimPGDN.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_DienNuoc_TimPGDN.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dataGridView_DienNuoc_TimPGDN.DataSource = dataSource;
            }
        }

        // Nút tìm theo phòng được bấm
        private void Button_DienNuoc_TimPGDN_TimTheoPhong_Click(object sender, EventArgs e)
        {
            BindingList<PHIEUGHIDIENNUOC> dataSource = new BindingList<PHIEUGHIDIENNUOC>();
            foreach (var item in PhieuGhiDienNuocBUS.TimPGTheoPhong(ComboBox_DienNuoc_TimPGDN_Phong.SelectedValue.ToString()))
            {
                dataSource.Add(item);
            }

            if (dataSource.Count == 0)
            {
                NotificationBox_DienNuoc_TimPGDN.Text = "Hiện không có phiếu ghi nào được ghi cho phòng này!";
                NotificationBox_DienNuoc_TimPGDN.Visible = true;
            }
            else
            {
                dataGridView_DienNuoc_TimPGDN.DataSource = dataSource;
            }
        }

        // Nút xóa được bấm
        private void Button_DienNuoc_TimPGDN_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_DienNuoc_TimPGDN.SelectedRows.Count == 0)
            {
                NotificationBox_DienNuoc_TimPGDN.Text = "Chưa có sinh viên nào được chọn để xóa!";
                NotificationBox_DienNuoc_TimPGDN.Visible = true;
            }
            else
            {
                PHIEUGHIDIENNUOC selected = dataGridView_DienNuoc_TimPGDN.SelectedRows[0].DataBoundItem as PHIEUGHIDIENNUOC;
                String result = PhieuGhiDienNuocBUS.XoaPG(selected.MaPhieuGhiDienNuoc);
                if (result == null)
                {
                    NotificationBox_DienNuoc_TimPGDN.Text = "Xóa phiếu ghi thành công!";
                    NotificationBox_DienNuoc_TimPGDN.Visible = true;
                    dataGridView_DienNuoc_TimPGDN.DataSource = new BindingList<PHIEUGHIDIENNUOC>();

                }
                else
                {
                    NotificationBox_DienNuoc_TimPGDN.Text = result;
                    NotificationBox_DienNuoc_TimPGDN.Visible = true;
                }
            }
        }

        // Nút lập hóa đơn được bấm
        private void Button_DienNuoc_TimPGDN_LapHD_Click(object sender, EventArgs e)
        {
            if (dataGridView_DienNuoc_TimPGDN.SelectedRows.Count == 0)
            {
                NotificationBox_DienNuoc_TimPGDN.Text = "Chưa có hợp đồng nào được chọn để lập phiếu thu!";
                NotificationBox_DienNuoc_TimPGDN.Visible = true;
            }
            else
            {
                PHIEUGHIDIENNUOC selected = dataGridView_DienNuoc_TimPGDN.SelectedRows[0].DataBoundItem as PHIEUGHIDIENNUOC;
                // code để chuyển dữ liệu sang tabpage thêm 
                TextBox_DienNuoc_LapHDDN_MaPG.Text = selected.MaPhieuGhiDienNuoc;

                // Hiện tất cả hóa đơn khi chuyển sang tabpage LapPT
                BindingList<HOADON> dataSource = new BindingList<HOADON>();
                foreach (var item in HoaDonBUS.TimTatCaHD())
                {
                    dataSource.Add(item);
                }
                dataGridView_DienNuoc_LapHDDN.DataSource = dataSource;
                TabControl_Child_DienNuoc.SelectedTab = TabPage_Child_DienNuoc_LapHDDN;
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_DienNuoc_TimPGDN_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_DienNuoc_TimPGDN.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_DienNuoc_TimPGDN.DataSource = new BindingList<PHIEUGHIDIENNUOC>();

            Control ctrl = TabPage_Child_DienNuoc_TimPGDN;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Điện nước - Tìm hóa đơn điện nước

        // Load combo box khi vào tabpage tìm kiếm
        private void TabPage_Child_DienNuoc_TimHDDN_Enter(object sender, EventArgs e)
        {
            BindingList<PHONG> dataSource = new BindingList<PHONG>();

            foreach (var item in SinhVienBUS.DSTatCaPhong())
            {
                dataSource.Add(item);
            }
            // Đưa danh sách phòng thành datasource của combo box
            ComboBox_DienNuoc_TimHDDN_Phong.DataSource = dataSource;
            // Đặt giá trị hiện ra và giá trị chọn của combo box
            ComboBox_DienNuoc_TimHDDN_Phong.DisplayMember = "TenP";
            ComboBox_DienNuoc_TimHDDN_Phong.ValueMember = "MaPhong";
        }

        // Nút tìm kiếm được bấm
        private void Button_DienNuoc_TimHDDN_TimKiem_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng sinh viên mới
            SINHVIENDTO sv = new SINHVIENDTO();
            // Lấy mã sinh viên từ textbox
            sv.MaSV = TextBox_SinhVien_TimKiem_MaSV.Text;
            //Kiểm tra số CMND để lấy hay không
            int a;
            if (Int32.TryParse(TextBox_SinhVien_TimKiem_SoCMND.Text, out a) == true)
            {
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_TimKiem_SoCMND.Text);
            }
            // Lấy số điện thoại từ textbox
            sv.SoDT = TextBox_SinhVien_TimKiem_SoDT.Text;
            // Lấy tên sinh viên từ textbox
            sv.TenSV = TextBox_SinhVien_TimKiem_TenSV.Text;

            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in SinhVienBUS.TimSV(sv))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_SinhVien_TimKiem.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dataGridView_SinhVien_TimKiem.DataSource = dataSource;
            }
        }

        // Nút tìm theo phòng được bấm
        private void Button_DienNuoc_TimHDDN_TimTheoPhong_Click(object sender, EventArgs e)
        {
            BindingList<HOADON> dataSource = new BindingList<HOADON>();
            foreach (var item in HoaDonBUS.TimHDTheoPhong(ComboBox_DienNuoc_TimHDDN_Phong.SelectedValue.ToString()))
            {
                dataSource.Add(item);
            }

            if (dataSource.Count == 0)
            {
                NotificationBox_DienNuoc_TimHDDN.Text = "Hiện không có hóa đơn nào ghi cho phòng này!";
                NotificationBox_DienNuoc_TimHDDN.Visible = true;
            }
            else
            {
                dataGridView_DienNuoc_TimHDDN.DataSource = dataSource;
            }
        }

        // Nút xóa được bấm
        private void Button_DienNuoc_TimHDDN_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_DienNuoc_TimHDDN.SelectedRows.Count == 0)
            {
                NotificationBox_DienNuoc_TimHDDN.Text = "Chưa có hóa đơn nào được chọn để xóa!";
                NotificationBox_DienNuoc_TimHDDN.Visible = true;
            }
            else
            {
                HOADON selected = dataGridView_DienNuoc_TimHDDN.SelectedRows[0].DataBoundItem as HOADON;
                String result = HoaDonBUS.XoaHD(selected.SoHoaDon);
                if (result == null)
                {
                    NotificationBox_DienNuoc_TimHDDN.Text = "Xóa hóa đơn thành công!";
                    NotificationBox_DienNuoc_TimHDDN.Visible = true;
                    dataGridView_DienNuoc_TimHDDN.DataSource = new BindingList<HOADON>();

                }
                else
                {
                    NotificationBox_DienNuoc_TimHDDN.Text = result;
                    NotificationBox_DienNuoc_TimHDDN.Visible = true;
                }
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_DienNuoc_TimHDDN_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_DienNuoc_TimHDDN.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_DienNuoc_TimHDDN.DataSource = new BindingList<HOADON>();

            Control ctrl = TabPage_Child_DienNuoc_TimHDDN;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Điện nước - Lập sổ ghi điện nước

        // Load dữ liệu khi vào tab
        private void TabPage_Child_DienNuoc_ThemSGDN_Enter(object sender, EventArgs e)
        {
            BindingList<SOGHIDIENNUOC> dataSource = new BindingList<SOGHIDIENNUOC>();
            foreach (var item in SoGhiDienNuocBUS.TimTatCaSGDN())
            {
                dataSource.Add(item);
            }
            dataGridView_DienNuoc_ThemSGDN.DataSource = dataSource;
        }

        // Nút thêm mới được bấm
        private void Button_DienNuoc_ThemSGDN_ThemMoi_Click_1(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_DienNuoc_ThemSGDN_NamGhi.Text, out a) == true)
            {
                SOGHIDIENNUOCDTO sg = new SOGHIDIENNUOCDTO();
                sg.MaSo = TextBox_DienNuoc_ThemSGDN_MaSo.Text;
                sg.Nam = Int32.Parse(TextBox_DienNuoc_ThemSGDN_NamGhi.Text);
                sg.TenSo = TextBox_DienNuoc_ThemSGDN_TenSo.Text;

                String result = SoGhiDienNuocBUS.ThemSoGhi(sg);
                if (result == null)
                {
                    NotificationBox_DienNuoc_ThemSGDN.Text = "Thêm mới sổ ghi thành công!";
                    NotificationBox_DienNuoc_ThemSGDN.Visible = true;

                    BindingList<SOGHIDIENNUOC> dataSource = new BindingList<SOGHIDIENNUOC>();
                    foreach (var item in SoGhiDienNuocBUS.TimSG(sg))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_DienNuoc_ThemSGDN.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_DienNuoc_ThemSGDN.Text = result;
                    NotificationBox_DienNuoc_ThemSGDN.Visible = true;
                }
            }
            else
            {
                NotificationBox_DienNuoc_ThemSGDN.Text = "Năm ghi sổ không đúng định dạng!";
                NotificationBox_DienNuoc_ThemSGDN.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_DienNuoc_ThemSGDN_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_DienNuoc_ThemSGDN.SelectedRows.Count == 0)
            {
                NotificationBox_DienNuoc_ThemSGDN.Text = "Chưa có sổ ghi nào được chọn để xóa!";
                NotificationBox_DienNuoc_ThemSGDN.Visible = true;
            }
            else
            {
                SOGHIDIENNUOC selected = dataGridView_DienNuoc_ThemSGDN.SelectedRows[0].DataBoundItem as SOGHIDIENNUOC;
                String result = SoGhiDienNuocBUS.XoaSG(selected.MaSo);
                if (result == null)
                {
                    NotificationBox_DienNuoc_ThemSGDN.Text = "Xóa sổ ghi thành công!";
                    NotificationBox_DienNuoc_ThemSGDN.Visible = true;
                    BindingList<SOGHIDIENNUOC> dataSource = new BindingList<SOGHIDIENNUOC>();
                    foreach (var item in SoGhiDienNuocBUS.TimTatCaSGDN())
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_DienNuoc_ThemSGDN.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_DienNuoc_ThemSGDN.Text = result;
                    NotificationBox_DienNuoc_ThemSGDN.Visible = true;
                }
            }
        }

        // Làm trắng textbox và datagrid khi chuyển tab
        private void TabPage_Child_DienNuoc_ThemSGDN_Leave(object sender, EventArgs e)
        {
            NotificationBox_DienNuoc_ThemSGDN.Visible = false;

            dataGridView_DienNuoc_ThemSGDN.DataSource = new BindingList<SOGHIDIENNUOC>();

            Control ctrl = TabPage_Child_DienNuoc_ThemSGDN;

            ClearAllText(ctrl);
        }

        #endregion

        #region Phòng - Tìm kiếm phòng

        private void iTalk_HeaderLabel10_Click(object sender, EventArgs e)
        {

        }
        // Button tìm kiếm
        private void btn_phong_timkiem_Click_1(object sender, EventArgs e)
        {
            // Tạo đối tượng Phòng mới
            PHONGDTO p = new PHONGDTO();
            // Lấy mã phòng từ textbox
            p.MaPhong = TextBox_Phong_Timkiem_maP.Text;
            // Lấy tên phòng từ txtbox
            p.TenP = TextBox_Phong_Timkiem_tenP.Text;
            // Lấy số người ở từ txtbox
            p.SoNguoiO = Int32.Parse(TextBox_Phong_Timkiem_soN.Text);
            // Lấy số người ở tối đa từ txtbox
            p.SoNguoiTD = Int32.Parse(TextBox_Phong_Timkiem_SNTD.Text);
            // Lấy vị trí phòng từ textbox
            p.ViTriP = TextBox_Phong_Timkiem_viTri.Text;
            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<PHONG> dataSource = new BindingList<PHONG>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in PhongBUS.TimP(p))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_Phong_timkiem.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dtg_Phong_Timkiem.DataSource = dataSource;
            }
        }
        // Button sửa
        private void btn_phong_tk_sua_Click_1(object sender, EventArgs e)
        {
            if (dtg_Phong_Timkiem.SelectedRows.Count == 0)
            {
                NotificationBox_Phong_timkiem.Text = "Chưa có phòng nào được chọn để sửa!";
                NotificationBox_Phong_timkiem.Visible = true;
            }
            else
            {
                PHONG selected = dtg_Phong_Timkiem.SelectedRows[0].DataBoundItem as PHONG;
                // code để chuyển dữ liệu sang tabpage thêm sửa
                TextBox_Phong_Them_maP.Text = selected.MaPhong;
                TextBox_Phong_Them_tenP.Text = selected.TenP;
                TextBox_Phong_Them_viTri.Text = selected.ViTriP;
                TextBox_Phong_Them_soN.Text = Convert.ToString(selected.SoNguoiO);
                TextBox_Phong_Them_SNTD.Text = Convert.ToString(selected.SoNguoiTD);

                // Hiện tất cả sinh viên khi chuyển sang tabpage ThemSua
                BindingList<PHONG> dataSource = new BindingList<PHONG>();
                foreach (var item in PhongBUS.TimTatCaP())
                {
                    dataSource.Add(item);
                }
                dtg_Phong_Themsua.DataSource = dataSource;
                TabControl_Child_Phong.SelectedTab = TabPage_Child_Phong_ThemSua;
            }
        }
        // Button xóa
        private void btn_phong_tk_xoa_Click_1(object sender, EventArgs e)
        {
            if (dtg_Phong_Timkiem.SelectedRows.Count == 0)
            {
                NotificationBox_Phong_timkiem.Text = "Chưa có phòng nào được chọn để xóa!";
                NotificationBox_Phong_timkiem.Visible = true;
            }
            else
            {
                PHONG selected = dtg_Phong_Timkiem.SelectedRows[0].DataBoundItem as PHONG;
                String result = PhongBUS.XoaP(selected.MaPhong);
                if (result == null)
                {
                    NotificationBox_Phong_timkiem.Text = "Xóa phòng thành công!";
                    NotificationBox_Phong_timkiem.Visible = true;
                    dtg_Phong_Timkiem.DataSource = new BindingList<PHONG>();

                }
                else
                {
                    NotificationBox_Phong_timkiem.Text = result;
                    NotificationBox_Phong_timkiem.Visible = true;
                }
            }
        }
        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_Phong_Timkiem_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_Phong_timkiem.Visible = false;
            // Xóa dữ liệu của datagrid
            dtg_Phong_Timkiem.DataSource = new BindingList<PHONG>();

            Control ctrl = TabPage_Child_Phong_Timkiem;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }
        #endregion

        #region Phòng - Thêm sửa
        //Button thêm
        private void btn_phong_them_Click_1(object sender, EventArgs e)
        {
            PHONGDTO p = new PHONGDTO();
            p.MaPhong = TextBox_Phong_Them_maP.Text;
            p.TenP = TextBox_Phong_Them_tenP.Text;
            p.ViTriP = TextBox_Phong_Them_viTri.Text;
            p.SoNguoiO = Int32.Parse(TextBox_Phong_Them_soN.Text);
            p.SoNguoiTD = Int32.Parse(TextBox_Phong_Them_SNTD.Text);
            String result = PhongBUS.ThemP(p);
            if (result == null)
            {
                NotificationBox_Phong_themsua.Text = "Thêm mới phòng thành công!";
                NotificationBox_Phong_themsua.Visible = true;

                BindingList<PHONG> dataSource = new BindingList<PHONG>();
                foreach (var item in PhongBUS.TimP(p))
                {
                    dataSource.Add(item);
                }
                dtg_Phong_Themsua.DataSource = dataSource;
            }
            else
            {
                NotificationBox_Phong_themsua.Text = result;
                NotificationBox_Phong_themsua.Visible = true;
            }
        }
        // button sửa
        private void btn_phong_them_sua_Click_1(object sender, EventArgs e)
        {
            PHONGDTO p = new PHONGDTO();
            p.MaPhong = TextBox_Phong_Them_maP.Text;
            p.TenP = TextBox_Phong_Them_tenP.Text;
            p.ViTriP = TextBox_Phong_Them_viTri.Text;
            p.SoNguoiO = Int32.Parse(TextBox_Phong_Them_soN.Text);
            p.SoNguoiTD = Int32.Parse(TextBox_Phong_Them_SNTD.Text);

            String result = PhongBUS.SuaP(p);
            if (result == null)
            {
                NotificationBox_Phong_themsua.Text = "Sửa phòng thành công!";
                NotificationBox_Phong_themsua.Visible = true;

                BindingList<PHONG> dataSource = new BindingList<PHONG>();
                foreach (var item in PhongBUS.TimP(p))
                {
                    dataSource.Add(item);
                }
                dtg_Phong_Themsua.DataSource = dataSource;
            }
            else
            {
                NotificationBox_Phong_themsua.Text = result;
                NotificationBox_Phong_themsua.Visible = true;
            }
        }
        //button xóa
        private void btn_phong_them_xoa_Click_1(object sender, EventArgs e)
        {
            if (dtg_Phong_Themsua.SelectedRows.Count == 0)
            {
                NotificationBox_Phong_themsua.Text = "Chưa có phòng nào được chọn để xóa!";
                NotificationBox_Phong_themsua.Visible = true;
            }
            else
            {
                PHONG selected = dtg_Phong_Themsua.SelectedRows[0].DataBoundItem as PHONG;
                String result = PhongBUS.XoaP(selected.MaPhong);
                if (result == null)
                {
                    NotificationBox_Phong_themsua.Text = "Xóa phòng thành công!";
                    NotificationBox_Phong_themsua.Visible = true;
                    dtg_Phong_Themsua.DataSource = new BindingList<PHONG>();
                }
                else
                {
                    NotificationBox_Phong_themsua.Text = result;
                    NotificationBox_Phong_themsua.Visible = true;
                }
            }
        }
        // Trả về page trắng khi chuyển tab
        private void TabPage_Child_Phong_ThemSua_Leave(object sender, EventArgs e)
        {
            NotificationBox_Phong_themsua.Visible = false;

            dtg_Phong_Themsua.DataSource = new BindingList<PHONG>();

            Control ctrl = TabPage_Child_Phong_ThemSua;

            ClearAllText(ctrl);
        }
        #endregion

        #region Tài sản- Tìm kiếm tài sản
        // Button tìm kiếm
        private void iTalk_Button_11_Click_1(object sender, EventArgs e)
        {
            TAISANDTO ts = new TAISANDTO();
            ts.MaTS = TextBox_Taisan_Timkiem_maTS.Text;
            ts.SoLuong = Int32.Parse(TextBox_Taisan_Timkiem_soLuong.Text);
            ts.TenTS = TextBox_Taisan_Timkiem_tenTS.Text;
            ts.TinhTrang = TextBox_Taisan_Timkiem_tinhTrang.Text;
            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<TAISAN> dataSource = new BindingList<TAISAN>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in TaiSanBUS.TimTS(ts))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_Taisan_timkiem.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_Taisan_timkiem.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dtg_Taisan_Timkiem.DataSource = dataSource;
            }
        }
        // Button sửa
        private void iTalk_Button_12_Click_1(object sender, EventArgs e)
        {
            if (dtg_Taisan_Timkiem.SelectedRows.Count == 0)
            {
                NotificationBox_Taisan_timkiem.Text = "Chưa có tài sản nào được chọn để sửa!";
                NotificationBox_Taisan_timkiem.Visible = true;
            }
            else
            {
                TAISAN selected = dtg_Taisan_Timkiem.SelectedRows[0].DataBoundItem as TAISAN;
                // code để chuyển dữ liệu sang tabpage thêm sửa
                TextBox_Taisan_Them_maTS.Text = selected.MaTS;
                TextBox_Taisan_Them_soLuong.Text = selected.SoLuong.ToString();
                TextBox_Taisan_Them_tenTS.Text = selected.TenTS;
                TextBox_Taisan_Them_tinhTrang.Text = selected.TinhTrang;
                // Hiện tất cả tài sản khi chuyển sang tabpage ThemSua
                BindingList<TAISAN> dataSource = new BindingList<TAISAN>();
                foreach (var item in TaiSanBUS.TimTatCaSV())
                {
                    dataSource.Add(item);
                }
                dtg_Taisan_ThemSua.DataSource = dataSource;
                TabControl_Children_TaiSan.SelectedTab = TabPage_Child_Taisan_ThemSua;
            }
        }
        // Button xóa
        private void iTalk_Button_13_Click_1(object sender, EventArgs e)
        {
            if (dtg_Taisan_Timkiem.SelectedRows.Count == 0)
            {
                NotificationBox_Taisan_timkiem.Text = "Chưa có tài sản nào được chọn để xóa!";
                NotificationBox_Taisan_timkiem.Visible = true;
            }
            else
            {
                TAISAN selected = dtg_Taisan_Timkiem.SelectedRows[0].DataBoundItem as TAISAN;
                String result = TaiSanBUS.XoaTS(selected.MaTS);
                if (result == null)
                {
                    NotificationBox_Taisan_timkiem.Text = "Xóa tài sản thành công!";
                    NotificationBox_Taisan_timkiem.Visible = true;
                    dtg_Taisan_Timkiem.DataSource = new BindingList<TAISAN>();

                }
                else
                {
                    NotificationBox_Taisan_timkiem.Text = result;
                    NotificationBox_Taisan_timkiem.Visible = true;
                }
            }
        }
        #endregion

        #region Tài sản- Thêm sửa tài sản
        //Button thêm
        private void btntaisan_them_Click(object sender, EventArgs e)
        {
            TAISANDTO ts = new TAISANDTO();
            ts.MaTS = TextBox_Taisan_Them_maTS.Text;
            ts.TenTS = TextBox_Taisan_Them_tenTS.Text;
            ts.SoLuong = Int32.Parse(TextBox_Taisan_Them_soLuong.Text);
            ts.TinhTrang = TextBox_Taisan_Them_tinhTrang.Text;
            String result = TaiSanBUS.ThemTS(ts);
            if (result == null)
            {
                NotificationBox_Taisan_themsua.Text = "Thêm mới tài sản thành công!";
                NotificationBox_Taisan_themsua.Visible = true;

                BindingList<TAISAN> dataSource = new BindingList<TAISAN>();
                foreach (var item in TaiSanBUS.TimTS(ts))
                {
                    dataSource.Add(item);
                }
                dtg_Taisan_ThemSua.DataSource = dataSource;
            }
            else
            {
                NotificationBox_Taisan_themsua.Text = result;
                NotificationBox_Taisan_themsua.Visible = true;
            }
        }
        //Button sửa
        private void btntaisan_them_sua_Click(object sender, EventArgs e)
        {
            TAISANDTO ts = new TAISANDTO();
            ts.MaTS = TextBox_Taisan_Them_maTS.Text;
            ts.TenTS = TextBox_Taisan_Them_tenTS.Text;
            ts.SoLuong = Int32.Parse(TextBox_Taisan_Them_soLuong.Text);
            ts.TinhTrang = TextBox_Taisan_Them_tinhTrang.Text;

            String result = TaiSanBUS.SuaTS(ts);
            if (result == null)
            {
                NotificationBox_Taisan_themsua.Text = "Sửa tài sản thành công!";
                NotificationBox_Taisan_themsua.Visible = true;

                BindingList<TAISAN> dataSource = new BindingList<TAISAN>();
                foreach (var item in TaiSanBUS.TimTS(ts))
                {
                    dataSource.Add(item);
                }
                dtg_Taisan_ThemSua.DataSource = dataSource;
            }
            else
            {
                NotificationBox_Taisan_themsua.Text = result;
                NotificationBox_Taisan_themsua.Visible = true;
            }
        }
        //Button xóa
        private void btntaisan_them_xoa_Click(object sender, EventArgs e)
        {
            if (dtg_Taisan_ThemSua.SelectedRows.Count == 0)
            {
                NotificationBox_Taisan_themsua.Text = "Không có tài sản nào được chọn để xóa";
                NotificationBox_Taisan_themsua.Visible = true;
            }
            else
            {
                TAISAN selected = dtg_Taisan_ThemSua.SelectedRows[0].DataBoundItem as TAISAN;
                String result = TaiSanBUS.XoaTS(selected.MaTS);
                if (result == null)
                {
                    NotificationBox_Taisan_themsua.Text = "Xóa tài sản thành công!";
                    NotificationBox_Taisan_themsua.Visible = true;
                    dtg_Taisan_ThemSua.DataSource = new BindingList<TAISAN>();

                }
                else
                {
                    NotificationBox_Taisan_themsua.Text = result;
                    NotificationBox_Taisan_themsua.Visible = true;
                }
            }
        }



        #endregion

        #region Các hàm phụ

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Hàm làm trắng textbox trong control
        private void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
        }

        #endregion

        //Lỡ tay click :v 
        #region Các hàm lỡ tay click

        private void monoFlat_ThemeContainer_Bright1_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small4_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small2_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_Label19_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_HeaderLabel9_Click(object sender, EventArgs e)
        {

        }

        private void monoFlat_NotificationBox3_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label20_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small11_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small13_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_Label21_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small12_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_Label22_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small14_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_Button_11_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Button_12_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Button_13_Click(object sender, EventArgs e)
        {

        }

        private void monoFlat_ControlBox_Bright1_Click(object sender, EventArgs e)
        {

        }

        private void monoFlat_Label2_Click(object sender, EventArgs e)
        {

        }

        private void monoFlat_Label1_Click(object sender, EventArgs e)
        {

        }
        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }
        private void iTalk_Label1_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small10_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small9_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small8_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small7_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small6_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_HeaderLabel7_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label12_Click(object sender, EventArgs e)
        {

        }

        private void btn_phong_tk_xoa_Click(object sender, EventArgs e)
        {

        }

        private void btn_phong_tk_sua_Click(object sender, EventArgs e)
        {

        }

        private void btn_phong_timkiem_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label11_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label10_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label9_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label7_Click(object sender, EventArgs e)
        {

        }

        private void btn_phong_them_xoa_Click(object sender, EventArgs e)
        {

        }

        private void btn_phong_them_sua_Click(object sender, EventArgs e)
        {

        }

        private void btn_phong_them_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_HeaderLabel8_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small3_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small5_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small1_TextChanged(object sender, EventArgs e)
        {

        }

        private void iTalk_Label18_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label17_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label16_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label15_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label14_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label13_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Label20_Click_1(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small11_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void iTalk_HeaderLabel9_Click_1(object sender, EventArgs e)
        {

        }

        private void monoFlat_NotificationBox3_Click_1(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small13_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small12_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void iTalk_Label21_Click_1(object sender, EventArgs e)
        {

        }

        private void iTalk_Label22_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void iTalk_Label23_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_TextBox_Small14_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void iTalk_HeaderLabel11_Click(object sender, EventArgs e)
        {

        }






















        #endregion


    }
}
