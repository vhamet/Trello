using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }
    }
}