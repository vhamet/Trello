using System.Collections.Generic;

namespace Trello.Models
{
    public class BoardsViewModel
    {
        public BoardsViewModel(List<Board> boards)
        {
            Boards = boards;
        }
        
        public List<Board> Boards { get; set; }
    }
}