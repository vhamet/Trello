using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class UserBoard
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}