﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Stazh.Core.Data.Entities;

namespace Stazh.Data.Persistence
{
    public class StazhDataContext : DbContext
    {
        private readonly string _connectionString;

        public StazhDataContext()
        {
            _connectionString = @"Server=.\SQLEXPRESS;Database=Stazh;Trusted_Connection=True;";
        }

        public StazhDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentItemId);
        }
    }
}
