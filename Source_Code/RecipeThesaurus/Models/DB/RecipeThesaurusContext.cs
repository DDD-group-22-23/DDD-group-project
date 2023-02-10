using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RecipeThesaurus.Models;
using RecipeThesaurus.Models.DB.LoginUsernamePassword;

namespace RecipeThesaurus.Models.DB.context;
public partial class RecipeThesaurusContext
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Id { get; set; } = null!;

}


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

        // [Asma Khalid]: Query for store procedure.  
        
    modelBuilder.Entity<LoginByUsernamePassword>().HasNoKey();
        
    }
    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
