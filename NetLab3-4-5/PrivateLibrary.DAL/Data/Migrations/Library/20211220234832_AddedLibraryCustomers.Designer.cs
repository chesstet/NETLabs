﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrivateLibrary.DAL.Contexts;

#nullable disable

namespace PrivateLibrary.DAL.Data.Migrations.Library
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20211220234832_AddedLibraryCustomers")]
    partial class AddedLibraryCustomers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BookLibraryCustomer", b =>
                {
                    b.Property<Guid>("BooksId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BooksId", "CustomersId");

                    b.HasIndex("CustomersId");

                    b.ToTable("BookLibraryCustomer");
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfDeath")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("DirectionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("DirectionId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.Direction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.LibraryCustomer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("LibraryCustomers");
                });

            modelBuilder.Entity("BookLibraryCustomer", b =>
                {
                    b.HasOne("PrivateLibrary.DAL.Models.Book.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrivateLibrary.DAL.Models.Book.LibraryCustomer", null)
                        .WithMany()
                        .HasForeignKey("CustomersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.Book", b =>
                {
                    b.HasOne("PrivateLibrary.DAL.Models.Book.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.HasOne("PrivateLibrary.DAL.Models.Book.Direction", "Direction")
                        .WithMany("Books")
                        .HasForeignKey("DirectionId");

                    b.Navigation("Author");

                    b.Navigation("Direction");
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("PrivateLibrary.DAL.Models.Book.Direction", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}