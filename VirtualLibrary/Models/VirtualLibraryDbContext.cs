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
            entity.HasKey(e => e.Id).HasName("PK__Articles__3214EC07F069ECEC");

            entity.Property(e => e.Author).HasMaxLength(100);

            entity.HasOne(d => d.Item).WithMany(p => p.Articles)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Articles__ItemId__403A8C7D");
        });

        modelBuilder.Entity<ArticleCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__ArticleC__C26CCCC5323192A0");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleCopies)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__ArticleCo__Artic__4D94879B");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07B825115C");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA8D7D3A38").IsUnique();

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Isbn).HasColumnName("ISBN");

            entity.HasOne(d => d.Item).WithMany(p => p.Books)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Books__ItemId__3D5E1FD2");
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__BookCopi__C26CCCC5E76261AA");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookCopie__BookI__4AB81AF0");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Items__3214EC0717C528A5");

            entity.Property(e => e.ItemName).HasMaxLength(50);
            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Items)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Items__Publisher__398D8EEE");
        });

        modelBuilder.Entity<Magazine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Magazine__3214EC07C71B2932");

            entity.HasIndex(e => e.IssueNumber, "UQ__Magazine__5703F26CCF0E55B4").IsUnique();

            entity.HasOne(d => d.Item).WithMany(p => p.Magazines)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Magazines__ItemI__440B1D61");
        });

        modelBuilder.Entity<MagazineArticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Magazine__3214EC078F1EDE53");

            entity.ToTable("MagazineArticle");

            entity.HasOne(d => d.Article).WithMany(p => p.MagazineArticles)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK__MagazineA__Artic__47DBAE45");

            entity.HasOne(d => d.Magazine).WithMany(p => p.MagazineArticles)
                .HasForeignKey(d => d.MagazineId)
                .HasConstraintName("FK__MagazineA__Magaz__46E78A0C");
        });

        modelBuilder.Entity<MagazineCopy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__Magazine__C26CCCC58566BDDA");

            entity.HasOne(d => d.Magazine).WithMany(p => p.MagazineCopies)
                .HasForeignKey(d => d.MagazineId)
                .HasConstraintName("FK__MagazineC__Magaz__5070F446");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC0762B8ED1F");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
