using ShoppingMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ShoppingMVC.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fiction", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Computing", DisplayOrder = 2 },
            new Category { Id = 3, Name = "Business", DisplayOrder = 3 },
            new Category { Id = 4, Name = "History", DisplayOrder = 4 }
            );

            modelBuilder.Entity<Product>().HasData(
             
            new Product
            {
                Id = 1,
                Title = "Normal People",
                Author = "Sally Rooney",
                Description = "A story of mutual fascination, friendship and love. It takes us from that first conversation to the years beyond,in the company of two people who try to stay apart but find they can't.",
                ISBN = "9780571334650",
                ListPrice = 28,
                CategoryId = 1,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 2,
                Title = "The Alchemist",
                Author = "Paulo Coelho",
                Description = "Paulo Coelho's enchanting novel has inspired a devoted following around the world. Lush, evocative, and deeply humane, the story of Santiago is an eternal testament to the transforming power of our dreams and the importance of listening.",
                ISBN = "9780062315007",
                ListPrice = 25,
                CategoryId = 1,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 3,
                Title = "Mastering AI",
                Author = "Jeremy Kahn",
                Description = "An urgent book on generative artificial intelligence from one of the top US journalists in the tech field, exploring the risk and benefits looming.",
                ISBN = "9781835010433",
                ListPrice = 40,
                CategoryId = 2,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 4,
                Title = "Dummies: Coding For Dummies",
                Author = "Nikhil Abraham",
                Description = "Hands-on exercises help you learn to code like a pro. No coding experience is required for Coding For Dummies, your one-stop guide to building a foundation of knowledge in writing computer code for web, application, and software development.",
                ISBN = "9781119293323",
                ListPrice = 71,
                CategoryId = 2,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 5,
                Title = "Main Street Millionaire",
                Author = "Codie Sanchez",
                Description = "Rich people know a secret. In this book, former Wall Street investor Codie Sanchez pulls back the curtain. Rich people know a secret. In this book, former Wall Street investor Codie Sanchez pulls back the curtain.",
                ISBN = "9781529146721",
                ListPrice = 40,
                CategoryId = 3,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 6,
                Title = "The Intelligent Investor Third Edition",
                Author = "Benjamin Graham",
                Description = "Since its original publication in 1949, Benjamin Graham's revered classic, The Intelligent Investor, has taught and inspired millions of people worldwide and remains the most respected guide to investing.",
                ISBN = "9780063423534",
                ListPrice = 40,
                CategoryId = 3,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 7,
                Title = "The Treaty of Waitangi: Te Tiriti o Waitangi",
                Author = "Ross Calman",
                Description = "The story of The Treaty of Waitangi/Te Tiriti o Waitangi is one of the greatest in New Zealand history. It marks the moment British and Maori history intersected and a new nation was formed.",
                ISBN = "9781990042775",
                ListPrice = 30,
                CategoryId = 4,
                ImageUrl = "",
                Stock = 10
            },

            new Product
            {
                Id = 8,
                Title = "Pakeha Slaves, Maori Masters",
                Author = "Trevor Bentley",
                Description = "While people are aware of the atrocities of the black slave trade, few are aware of the enslavement and trafficking of Europeans in 19th-century New Zealand.",
                ISBN = "9780473700874",
                ListPrice = 40,
                CategoryId = 4,
                ImageUrl = "",
                Stock = 10
            }
            );
        }

    }
}
