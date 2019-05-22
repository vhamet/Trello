using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models
{
    public class BoardService
    {
        private TrelloDbContext _context;

        public BoardService(TrelloDbContext context)
        {
            _context = context;
        }

        public List<Board> GetAllBoards()
        {
            var boards = _context.tblBoard.ToList();
            return boards;
        }  

        public Board CreateBoard(Board board)
        {
            try
            {
                _context.tblBoard.Add(board);
                _context.SaveChanges();
                
                return board;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        } 

        public bool UpdateFavorite(int id)
        {
            try
            {
                var board = _context.tblBoard.FirstOrDefault(b => b.Id == id);
                if (board != null)
                {
                    board.isFavorite = !board.isFavorite;
                    return _context.SaveChanges() == 1;
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }
    }
}