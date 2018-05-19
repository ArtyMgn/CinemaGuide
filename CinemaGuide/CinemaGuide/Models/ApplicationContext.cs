﻿using CinemaGuide.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace CinemaGuide.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbProfile> Profiles { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>()
                .HasOne(user => user.Profile)
                .WithOne(profile => profile.User)
                .HasForeignKey<DbUser>(x => x.ProfileForeignKey);
        }
    }
}
