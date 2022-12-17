using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<BrandModel> BrandModel { get; set; }
        public virtual DbSet<CarOffer> CarOffer { get; set; }
        public virtual DbSet<Cartype> Cartype { get; set; }
        public virtual DbSet<Colors> Colors { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<FeaturesType> FeaturesType { get; set; }
        public virtual DbSet<Gearbox> Gearbox { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Specs> Specs { get; set; }
        public virtual DbSet<SummaryMostViewed> SummaryMostViewed { get; set; }
        public virtual DbSet<Trim> Trim { get; set; }
        public virtual DbSet<Year> Year { get; set; }
        public virtual DbSet<ImageGallary> ImageGallary { get; set; }
        public virtual DbSet<CarFeatures> CarFeatures { get; set; }
        public virtual DbSet<OfferCustomerInfo> OfferCustomerInfo { get; set; }
        public virtual DbSet<FavoriteOffer> FavoriteOffer { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CarOffer>().Property(x => x.Price).HasPrecision(18, 2);
            //builder.Entity<FavoriteOffer>().HasOne(t => t.Offer)
            //.WithMany();
            base.OnModelCreating(builder);
        }
    }
}
