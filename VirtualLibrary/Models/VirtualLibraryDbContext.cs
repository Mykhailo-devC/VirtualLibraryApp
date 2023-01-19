using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;

public partial class VirtualLibraryDbContext : DbContext
{
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
            entity.HasKey(e => e.Id).HasName("PK__Articles__3214EC07EEECDBC9");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ArticleCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__ArticleC__C26CCCC52B9680AC");

            entity.HasIndex(e => e.Version, "UQ__ArticleC__0F540134C210F98E").IsUnique();

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleCopies)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__ArticleCo__Artic__3A81B327");

            entity.HasOne(d => d.Item).WithMany(p => p.ArticleCopies)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__ArticleCo__ItemI__3B75D760");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC073A6D69FC");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__BookCopi__C26CCCC5CA618E1F");

            entity.HasIndex(e => e.Isbn, "UQ__BookCopi__447D36EA0C097A99").IsUnique();

            entity.Property(e => e.Isbn).HasColumnName("ISBN");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookCopie__BookI__35BCFE0A");

            entity.HasOne(d => d.Item).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__BookCopie__ItemI__36B12243");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Items__3214EC0751B5B6A0");

            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Items)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Items__Publisher__276EDEB3");
        });

        modelBuilder.Entity<Magazine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Magazine__3214EC0759E5CCA3");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MagazineArticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Magazine__3214EC070EA99B31");

            entity.ToTable("MagazineArticle");

            entity.HasIndex(e => e.IssueNumber, "UQ__Magazine__5703F26C46E00E74").IsUnique();

            entity.HasOne(d => d.Article).WithMany(p => p.MagazineArticles)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__MagazineA__Artic__31EC6D26");

            entity.HasOne(d => d.Magazine).WithMany(p => p.MagazineArticles)
                .HasForeignKey(d => d.MagazineId)
                .HasConstraintName("FK__MagazineA__Magaz__30F848ED");
        });

        modelBuilder.Entity<MagazineCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__Magazine__C26CCCC55F07A4AA");

            entity.HasIndex(e => e.IssureNumber, "UQ__Magazine__F1B3FFCC9771C23A").IsUnique();

            entity.HasOne(d => d.Item).WithMany(p => p.MagazineCopies)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__MagazineC__ItemI__403A8C7D");

            entity.HasOne(d => d.Magazine).WithMany(p => p.MagazineCopies)
                .HasForeignKey(d => d.MagazineId)
                .HasConstraintName("FK__MagazineC__Magaz__3F466844");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC0789315C32");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
