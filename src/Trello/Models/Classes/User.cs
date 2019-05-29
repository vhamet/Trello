using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        public virtual IList<UserBoard> UserBoards { get; set; }
    }
}