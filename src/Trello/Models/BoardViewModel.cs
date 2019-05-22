using System.Collections.Generic;

namespace Trello.Models
{
    public class BoardViewModel
    {
        public List<Board> Boards { get; set; }

        public BoardViewModel(List<Board> boards)
        {
            Boards = boards;
        }
    }
}