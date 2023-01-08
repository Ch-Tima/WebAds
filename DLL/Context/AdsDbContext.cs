using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Context
{
    public class AdDbContext : IdentityDbContext<User, IdentityRole, string>
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
            //modelBuilder.Entity<User>().Property<string>(x => x.CityName).IsRequired(false);
            modelBuilder.Entity<User>().HasOne(x => x.City).WithMany(x => x.Users).HasForeignKey(x => x.CityName).OnDelete(DeleteBehavior.SetNull);


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

            //FavoritesAd
            modelBuilder.Entity<FavouritesAd>().HasIndex(x => x.Id).IsUnique();
            //modelBuilder.Entity<FavouritesAd>().HasKey(x => new { x.AdId, x.UserId });
            modelBuilder.Entity<FavouritesAd>().HasOne(x => x.User).WithMany(x => x.FavoritesAds).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FavouritesAd>().HasOne(x => x.Ad).WithMany(x => x.FavoritesAds).HasForeignKey(x => x.AdId).OnDelete(DeleteBehavior.NoAction);


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
                    Name = "Kharkiv",
                    Region = "Kharkiv"
                },
                new City()
                {
                    Name = "Lozova",
                    Region = "Kharkiv"
                },
                new City()
                {
                    Name = "Izium",
                    Region = "Kharkiv"
                },
                new City()
                {
                    Name = "Odesa",
                    Region = "Odesa"
                },
                new City()
                {
                    Name = "Izmail",
                    Region = "Odesa"
                },
                new City()
                {
                    Name = "Dnipro",
                    Region = "Dnipro­petrovsk"
                }
            });

            modelBuilder.Entity<Category>().HasData(new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "House and garden"
                },
                new Category()
                {
                    Id = 88,
                    Name = "Instruments",
                    CategoryId = 1,
                },
                new Category()
                {
                    Id = 89,
                    Name = "Furniture",
                    CategoryId = 1
                },

                new Category()
                {
                    Id = 2,
                    Name = "Transport",
                },

                new Category()
                {
                    Id = 3,
                    Name = "Electronics",
                },

                new Category()
                {
                    Id = 77,
                    Name = "PC",
                    CategoryId = 3
                },
                new Category()
                {
                    Id = 6231,
                    Name = "CPU",
                    CategoryId = 77
                },
                new Category()
                {
                    Id = 6232,
                    Name = "GPU",
                    CategoryId = 77
                },
                new Category()
                {
                    Id = 6233,
                    Name = "SSD/HDD",
                    CategoryId = 77
                },
                new Category()
                {
                    Id = 6234,
                    Name = "RAM",
                    CategoryId = 77
                },


                new Category()
                {
                    Id = 78,
                    Name = "Phone",
                    CategoryId = 3
                },
                new Category()
                {
                    Id = 66,
                    Name = "Samsung",
                    CategoryId = 78
                },
                new Category()
                {
                    Id = 67,
                    Name = "Xiaomi",
                    CategoryId = 78
                },
                new Category()
                {
                    Id = 68,
                    Name = "Apple",
                    CategoryId = 78
                },

            });
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FavouritesAd> FavouritesAds { get; set; }
    }
}
