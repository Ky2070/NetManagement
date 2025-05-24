namespace QuanlyquanNet.ViewModel
{
    public class ReportMetadata
    {
        public string FileName { get; set; }      // Tên file PDF
        public string NguoiTao { get; set; }      // Username nhân viên
        public DateTime NgayTao { get; set; }
        public string TrangThai { get; set; }     // "ChoDuyet", "DaDuyet", "TuChoi"
        public string? LyDoTuChoi { get; set; }
    }
}
