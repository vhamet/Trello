using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models
{
    public class BoardService
    {
        private TrelloDbContext db;

        public BoardService(TrelloDbContext context)
        {
            db = context;
        }

        public List<Board> GetAllBoards()
        {
            var boards = db.tblBoard.ToList();
            return boards;
        }  

        public Board CreateBoard(Board board)
        {
            try
            {
                db.tblBoard.Add(board);
                db.SaveChanges();
                
                return board;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        }

        public bool UpdateIsFavorite(int id, bool value)
        {
            try
            {
                var record = db.tblBoard.Find(id);
                if (record != null)
                {
                    record.isFavorite = value;
                    return db.SaveChanges() == 1;
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