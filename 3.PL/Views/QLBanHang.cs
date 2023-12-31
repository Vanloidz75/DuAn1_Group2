﻿using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using ZXing;
using _2.BUS.IServices;
using _2.BUS.Services;
using System.Drawing.Imaging;
using _2.BUS.ViewModels;
using System.Globalization;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using System.Runtime.CompilerServices;
using RJCodeAdvance.RJControls;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
using _2.BUS.Utilities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Font = iTextSharp.text.Font;
using Rectangle = System.Drawing.Rectangle;

namespace _3.PL.Views
{
    public partial class QLBanHang : UserControl
    {
        INhanVienServices _iNhanVienServices;
        IDoChoiServices _iDoChoiServices;
        IThucAnServices _iThucAnServices;
        IThuCungServices _iThuCungServices;
        IKhachHangServices _iKhachHangServices;
        IHoaDonServices _iHoaDonServices;
        IHDTCCTServices _iHoaDonChiTietServices;
        IHDDCCTServices _iHDDCCTServices;
        IHDTACTServices _iHDTACTServices;
        NhanVienView nv;
        List<HoaDonChiTietView> hdct;

        Guid id;
        Guid idhd;
        Guid idsp;
        decimal thanhtien;
        decimal giagoc;
        Validates vld;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        public QLBanHang(NhanVienView nhanVienView)
        {
            InitializeComponent();
            _iNhanVienServices = new NhanVienServices();
            _iDoChoiServices = new DoChoiServices();
            _iThucAnServices = new ThucAnServices();
            _iThuCungServices = new ThuCungServices();
            _iKhachHangServices = new KhachHangServices();
            _iHoaDonServices = new HoaDonServices();
            _iHoaDonChiTietServices = new HDTCCTServices();
            _iHDDCCTServices = new HDDCCTServices();
            _iHDTACTServices = new HDTACTServices();
            hdct = new List<HoaDonChiTietView>();
            nv = nhanVienView;
            vld = new Validates();
        }
        public void GetNhanVien(NhanVienView nhanVienView)
        {
            nv = nhanVienView;
            MessageBox.Show(nhanVienView.Ten);
        }

        private void QLGioHang_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadVideo();
        }

        private void LoadVideo()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filterInfoCollection)
                cbb_Camera.Items.Add(device.Name);
            cbb_Camera.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cbb_Camera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();

        }
        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null)
            {
                tbt_Barcode.Invoke(new MethodInvoker(delegate ()
                {
                    tbt_Barcode.Text = result.ToString();
                }));
            }
            ptb_Scan.Image = bitmap;
        }
        public void StopProgressBar()
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Stop();
            }
        }
        public string CreateKey()
        {
            string ma = "HD";
            DateTime dateTime = DateTime.Now;
            string d = String.Format($"{dateTime.Day}{dateTime.Month}{dateTime.Year}_{dateTime.Hour}{dateTime.Minute}{dateTime.Second}");
            ma = ma + d;
            return ma;
        }
        private string ChangeFormatMoney(decimal value)
        {
            return string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}", value);
        }
        public Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = System.Drawing.Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

        private void LoadData()
        {
            dtgv_HoaDon.Columns.Clear();
            flpn_ThuCung.Controls.Clear();
            flpn_ThucAn.Controls.Clear();
            flpn_DoChoi.Controls.Clear();
            //Hóa đơn
            dtgv_HoaDon.ColumnCount = 5;
            dtgv_HoaDon.Columns[0].Name = "ID";
            dtgv_HoaDon.Columns[0].Visible = false;
            dtgv_HoaDon.Columns[1].Name = "Mã hóa đơn";
            dtgv_HoaDon.Columns[2].Name = "Tên nhân viên";
            dtgv_HoaDon.Columns[3].Name = "Ngày tạo";
            dtgv_HoaDon.Columns[4].Name = "Trạng thái";
            foreach (var x in _iThuCungServices.GetAll())
            {
                Image image = resizeImage(90, 50, x.Image);
                DoubleClickButton btnTC = new DoubleClickButton();
                btnTC.Image = image;
                btnTC.Height = 100;
                btnTC.Width = 100;
                btnTC.TextImageRelation = TextImageRelation.ImageAboveText;
                btnTC.Text = x.Ten;
                btnTC.Tag = string.Format(x.IdTCCT.ToString());
                btnTC.BackColor = SystemColors.Control;
                btnTC.ForeColor = Color.Black;
                btnTC.DoubleClick += new EventHandler(btnTC_Click);
                if(x.SoLuong == 0)
                {
                    btnTC.Enabled = false;
                }
                this.flpn_ThuCung.Controls.Add(btnTC);
            }
            foreach (var x in _iThucAnServices.GetAll())
            {
                Image image = resizeImage(90, 50, x.Image);
                DoubleClickButton btnTA = new DoubleClickButton();
                btnTA.Image = image;
                btnTA.Height = 100;
                btnTA.Width = 100;
                btnTA.TextImageRelation = TextImageRelation.ImageAboveText;
                btnTA.Text = x.Ten;
                btnTA.Tag = string.Format(x.Id.ToString());
                btnTA.BackColor = SystemColors.Control;
                btnTA.ForeColor = Color.Black;
                btnTA.DoubleClick += new EventHandler(btnTA_Click);
                if (x.SoLuongTon == 0)
                {
                    btnTA.Enabled = false;
                }
                this.flpn_ThucAn.Controls.Add(btnTA);
            }
            foreach (var x in _iDoChoiServices.GetAll())
            {
                Image image = resizeImage(90, 50, x.Image);
                DoubleClickButton btnDC = new DoubleClickButton();
                btnDC.Image = image;
                btnDC.Height = 100;
                btnDC.Width = 100;
                btnDC.TextImageRelation = TextImageRelation.ImageAboveText;
                btnDC.Text = x.Ten;
                btnDC.Tag = string.Format(x.Id.ToString());
                btnDC.BackColor = SystemColors.Control;
                btnDC.ForeColor = Color.Black;
                if (x.SoLuongTon == 0)
                {
                    btnDC.Enabled = false;
                }
                btnDC.DoubleClick += new EventHandler(btnDC_Click);
                
                this.flpn_DoChoi.Controls.Add(btnDC);
            }
            foreach (var x in _iHoaDonServices.GetAll())
            {
                if (x.TinhTrang == 0)
                {
                    dtgv_HoaDon.Rows.Add(x.Id, x.Ma, x.TenNv, x.NgayTao, x.TinhTrang == 0 ? "Chưa thanh toán" : "Đã thanh toán");
                }
            }

        }
        private void btnTC_Click(object sender, EventArgs e)
        {

            id = Guid.Parse((string)((Button)sender).Tag);
            var tc = _iThuCungServices.GetAll().FirstOrDefault(t => t.IdTCCT == id);

            if (tc.SoLuong <= 0)
            {
                MessageBox.Show("Số lượng không đủ");
            }
            if (hdct.FirstOrDefault(x => x.IdSp == tc.IdTCCT) != null)
            {
                if(hdct.FirstOrDefault(x => x.IdSp == tc.IdTCCT).SoLuong == tc.SoLuong)
                {
                    MessageBox.Show("Số lượng không đủ");
                }
                hdct.FirstOrDefault(x => x.IdSp == tc.IdTCCT).SoLuong += 1;
                LoadHDCT();
            }
            else
            {
                HoaDonChiTietView hdctv = new HoaDonChiTietView();
                hdctv.Id = Guid.NewGuid();
                hdctv.IdSp = tc.IdTCCT;
                hdctv.IdHoaDon = Guid.Empty;
                hdctv.SoLuong = 1;
                hdctv.DonGia = tc.GiaBan;
                hdctv.TongTien = tc.GiaBan * hdctv.SoLuong;
                hdctv.Ten = tc.Ten;
                hdct.Add(hdctv);
                LoadHDCT();
            }
        }
        private void btnTA_Click(object sender, EventArgs e)
        {

            id = Guid.Parse((string)((Button)sender).Tag);
            var x = _iThucAnServices.GetAll().FirstOrDefault(t => t.Id == id);
            if (x.SoLuongTon <= 0)
            {
                MessageBox.Show("Số lượng không đủ");
            }
            else if (hdct.FirstOrDefault(x => x.IdSp == x.Id) != null)
            {
                hdct.FirstOrDefault(x => x.IdSp == x.Id).SoLuong += 1;
                LoadHDCT();
            }
            else
            {
                HoaDonChiTietView hdctv = new HoaDonChiTietView();
                hdctv.Id = Guid.NewGuid();
                hdctv.IdSp = x.Id;
                hdctv.IdHoaDon = Guid.Empty;
                hdctv.SoLuong = 1;
                hdctv.DonGia = x.GiaBan;
                hdctv.TongTien = x.GiaBan * hdctv.SoLuong;
                hdctv.Ten = x.Ten;
                hdct.Add(hdctv);
                LoadHDCT();
            }
        }
        private void btnDC_Click(object sender, EventArgs e)
        {

            id = Guid.Parse((string)((Button)sender).Tag);
            var x = _iDoChoiServices.GetAll().FirstOrDefault(t => t.Id == id);
            if (x.SoLuongTon <= 0)
            {
                MessageBox.Show("Số lượng không đủ");
            }
            else if (hdct.FirstOrDefault(x => x.IdSp == x.Id) != null)
            {
                hdct.FirstOrDefault(x => x.IdSp == x.Id).SoLuong += 1;
                LoadHDCT();
            }
            else
            {
                HoaDonChiTietView hdctv = new HoaDonChiTietView();
                hdctv.Id = Guid.NewGuid();
                hdctv.IdSp = x.Id;
                hdctv.IdHoaDon = Guid.Empty;
                hdctv.SoLuong = 1;
                hdctv.DonGia = x.GiaBan;
                hdctv.Ten = x.Ten;
                hdctv.TongTien = x.GiaBan * hdctv.SoLuong;
                hdct.Add(hdctv);
                LoadHDCT();
            }

        }

        private void LoadHDCT()
        {
            thanhtien = 0;
            dtgv_HoaDonCt.Rows.Clear();
            int sc = dtgv_HoaDon.Rows.Count;
            int i = 0;
            foreach (var x in hdct)
            {

                dtgv_HoaDonCt.Rows.Add(x.IdSp, x.Ten, x.SoLuong, ChangeFormatMoney(x.DonGia), ChangeFormatMoney(x.TongTien));
                thanhtien += x.TongTien;
                i++;
            }
            dtgv_HoaDonCt.Rows[i].Cells[4].Value = ChangeFormatMoney(thanhtien);
            giagoc = thanhtien;
        }

        private void dtgv_HoaDonCt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgv_HoaDonCt.CurrentCell != null && dtgv_HoaDonCt.CurrentCell.Value != null)
            {
                idsp = Guid.Parse(dtgv_HoaDonCt.CurrentRow.Cells[0].Value.ToString());
                ShowCongTru();
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (hdct.Count != 0)
            {
                if (idhd != Guid.Empty)
                {
                    var idsp = dtgv_HoaDonCt.CurrentRow.Cells[0].Value.ToString();
                    var dc = _iHDDCCTServices.GetAll().FirstOrDefault(x => x.IdHoaDon == idhd && x.IdSp.ToString() == idsp);
                    var ta = _iHDTACTServices.GetAll().FirstOrDefault(x => x.IdHoaDon == idhd && x.IdSp.ToString() == idsp);
                    var tc = _iHoaDonChiTietServices.GetAll().FirstOrDefault(x => x.IdHoaDon == idhd && x.IdSp.ToString() == idsp);
                    if (dc != null)
                    {
                        var z = _iDoChoiServices.GetAll().FirstOrDefault(x => x.Id.ToString() == idsp);
                        z.SoLuongTon += dc.SoLuong;
                        _iDoChoiServices.Update(z);
                        _iHDDCCTServices.Delete(_iHDDCCTServices.GetAll().FirstOrDefault(x => x.IdHoaDon == idhd));
                    }
                    if (ta != null)
                    {
                        var z = _iThucAnServices.GetAll().FirstOrDefault(x => x.Id.ToString() == idsp);
                        z.SoLuongTon += ta.SoLuong;
                        _iThucAnServices.Update(z);
                        _iHDTACTServices.Delete(_iHDTACTServices.GetAll().FirstOrDefault(x => x.IdHoaDon == idhd));
                    }
                    if (tc != null)
                    {
                        var z = _iThuCungServices.GetAll().FirstOrDefault(x => x.IdTCCT.ToString() == idsp);
                        z.SoLuong += tc.SoLuong;
                        _iThuCungServices.Update(z);
                        _iHoaDonChiTietServices.Delete(_iHoaDonChiTietServices.GetAll().FirstOrDefault(x => x.IdHoaDon == idhd));
                    }
                }
                hdct.RemoveAt(dtgv_HoaDonCt.CurrentRow.Index);
                LoadHDCT();
            }
        }
        private void btn_TaoHoaDon_Click(object sender, EventArgs e)
        {

            var khvl = _iKhachHangServices.GetAll().FirstOrDefault(x => x.Ma == "KH00");

            if (hdct != null)
            {
                var x = new HoaDonView()
                {
                    Id = Guid.NewGuid(),
                    Ma = CreateKey(),
                    NgayTao = DateTime.Now,
                    NgayThanhToan = DateTime.Now,
                    NgayGiaoHang = DateTime.Now,
                    NgayNhan = DateTime.Now,
                    TienCoc = 0,
                    TienShip = 0,
                    TenNguoiNhan = khvl.HoVaTen,
                    IdKhachHang = khvl.Id,
                    IdNhanVien = nv.Id,
                    DiaChi = khvl.DiaChi,
                    TinhTrang = 0,
                    Sdt = khvl.Sdt,
                    PhanTramGiamGia = 0
                };
                if (_iHoaDonServices.Add(x))
                {
                    foreach (var i in hdct)
                    {
                        if (i != null)
                        {
                            if (_iDoChoiServices.GetAll().FirstOrDefault(x => x.Id == i.IdSp) != null)
                            {
                                i.IdHoaDon = x.Id;
                                _iHDDCCTServices.Add(i);
                                var z = _iDoChoiServices.GetAll().FirstOrDefault(x => x.Id == i.IdSp);
                                z.SoLuongTon -= i.SoLuong;
                                _iDoChoiServices.Update(z);
                            }
                            else if (_iThucAnServices.GetAll().FirstOrDefault(x => x.Id == i.IdSp) != null)
                            {
                                i.IdHoaDon = x.Id;
                                _iHDTACTServices.Add(i);
                                var z = _iThucAnServices.GetAll().FirstOrDefault(x => x.Id == i.IdSp);
                                z.SoLuongTon -= i.SoLuong;
                                _iThucAnServices.Update(z);
                            }
                            else
                            {
                                i.IdHoaDon = x.Id;
                                _iHoaDonChiTietServices.Add(i);
                                var z = _iThuCungServices.GetAll().FirstOrDefault(x => x.IdTCCT == i.IdSp);
                                z.SoLuong -= i.SoLuong;
                                _iThuCungServices.Update(z);
                            }

                        }
                    }
                    Clear();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Chưa có sản phẩm");
            }

        }

        private void dtgv_HoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgv_HoaDon.CurrentCell != null && dtgv_HoaDon.CurrentCell.Value != null)
            {
                hdct.Clear();
                idhd = Guid.Parse(dtgv_HoaDon.CurrentRow.Cells[0].Value.ToString());
                var x = _iHoaDonServices.GetAll().FirstOrDefault(x => x.Id == idhd);
                tbt_MaHoaDon.Texts = x.Ma;
                tbt_NgayTao.Texts = x.NgayTao.ToString();
                tbt_TenNhanVien.Texts = x.TenNv;
                tbt_PTGiamGia.Texts = x.PhanTramGiamGia.ToString();
                tbt_TienKhachDua.Texts = x.TienCoc.ToString();
                tbt_TienChuyenKhoan.Texts = x.TienShip.ToString();
                var dcct = _iHDDCCTServices.GetAll().Where(p => p.IdHoaDon == idhd).ToList();
                foreach (var dc in dcct)
                {
                    hdct.Add(dc);
                }

                var tact = _iHDTACTServices.GetAll().Where(p => p.IdHoaDon == idhd);
                foreach (var ta in tact)
                {
                    hdct.Add(ta);
                }
                var tcct = _iHoaDonChiTietServices.GetAll().Where(p => p.IdHoaDon == idhd);
                foreach (var tc in tcct)
                {
                    hdct.Add(tc);
                }
                thanhtien = 0;
                LoadHDCT();
                tbt_TongTien.Texts = ChangeFormatMoney(thanhtien);
                if (x.TinhTrang == 1)
                {
                    tbt_TienKhachDua.Enabled = false;
                    tbt_PTGiamGia.Enabled = false;
                    btn_ThanhToan.Enabled = false;
                }
                else
                {
                    tbt_TienKhachDua.Enabled = true;
                    tbt_PTGiamGia.Enabled = true;
                    btn_ThanhToan.Enabled = true;
                    btn_Sua.Enabled = true;
                }
            }
        }
        private void XuatHoaDon()
        {
            PdfPTable pdfTable = new PdfPTable(dtgv_HoaDonCt.ColumnCount - 1);
            BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIAL.TTF", BaseFont.IDENTITY_H, true);
            Font normalFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font headerFont = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            Font foooterFont = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.RED);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 90;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            foreach (DataGridViewColumn column in dtgv_HoaDonCt.Columns)
            {
                if (column.Index != 0)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, normalFont));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    cell.Border = 0;
                    cell.PaddingLeft = 10;
                    pdfTable.AddCell(cell);
                }
            }
            foreach (DataGridViewRow row in dtgv_HoaDonCt.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex != 0)
                    {
                        if (cell.Value != null)
                        {
                            PdfPCell pdfcell = new PdfPCell(new Phrase(cell.Value.ToString(), normalFont));
                            pdfcell.Border = 0;
                            pdfcell.PaddingLeft = 10;
                            pdfTable.AddCell(pdfcell);
                        }

                    }
                }

            }

            var path = "E:\\Pic\\HoaDon\\";
            var x = _iHoaDonServices.GetAll().FirstOrDefault(x => x.Id == idhd);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream stream = new FileStream(path + $"{x.Ma}.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                //Create a base font object making sure to specify IDENTITY-H
                Paragraph header = new Paragraph("Hóa đơn thanh toán", headerFont);
                header.Alignment = Element.ALIGN_CENTER;
                Paragraph ma = new Paragraph($"Mã hóa đơn: {x.Ma}", normalFont);
                ma.Alignment = Element.ALIGN_LEFT;
                Paragraph ngaythanhtoan = new Paragraph($"Ngày thanh toán: {x.NgayThanhToan}", normalFont);
                ngaythanhtoan.Alignment = Element.ALIGN_LEFT;
                pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                Paragraph phanTramGiamGia = new Paragraph($"Giảm giá: ", normalFont);
                Paragraph giatriptgg = new Paragraph($"{x.PhanTramGiamGia}%", normalFont);

                Paragraph tongTien = new Paragraph($"Tổng cộng:", headerFont);
                Paragraph giatritt = new Paragraph($"{ChangeFormatMoney(thanhtien)}", headerFont);

                Paragraph khachdua = new Paragraph($"Khách đưa:", normalFont);
                Paragraph giatrikd = new Paragraph($"{ChangeFormatMoney(x.TienCoc)}", normalFont);
                Paragraph khachchuyenkhoan = new Paragraph($"Khách chuyển khoản:", normalFont);
                Paragraph giatrikck = new Paragraph($"{ChangeFormatMoney(x.TienShip)}", normalFont);
                Paragraph trakhach = new Paragraph($"Trả khách:", normalFont);
                Paragraph giatritk = new Paragraph($"{ChangeFormatMoney(x.TienCoc + x.TienShip - thanhtien)}", normalFont);

                PdfPTable table = new PdfPTable(4);

                PdfPCell cell = new PdfPCell(phanTramGiamGia);
                cell.Colspan = 3;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(giatriptgg);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(tongTien);
                cell.Colspan = 3;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(giatritt);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(khachdua);
                cell.Colspan = 3;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(giatrikd);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(khachchuyenkhoan);
                cell.Colspan = 3;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(giatrikck);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(trakhach);
                cell.Colspan = 3;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(giatritk);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                table.AddCell(cell);

                Paragraph camon = new Paragraph("XIN CẢM ƠN - HẸN GẶP LẠI!!", foooterFont);
                camon.Alignment = Element.ALIGN_CENTER;
                Paragraph chia = new Paragraph("--------------------------------------------------------------", normalFont);
                chia.Alignment = Element.ALIGN_CENTER;
                chia.SpacingAfter = 10;
                //Create a specific font object
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(header);
                pdfDoc.Add(ma);
                pdfDoc.Add(ngaythanhtoan);
                pdfDoc.Add(chia);
                pdfDoc.Add(pdfTable);
                pdfDoc.Add(chia);
                pdfDoc.Add(table);
                pdfDoc.Add(chia);
                pdfDoc.Add(camon);
                pdfDoc.Close();
            }
            MessageBox.Show("Xuất hóa đơn thành công");
        }
        private void btn_ThanhToan_Click(object sender, EventArgs e)
        {
            if (tbt_PTGiamGia.Texts != "" && tbt_TienKhachDua.Texts != "")
            {
                if (Convert.ToDecimal(tbt_TienKhachDua.Texts)+Convert.ToDecimal(tbt_TienChuyenKhoan.Texts) >= thanhtien)
                {
                    var hd = _iHoaDonServices.GetAll().FirstOrDefault(x => x.Id == idhd);
                    if (hd.TinhTrang == 0)
                    {
                        hd.PhanTramGiamGia = Convert.ToDecimal(tbt_PTGiamGia.Texts);
                        hd.TinhTrang = 1;
                        hd.TienCoc = Convert.ToDecimal(tbt_TienKhachDua.Texts);
                        hd.TienShip = Convert.ToDecimal(tbt_TienChuyenKhoan.Texts);
                        _iHoaDonServices.Update(hd);
                        MessageBox.Show("Thanh toán thành công");
                        DialogResult result = MessageBox.Show("Bạn có muốn in hóa đơn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(result == DialogResult.Yes)
                        {
                            XuatHoaDon();
                        }
                        LoadData();
                        Clear();
                    }
                    else
                    {
                        MessageBox.Show("Hóa đơn đã thanh toán");
                    }

                }
                else
                {
                    MessageBox.Show("Khách đưa chưa đủ tiền");
                }

            }
            else
            {
                MessageBox.Show("Chưa nhập tiền khách đưa");
            }
        }
        private void tbt_PTGiamGia__TextChanged(object sender, EventArgs e)
        {
            if (tbt_PTGiamGia.Texts != "")
            {
                var ptgg = Convert.ToDecimal(tbt_PTGiamGia.Texts);
                thanhtien = giagoc * (1 - ptgg / 100);
                tbt_TongTien.Texts = ChangeFormatMoney(thanhtien);
            }
            else
            {
                tbt_TongTien.Texts = ChangeFormatMoney(giagoc);
            }
        }

        private void tbt_TienKhachDua__TextChanged(object sender, EventArgs e)
        {
            lbl_errTkd.Text = vld.CheckNumber(tbt_TienKhachDua.Texts);
            if (lbl_errTkd.Text == "")
            {
                if (tbt_TienKhachDua.Texts != "" && tbt_TienChuyenKhoan.Texts != "")
                {
                    var tkd = Convert.ToDecimal(tbt_TienKhachDua.Texts);
                    var tck = Convert.ToDecimal(tbt_TienChuyenKhoan.Texts);
                    var tienthua = tkd + tck - thanhtien;
                    tbt_TienThua.Texts = ChangeFormatMoney(tienthua);
                }
            }
        }
        private string ReadBarcodeFormImage(string path)
        {
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode((Bitmap)Bitmap.FromFile(path));
            if (result != null)
            {
                return result.ToString();
            }
            return "";
        }
        private void tbt_Barcode_TextChanged(object sender, EventArgs e)
        {
            int count = 0;
            if (tbt_Barcode.Text != "")
            {
                var dcall = _iDoChoiServices.GetAll();

                foreach (var d in dcall)
                {
                    if (tbt_Barcode.Text == ReadBarcodeFormImage(d.Barcode))
                    {
                        if (d.SoLuongTon > 0)
                        {
                            HoaDonChiTietView hdctv = new HoaDonChiTietView();
                            hdctv.Id = Guid.NewGuid();
                            hdctv.IdSp = d.Id;
                            hdctv.SoLuong = 1;
                            hdctv.DonGia = d.GiaBan;
                            hdctv.Ten = d.Ten;
                            hdctv.TongTien = d.GiaBan * 1;
                            hdct.Add(hdctv);
                            LoadHDCT();
                            count++;
                        }
                        else
                        {
                            MessageBox.Show("Không đủ số lượng");
                        }
                    }
                }
                var taall = _iThucAnServices.GetAll();
                foreach (var t in taall)
                {
                    if (tbt_Barcode.Text == ReadBarcodeFormImage(t.Barcode))
                    {
                        if (t.SoLuongTon > 0)
                        {
                            HoaDonChiTietView hdctv = new HoaDonChiTietView();
                            hdctv.Id = Guid.NewGuid();
                            hdctv.IdSp = t.Id;
                            hdctv.SoLuong = 1;
                            hdctv.DonGia = t.GiaBan;
                            hdctv.Ten = t.Ten;
                            hdctv.TongTien = t.GiaBan * 1;
                            hdct.Add(hdctv);
                            LoadHDCT();
                            count++;
                        }
                        else
                        {
                            MessageBox.Show("Không đủ số lượng");
                        }
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("Không có sản phẩm nào");
                }
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
           
            if (idhd != Guid.Empty)
            {
                Guid idn = idhd;
                foreach (var hd in hdct.ToList())
                {
                    if (hd != null)
                    {
                        if (hd.IdHoaDon == Guid.Empty)
                        {
                            hd.IdHoaDon = idn;
                            if (_iDoChoiServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp) != null)
                            {
                                _iHDDCCTServices.Add(hd);
                                var z = _iDoChoiServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp);
                                z.SoLuongTon -= hd.SoLuong;
                                _iDoChoiServices.Update(z);
                                Clear();
                            }
                            else if (_iThucAnServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp) != null)
                            {
                                _iHDTACTServices.Add(hd);
                                var z = _iThucAnServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp);
                                z.SoLuongTon -= hd.SoLuong;
                                _iThucAnServices.Update(z);
                                Clear();
                            }
                            else
                            {
                                _iHoaDonChiTietServices.Add(hd);
                                var z = _iThuCungServices.GetAll().FirstOrDefault(x => x.IdTCCT == hd.IdSp);
                                z.SoLuong -= hd.SoLuong;
                                _iThuCungServices.Update(z);
                                Clear();
                            }
                        }
                        else
                        {
                            if (_iDoChoiServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp) != null)
                            {

                                _iHDDCCTServices.Update(hd);
                                var z = _iDoChoiServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp);
                                _iDoChoiServices.Update(z);
                                Clear();
                            }
                            else if (_iThucAnServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp) != null)
                            {
                                _iHDTACTServices.Update(hd);
                                var z = _iThucAnServices.GetAll().FirstOrDefault(x => x.Id == hd.IdSp);
                                _iThucAnServices.Update(z);
                                Clear();
                            }
                            else
                            {

                                _iHoaDonChiTietServices.Update(hd);
                                var z = _iThuCungServices.GetAll().FirstOrDefault(x => x.IdTCCT == hd.IdSp);
                                _iThuCungServices.Update(z);
                                Clear();
                            }
                        }
                    }
                }
            }
        }

        private void Clear()
        {
            hdct.Clear();
            idhd = Guid.Empty;
            LoadHDCT();
            tbt_MaHoaDon.Texts = "";
            tbt_NgayTao.Texts = "";
            tbt_PTGiamGia.Texts = "";
            tbt_TenNhanVien.Texts = "";
            tbt_TienKhachDua.Texts = "";
            tbt_TienThua.Texts = "";
            tbt_TongTien.Texts = "";
            tbt_Barcode.Text = "";
            tbt_TienChuyenKhoan.Texts = "";
            btn_Sua.Enabled = false;
        }
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void ShowCongTru()
        {
            var dc = _iDoChoiServices.GetAll().FirstOrDefault(dc => dc.Id == idsp);
            var ta = _iThucAnServices.GetAll().FirstOrDefault(ta => ta.Id == idsp);
            var tc = _iThuCungServices.GetAll().FirstOrDefault(tc => tc.IdTCCT == idsp);
            var x = hdct.FirstOrDefault(x => x.IdSp == idsp);
            if (x.SoLuong == 0)
            {
                btn_TruSl.Enabled = false;
            }
            else
            {
                btn_TruSl.Enabled = true;
            }
            if (dc != null)
            {
                if (dc.SoLuongTon == x.SoLuong)
                {
                    btn_CongSl.Enabled = false;
                }
                else
                {
                    btn_CongSl.Enabled = true;
                }
            }

            if (ta != null)
            {
                if (ta.SoLuongTon == x.SoLuong)
                {
                    btn_CongSl.Enabled = false;
                }
                else
                {
                    btn_CongSl.Enabled = true;
                }
            }
            if (tc != null)
            {
                if (tc.SoLuong == x.SoLuong)
                {
                    btn_CongSl.Enabled = false;
                }
                else
                {
                    btn_CongSl.Enabled = true;
                }
            }
        }
        private void btn_CongSl_Click(object sender, EventArgs e)
        {
            if (idsp != Guid.Empty)
            {
                var x = hdct.FirstOrDefault(x => x.IdSp == idsp);
                x.SoLuong += 1;
                x.TongTien = x.DonGia * x.SoLuong;
                LoadHDCT();
                ShowCongTru();
            }

        }

        private void btn_TruSl_Click(object sender, EventArgs e)
        {
            if (idsp != Guid.Empty)
            {
                var x = hdct.FirstOrDefault(x => x.IdSp == idsp);
                x.SoLuong -= 1;
                x.TongTien = x.DonGia * x.SoLuong;
                LoadHDCT();
                ShowCongTru();
            }
        }

        private void tbt_TienChuyenKhoan__TextChanged(object sender, EventArgs e)
        {
            lbl_errTck.Text = vld.CheckNumber(tbt_TienChuyenKhoan.Texts);
            if (lbl_errTck.Text == "")
            {
                if (tbt_TienKhachDua.Texts != "" && tbt_TienChuyenKhoan.Texts != "")
                {
                    var tkd = Convert.ToDecimal(tbt_TienKhachDua.Texts);
                    var tck = Convert.ToDecimal(tbt_TienChuyenKhoan.Texts);
                    var tienthua = tkd + tck - thanhtien;
                    tbt_TienThua.Texts = ChangeFormatMoney(tienthua);
                }
            }
        }

        private void QLBanHang_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Stop();
            }
        }
    }
}
