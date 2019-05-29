using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class Card
    {
        public int CardId { get; set; }

        public int ListId { get; set; }

        public List List { get; set; }

        [StringLength(255)]
        public string Title { get; set; }
    }
}