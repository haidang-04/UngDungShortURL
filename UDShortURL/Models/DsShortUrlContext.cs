using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UDShortURL.Models;

public partial class DsShortUrlContext : DbContext
{
    public DsShortUrlContext()
    {
    }

    public DsShortUrlContext(DbContextOptions<DsShortUrlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DsShortUrl> ShortUrls { get; set; }
    public object UDShortURLs { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=14.0.22.12;Initial Catalog=dbshort;Persist Security Info=True;User ID=tn;Password=@Abc123456;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DsShortUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shortUrl__3214EC076DF55B4F");

            entity.ToTable("shortUrl");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LongUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RedirectCount).HasDefaultValue(0);
            entity.Property(e => e.ShortUrl1)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
