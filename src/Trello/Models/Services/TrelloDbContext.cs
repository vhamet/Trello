using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models
{
    public class TrelloDbContext : DbContext
    {
        public TrelloDbContext () : base()
        {
            
        }
        
        public TrelloDbContext (DbContextOptions<TrelloDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBoard>().HasKey(sc => new { sc.UserId, sc.BoardId });
        }

        public virtual DbSet<Models.Board> tblBoard { get; set; }
        public virtual DbSet<Models.User> tblUser { get; set; }
        public virtual DbSet<Models.UserBoard> tblUserBoard { get; set; }
    }
}