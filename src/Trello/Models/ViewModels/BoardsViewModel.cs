using System.Collections.Generic;

namespace Trello.Models
{
    public class BoardsViewModel
    {
        public List<Board> Boards { get; set; }

        public BoardsViewModel(List<Board> boards)
        {
            Boards = boards;
        }
    }
}