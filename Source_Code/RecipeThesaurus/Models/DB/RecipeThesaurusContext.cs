using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipeThesaurus.Models.DB;

public partial class RecipeThesaurusContext : DbContext
{
    public RecipeThesaurusContext()
    {
    }

    public RecipeThesaurusContext(DbContextOptions<RecipeThesaurusContext> options)
        : base(options)
    {
    }

 //
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
        modelBuilder.Entity<LoginByUsernamePassword>().HasNoKey();
    }
//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
