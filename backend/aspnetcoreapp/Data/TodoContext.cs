using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TodoApi.Models;
using MongoDB.EntityFrameworkCore;
namespace TodoApi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().ToContainer("TodoContainer");
        }

    }
}