using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Context
{
    public class AdDbContext : IdentityDbContext<User>
    {
        public AdDbContext(DbContextOptions<AdDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ad
            modelBuilder.Entity<Ad>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Ad>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<Ad>().Property(x => x.Content).IsRequired().HasColumnType("varchar").HasMaxLength(500);
            modelBuilder.Entity<Ad>().Property(x => x.DateCreate).HasDefaultValueSql("GetUtcDate()");
            modelBuilder.Entity<Ad>().Property(x => x.Price).IsRequired().HasColumnType("decimal").HasPrecision(9, 2);
            modelBuilder.Entity<Ad>().Property(x => x.IsVerified).IsRequired().HasColumnType("bit").HasDefaultValue(false);
            modelBuilder.Entity<Ad>().Property(x => x.IsTop).IsRequired().HasColumnType("bit").HasDefaultValue(false);
            //CityId OneToMany
            modelBuilder.Entity<Ad>().HasOne(x => x.City).WithMany(x => x.Ads).HasForeignKey(x => x.CityName);
            //CategotyId OneToMany
            modelBuilder.Entity<Ad>().HasOne(x => x.Categoty).WithMany(x => x.Ads).HasForeignKey(x => x.CategotyId);
            //UserId OneToMany
            modelBuilder.Entity<Ad>().HasOne(x => x.User).WithMany(x => x.Ads).HasForeignKey(x => x.UserId).IsRequired();


            //User
            modelBuilder.Entity<User>().Property(x => x.Surname).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<User>().Property(x => x.IsMailing).IsRequired().HasColumnType("bit").HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(x => x.IconPath).IsRequired().HasColumnType("varchar").HasMaxLength(225).HasDefaultValue("img/defUser.png");
            //CityName OneToMany
            modelBuilder.Entity<User>().HasOne(x => x.City).WithMany(x => x.Users).HasForeignKey(x => x.CityName).OnDelete(DeleteBehavior.ClientNoAction);


            //Categoty
            modelBuilder.Entity<Category>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            modelBuilder.Entity<Category>().HasMany(x => x.Categories).WithOne(x => x.Categors).HasForeignKey(x => x.CategoryId);


            //City
            modelBuilder.Entity<City>().HasKey(x => x.Name);
            modelBuilder.Entity<City>().Property(x => x.Region).IsRequired().HasColumnType("varchar").HasMaxLength(100);


            //Comment
            modelBuilder.Entity<Comment>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Comment>().Property(x => x.Content).HasColumnType("varchar").HasMaxLength(500);
            modelBuilder.Entity<Comment>().Property(x => x.DateCreate).HasDefaultValueSql("GetUtcDate()");
            //Product -> Comments OneToMany
            modelBuilder.Entity<Comment>().HasOne(x => x.Ad).WithMany(x => x.Comments).HasForeignKey(x => x.AdId);
            //User -> Comments OneToMany
            modelBuilder.Entity<Comment>().HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);


            //default initialization
            var managerDef = new User()
            {
                UserName = "Tima",
                NormalizedUserName = "Tima".ToUpper(),
                Surname = "Ch",
                NormalizedEmail = "temp.tima@gmail.com".ToUpper(),
                Email = "temp.tima@gmail.com",
                EmailConfirmed = true
            };
            var hasher = new PasswordHasher<User>();
            managerDef.PasswordHash = hasher.HashPassword(managerDef, "admin");

            var userRole = new IdentityRole()
            {
                Name = UserRole.User,
                NormalizedName = UserRole.User.ToUpper()
            };
            var managerRole = new IdentityRole()
            {
                Name = UserRole.Manager,
                NormalizedName = UserRole.Manager.ToUpper()
            };

            modelBuilder.Entity<User>().HasData(new User[] { managerDef });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole[] { userRole, managerRole });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>()
                {
                    RoleId = managerRole.Id,
                    UserId = managerDef.Id
                }
            });
            modelBuilder.Entity<City>().HasData(new City[]
            {
                new City()
                {
                    Name = "Name_0F",
                    Region = "Region_F"
                },
                new City()
                {
                    Name = "Name_1F",
                    Region = "Region_F"
                },
                new City()
                {
                    Name = "Name_78B",
                    Region = "Region_B"
                },
                new City()
                {
                    Name = "Name_24B",
                    Region = "Region_B"
                },
                new City()
                {
                    Name = "Name_22B",
                    Region = "Region_B"
                },
                new City()
                {
                    Name = "Name_99X",
                    Region = "Region_X"
                }
            });

            modelBuilder.Entity<Category>().HasData(new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Category_EE"
                },
                new Category()
                {
                    Id = 88,
                    Name = "Category_0E",
                    CategoryId = 1,
                },
                new Category()
                {
                    Id = 89,
                    Name = "Category_1E",
                    CategoryId = 1
                },
                new Category()
                {
                    Id = 2,
                    Name = "Category_CC",
                },
                new Category()
                {
                    Id = 3,
                    Name = "Category_FF",
                },
                new Category()
                {
                    Id = 77,
                    Name = "Category_F1",
                    CategoryId = 3
                },
                new Category()
                {
                    Id = 78,
                    Name = "Category_F0",
                    CategoryId = 3
                },
                new Category()
                {
                    Id = 66,
                    Name = "Category_F0A",
                    CategoryId = 78
                },
                new Category()
                {
                    Id = 67,
                    Name = "Category_F0B",
                    CategoryId = 78
                },
                new Category()
                {
                    Id = 68,
                    Name = "Category_F0C",
                    CategoryId = 78
                },

            });
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
