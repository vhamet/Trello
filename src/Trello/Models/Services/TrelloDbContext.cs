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

        public virtual DbSet<Models.Board> tblBoard { get; set; }
        public virtual DbSet<Models.User> tblUser { get; set; }
    }
}