using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace VirtualLibrary.Models;

public partial class VirtualLibraryDbContext : DbContext
{
    public VirtualLibraryDbContext()
    {
    }

    public VirtualLibraryDbContext(DbContextOptions<VirtualLibraryDbContext> options)
        : base(options)
    { 
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleCopy> ArticleCopies { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCopy> BookCopies { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Magazine> Magazines { get; set; }

    public virtual DbSet<MagazineArticle> MagazineArticles { get; set; }

    public virtual DbSet<MagazineCopy> MagazineCopies { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable(t => t.HasTrigger("delete_MagazineArticle"));

            entity.HasKey(e => e.Id).HasName("PK__Articles__3214EC073CFF80C0");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ArticleCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__ArticleC__C26CCCC5526975C3");

            entity.HasIndex(e => e.Version, "UQ__ArticleC__0F54013433DBD72A").IsUnique();

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleCopies)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__ArticleCo__Artic__3A81B327");

            entity.HasOne(d => d.Item).WithOne(p => p.ArticleCopy)
                .HasForeignKey<ArticleCopy>(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ArticleCo__ItemI__3B75D760");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07476BFDDD");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__BookCopi__C26CCCC5B57804CE");

            entity.HasIndex(e => e.Isbn, "UQ__BookCopi__447D36EAF37893F6").IsUnique();

            entity.Property(e => e.Isbn).HasColumnName("ISBN");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookCopie__BookI__35BCFE0A");

            entity.HasOne(d => d.Item).WithOne(p => p.BookCopy)
                .HasForeignKey<BookCopy>(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BookCopie__ItemI__36B12243");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Items__3214EC0700F9F50B");

            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Date");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Items)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Items__Publisher__276EDEB3");
        });

        modelBuilder.Entity<Magazine>(entity =>
        {
            entity.ToTable(t => t.HasTrigger("delete_MagazineArticle_forMagazines"));

            entity.HasKey(e => e.Id).HasName("PK__Magazine__3214EC07B650E33C");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MagazineArticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Magazine__3214EC0783FA6995");

            entity.ToTable("MagazineArticle");

            entity.HasOne(d => d.Article).WithMany(p => p.MagazineArticles)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MagazineA__Artic__31EC6D26");

            entity.HasOne(d => d.Magazine).WithMany(p => p.MagazineArticles)
                .HasForeignKey(d => d.MagazineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MagazineA__Magaz__30F848ED");
        });

        modelBuilder.Entity<MagazineCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__Magazine__C26CCCC53AD7F679");

            entity.HasIndex(e => e.IssureNumber, "UQ__Magazine__F1B3FFCC737B8277").IsUnique();

            entity.HasOne(d => d.Item).WithOne(p => p.MagazineCopy)
                .HasForeignKey<MagazineCopy>(d => d.ItemId)
                .HasConstraintName("FK__MagazineC__ItemI__403A8C7D");

            entity.HasOne(d => d.Magazine).WithMany(p => p.MagazineCopies)
                .HasForeignKey(d => d.MagazineId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MagazineC__Magaz__3F466844");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable(t => t.HasTrigger("delete_books"));
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC073E4D3E3A");

            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("Publisher");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
