﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerAPI.DataBase;

namespace ServerAPI.Migrations
{
    [DbContext(typeof(CloudContext))]
    [Migration("20210511163613_cloudDbMig-v2.0")]
    partial class cloudDbMigv20
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ServerAPI.Models.Files", b =>
                {
                    b.Property<int>("FilesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Bytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentFolderId")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<string>("TruePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FilesId");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("FileTable");
                });

            modelBuilder.Entity("ServerAPI.Models.Folder", b =>
                {
                    b.Property<int>("FolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("int");

                    b.Property<string>("TruePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FolderId");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("FolderTable");
                });

            modelBuilder.Entity("ServerAPI.Models.Files", b =>
                {
                    b.HasOne("ServerAPI.Models.Folder", "ParentFolder")
                        .WithMany("Files")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("ServerAPI.Models.Folder", b =>
                {
                    b.HasOne("ServerAPI.Models.Folder", "ParentFolder")
                        .WithMany("Folders")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("ServerAPI.Models.Folder", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Folders");
                });
#pragma warning restore 612, 618
        }
    }
}
