using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trello.Models
{
    public class Board
    {
        public int BoardId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Background { get; set; }

        public string BackgroundClass
        {
            get
            {
                return "background-" + Background;
            }
        }

        public bool isFavorite { get; set; }
        public string SelectedClass
        {
            get
            {
                return isFavorite ? "board-link-star-selected" : string.Empty;
            }
        }

        public IList<UserBoard> UserBoards { get; set; }
    }
}