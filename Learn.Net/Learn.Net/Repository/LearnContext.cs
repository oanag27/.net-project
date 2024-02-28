using System;
using System.Collections.Generic;
using Learn.Net.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Learn.Net.Repository;

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

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductimage> TblProductimages { get; set; }

    public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }
    //not required as we have added the apiconnection in the appsettings.json
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=OMG\\MSSQLSERVER01;Database=learn;Trusted_Connection=true;TrustServerCertificate=True;encrypt=false");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
