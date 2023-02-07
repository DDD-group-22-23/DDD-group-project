using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RecipeThesaurus.Models.DB
{
    public partial class RecipeThesaurusContext : DbContext
    {
        public RecipeThesaurusContext()
        {
        }

        public RecipeThesaurusContext(DbContextOptions<RecipeThesaurusContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:recipethesaurus.database.windows.net,1433;Initial Catalog=RecipeThesaurus;Persist Security Info=False;User ID=recipethesaurussqluser;Password=yFctu4fAhCsYQ86g");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////modelBuilder.Entity<Login>(entity =>  
            ////{  
            ////    entity.Property(e => e.Id).HasColumnName("id");  

            ////    entity.Property(e => e.Password)  
            ////        .IsRequired()  
            ////        .HasColumnName("password")  
            ////        .HasMaxLength(50)  
            ////        .IsUnicode(false);  

            ////    entity.Property(e => e.Username)  
            ////        .IsRequired()  
            ////        .HasColumnName("username")  
            ////        .HasMaxLength(50)  
            ////        .IsUnicode(false);  
            ////});  

            // [Asma Khalid]: Query for store procedure.  
            //modelBuilder.Query<LoginByUsernamePassword>();

            modelBuilder.Entity<LoginByUsernamePassword>();
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
