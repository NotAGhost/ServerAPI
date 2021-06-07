using System.IO;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Models;

namespace ServerAPI.DataBase {
    public class CloudContext : DbContext {
        public CloudContext(DbContextOptions<CloudContext> options) : base(options) {

        }

        public DbSet<Folder> CloudFolders { get; set; }
        public DbSet<Files> CloudFilesModel { get; set; }
        public DbSet<FilesContent> CloudFilesContent { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Files>().ToTable("FileTable");
            modelBuilder.Entity<FilesContent>().ToTable("FilesContentTable");
            modelBuilder.Entity<Folder>().ToTable("FolderTable");

            modelBuilder.Entity<Files>()
                .HasOne(s => s.ParentFolder)
                .WithMany(g => g.Files)
                .HasForeignKey(s => s.ParentFolderId);

            modelBuilder.Entity<Folder>()
                .HasOne(s => s.ParentFolder)
                .WithMany(g => g.Folders)
                .HasForeignKey(s => s.ParentFolderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Files>()
                .HasOne(p => p.FileContent)
                .WithOne(p => p.FileModel)
                .HasForeignKey<FilesContent>(k => k.FilesDataId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FilesContent>().HasKey(p => p.ContentId);

        }
    }
}