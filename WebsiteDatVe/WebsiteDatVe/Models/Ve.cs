namespace WebsiteDatVe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ve")]
    public partial class Ve
    {
        [Key]
        [StringLength(50)]
        public string MaVe { get; set; }

        public long? MaChuyenBay { get; set; }

        [StringLength(50)]
        public string HangVe { get; set; }

        [StringLength(4)]
        public string SoGhe { get; set; }

        public long? MaKhachHang { get; set; }

        [StringLength(50)]
        public string TinhTrang { get; set; }

        public DateTime? NgayDat { get; set; }

        public virtual ChuyenBay ChuyenBay { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
