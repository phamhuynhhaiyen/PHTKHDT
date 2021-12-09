using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebsiteDatVe.Models
{
    public partial class DatVeDB : DbContext
    {
        public DatVeDB()
            : base("name=DatVeDB")
        {
        }

        public virtual DbSet<ChuyenBay> ChuyenBays { get; set; }
        public virtual DbSet<HangBay> HangBays { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<MayBay> MayBays { get; set; }
        public virtual DbSet<SanBay> SanBays { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<Ve> Ves { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChuyenBay>()
                .Property(e => e.MaMayBay)
                .IsUnicode(false);

            modelBuilder.Entity<ChuyenBay>()
                .HasMany(e => e.Ves)
                .WithOptional(e => e.ChuyenBay)
                .WillCascadeOnDelete();

            modelBuilder.Entity<HangBay>()
                .HasMany(e => e.MayBays)
                .WithOptional(e => e.HangBay)
                .WillCascadeOnDelete();

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.CMND)
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .HasMany(e => e.Ves)
                .WithOptional(e => e.KhachHang)
                .WillCascadeOnDelete();

            modelBuilder.Entity<MayBay>()
                .Property(e => e.MaMayBay)
                .IsUnicode(false);

            modelBuilder.Entity<MayBay>()
                .HasMany(e => e.ChuyenBays)
                .WithOptional(e => e.MayBay)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Quyen)
                .IsFixedLength();

            modelBuilder.Entity<Ve>()
                .Property(e => e.MaVe)
                .IsUnicode(false);

            modelBuilder.Entity<Ve>()
                .Property(e => e.SoGhe)
                .IsUnicode(false);
        }
    }
}
