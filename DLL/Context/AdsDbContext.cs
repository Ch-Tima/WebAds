using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Context
{
    public class AdDbContext : IdentityDbContext<User>
    {
        public AdDbContext(DbContextOptions<AdDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ad
            modelBuilder.Entity<Ad>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Ad>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<Ad>().Property(x => x.Content).HasColumnType("varchar").HasMaxLength(500);
            modelBuilder.Entity<Ad>().Property(x => x.DateCreate).HasDefaultValueSql("GetUtcDate()");
            modelBuilder.Entity<Ad>().Property(x => x.Price).IsRequired().HasColumnType("decimal").HasPrecision(9, 2);
            modelBuilder.Entity<Ad>().Property(x => x.IsVerified).IsRequired().HasColumnType("bit").HasDefaultValue(false);
            modelBuilder.Entity<Ad>().Property(x => x.IsTop).IsRequired().HasColumnType("bit").HasDefaultValue(false);

            //CityId OneToMany
            modelBuilder.Entity<Ad>().HasOne(x => x.City).WithMany(x => x.Ads).HasForeignKey(x => x.CityId);
            //CategotyId OneToMany
            modelBuilder.Entity<Ad>().HasOne(x => x.Categoty).WithMany(x => x.Ads).HasForeignKey(x => x.CategotyId);
            //UserId OneToMany
            modelBuilder.Entity<Ad>().HasOne(x => x.User).WithMany(x => x.Ads).HasForeignKey(x => x.UserId);


            //User
            modelBuilder.Entity<User>().Property(x => x.Surname).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<User>().Property(x => x.IconPath).IsRequired().HasColumnType("varchar").HasMaxLength(600);
            modelBuilder.Entity<User>().Property(x => x.Address).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<User>().Property(x => x.NumberPhone).IsRequired().HasColumnType("varchar").HasMaxLength(100);


            //Categoty
            modelBuilder.Entity<Category>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            //modelBuilder.Entity<Category>().Property(x => x.CategotyId);
            //???


            //City
            modelBuilder.Entity<City>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<City>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<City>().Property(x => x.Region).IsRequired().HasColumnType("varchar").HasMaxLength(100);

            //Comment
            modelBuilder.Entity<Comment>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Comment>().Property(x => x.Content).HasColumnType("varchar").HasMaxLength(500);
            modelBuilder.Entity<Comment>().Property(x => x.DateCreate).HasDefaultValueSql("GetUtcDate()");
            //Product -> Comments OneToMany
            modelBuilder.Entity<Comment>().HasOne(x => x.Ad).WithMany(x => x.Comments).HasForeignKey(x => x.AdId);
            //User -> Comments OneToMany //???
            modelBuilder.Entity<Comment>().HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);


           
        }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
