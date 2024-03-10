using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Learn.Net.Repository.Models;

public partial class LearnContext : DbContext
{
    public LearnContext()
    {
    }

    public LearnContext(DbContextOptions<LearnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblOtpManager> TblOtpManagers { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductimage> TblProductimages { get; set; }

    public virtual DbSet<TblPwdManger> TblPwdMangers { get; set; }

    public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolemenumap> TblRolemenumaps { get; set; }

    public virtual DbSet<TblRolepermission> TblRolepermissions { get; set; }

    public virtual DbSet<TblSubtable> TblSubtables { get; set; }

    public virtual DbSet<TblTempuser> TblTempusers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=OMG\\MSSQLSERVER01;Initial Catalog=learn;Trusted_Connection=true;TrustServerCertificate=True;encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tbl_customer");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Creditlimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__tbl_menu__357D4CF821B4B761");

            entity.ToTable("tbl_menu");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TblOtpManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_otpM__3213E83FA65249BA");

            entity.ToTable("tbl_otpManager");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("datetime")
                .HasColumnName("createddate");
            entity.Property(e => e.Expiration)
                .HasColumnType("datetime")
                .HasColumnName("expiration");
            entity.Property(e => e.Otptext)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("otptext");
            entity.Property(e => e.Otptype)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("otptype");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tbl_product");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<TblProductimage>(entity =>
        {
            entity.ToTable("tbl_productimage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Productcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("productcode");
            entity.Property(e => e.Productimage)
                .HasColumnType("image")
                .HasColumnName("productimage");
        });

        modelBuilder.Entity<TblPwdManger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_pwdM__3213E83FF8D40E18");

            entity.ToTable("tbl_pwdManger");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Modifydate)
                .HasColumnType("datetime")
                .HasColumnName("modifydate");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblRefreshtoken>(entity =>
        {
            entity.HasKey(e => e.Userid);

            entity.ToTable("tbl_refreshtoken");

            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userid");
            entity.Property(e => e.Refreshtoken)
                .IsUnicode(false)
                .HasColumnName("refreshtoken");
            entity.Property(e => e.Tokenid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tokenid");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__tbl_role__357D4CF8D8F578C0");

            entity.ToTable("tbl_role");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TblRolemenumap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_role__3213E83F201B64E2");

            entity.ToTable("tbl_rolemenumap");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Menucode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("menucode");
            entity.Property(e => e.Userrole)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("userrole");
        });

        modelBuilder.Entity<TblRolepermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_role__3213E83F02CA2F95");

            entity.ToTable("tbl_rolepermission");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Haveadd).HasColumnName("haveadd");
            entity.Property(e => e.Havedelete).HasColumnName("havedelete");
            entity.Property(e => e.Haveedit).HasColumnName("haveedit");
            entity.Property(e => e.Haveview).HasColumnName("haveview");
            entity.Property(e => e.Menucode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("menucode");
            entity.Property(e => e.Userrole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userrole");
        });

        modelBuilder.Entity<TblSubtable>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__tbl_subt__357D4CF8257F89DB");

            entity.ToTable("tbl_subtable");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Menucode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("menucode");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TblTempuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_temp__3213E83FDE4AB766");

            entity.ToTable("tbl_tempuser");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("tbl_user");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
