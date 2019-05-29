using System.Collections.Generic;

namespace Trello.Models
{
    public class BoardViewModel
    {
        public BoardViewModel(Board board)
        {
            Board = board;
        }
        
        public Board Board { get; set; }
    }
}