using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class List
    {
        public int ListId { get; set; }

        public int BoardId { get; set; }

        public Board Board { get; set; }

        [StringLength(255)]
        public string Title { get; set; }
        public int Position { get; set; }

        public virtual IList<Card> Cards { get; set; }
    }
}