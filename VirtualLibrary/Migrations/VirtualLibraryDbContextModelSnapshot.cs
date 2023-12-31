﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtualLibrary.Models;

#nullable disable

namespace VirtualLibrary.Migrations
{
    [DbContext(typeof(VirtualLibraryDbContext))]
    partial class VirtualLibraryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VirtualLibrary.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Articles__3214EC073CFF80C0");

                    b.ToTable("Articles", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.ArticleCopy", b =>
                {
                    b.Property<int>("CopyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CopyId"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("CopyId")
                        .HasName("PK__ArticleC__C26CCCC5526975C3");

                    b.HasIndex("ArticleId");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.HasIndex(new[] { "Version" }, "UQ__ArticleC__0F54013433DBD72A")
                        .IsUnique();

                    b.ToTable("ArticleCopies", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Books__3214EC07476BFDDD");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.BookCopy", b =>
                {
                    b.Property<int>("CopyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CopyId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("Isbn")
                        .HasColumnType("int")
                        .HasColumnName("ISBN");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("CopyId")
                        .HasName("PK__BookCopi__C26CCCC5B57804CE");

                    b.HasIndex("BookId");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.HasIndex(new[] { "Isbn" }, "UQ__BookCopi__447D36EAF37893F6")
                        .IsUnique();

                    b.ToTable("BookCopies", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("PublishDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("Date")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Items__3214EC0700F9F50B");

                    b.HasIndex("PublisherId");

                    b.ToTable("Items", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.Magazine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Magazine__3214EC07B650E33C");

                    b.ToTable("Magazines", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.MagazineArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("MagazineId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Magazine__3214EC0783FA6995");

                    b.HasIndex("ArticleId");

                    b.HasIndex("MagazineId");

                    b.ToTable("MagazineArticle", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.MagazineCopy", b =>
                {
                    b.Property<int>("CopyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CopyId"));

                    b.Property<int>("IssureNumber")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("MagazineId")
                        .HasColumnType("int");

                    b.HasKey("CopyId")
                        .HasName("PK__Magazine__C26CCCC53AD7F679");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.HasIndex("MagazineId");

                    b.HasIndex(new[] { "IssureNumber" }, "UQ__Magazine__F1B3FFCC737B8277")
                        .IsUnique();

                    b.ToTable("MagazineCopies", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Publisher");

                    b.HasKey("Id")
                        .HasName("PK__Publishe__3214EC073E4D3E3A");

                    b.ToTable("Publishers", (string)null);
                });

            modelBuilder.Entity("VirtualLibrary.Models.ArticleCopy", b =>
                {
                    b.HasOne("VirtualLibrary.Models.Article", "Article")
                        .WithMany("ArticleCopies")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ArticleCo__Artic__3A81B327");

                    b.HasOne("VirtualLibrary.Models.Item", "Item")
                        .WithOne("ArticleCopy")
                        .HasForeignKey("VirtualLibrary.Models.ArticleCopy", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ArticleCo__ItemI__3B75D760");

                    b.Navigation("Article");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("VirtualLibrary.Models.BookCopy", b =>
                {
                    b.HasOne("VirtualLibrary.Models.Book", "Book")
                        .WithMany("BookCopies")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__BookCopie__BookI__35BCFE0A");

                    b.HasOne("VirtualLibrary.Models.Item", "Item")
                        .WithOne("BookCopy")
                        .HasForeignKey("VirtualLibrary.Models.BookCopy", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__BookCopie__ItemI__36B12243");

                    b.Navigation("Book");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("VirtualLibrary.Models.Item", b =>
                {
                    b.HasOne("VirtualLibrary.Models.Publisher", "Publisher")
                        .WithMany("Items")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Items__Publisher__276EDEB3");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("VirtualLibrary.Models.MagazineArticle", b =>
                {
                    b.HasOne("VirtualLibrary.Models.Article", "Article")
                        .WithMany("MagazineArticles")
                        .HasForeignKey("ArticleId")
                        .IsRequired()
                        .HasConstraintName("FK__MagazineA__Artic__31EC6D26");

                    b.HasOne("VirtualLibrary.Models.Magazine", "Magazine")
                        .WithMany("MagazineArticles")
                        .HasForeignKey("MagazineId")
                        .IsRequired()
                        .HasConstraintName("FK__MagazineA__Magaz__30F848ED");

                    b.Navigation("Article");

                    b.Navigation("Magazine");
                });

            modelBuilder.Entity("VirtualLibrary.Models.MagazineCopy", b =>
                {
                    b.HasOne("VirtualLibrary.Models.Item", "Item")
                        .WithOne("MagazineCopy")
                        .HasForeignKey("VirtualLibrary.Models.MagazineCopy", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MagazineC__ItemI__403A8C7D");

                    b.HasOne("VirtualLibrary.Models.Magazine", "Magazine")
                        .WithMany("MagazineCopies")
                        .HasForeignKey("MagazineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MagazineC__Magaz__3F466844");

                    b.Navigation("Item");

                    b.Navigation("Magazine");
                });

            modelBuilder.Entity("VirtualLibrary.Models.Article", b =>
                {
                    b.Navigation("ArticleCopies");

                    b.Navigation("MagazineArticles");
                });

            modelBuilder.Entity("VirtualLibrary.Models.Book", b =>
                {
                    b.Navigation("BookCopies");
                });

            modelBuilder.Entity("VirtualLibrary.Models.Item", b =>
                {
                    b.Navigation("ArticleCopy")
                        .IsRequired();

                    b.Navigation("BookCopy")
                        .IsRequired();

                    b.Navigation("MagazineCopy")
                        .IsRequired();
                });

            modelBuilder.Entity("VirtualLibrary.Models.Magazine", b =>
                {
                    b.Navigation("MagazineArticles");

                    b.Navigation("MagazineCopies");
                });

            modelBuilder.Entity("VirtualLibrary.Models.Publisher", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
