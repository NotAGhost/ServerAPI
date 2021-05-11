using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Models;

namespace ServerAPI.DataBase {
    public class CloudContext : DbContext {
        public CloudContext(DbContextOptions<CloudContext> options) : base(options) {

        }

        public DbSet<Folder> CloudFolders { get; set; }
        public DbSet<Files> CloudFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Files>().ToTable("FileTable");
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


        }
    }
}