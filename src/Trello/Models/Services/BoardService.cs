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
            try
            {
                var boards = db.tblBoard.ToList();
                return boards;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        }  

        public List<Board> GetUserBoards(int idUser)
        {
            try
            {
                var boards = db.tblUserBoard.Where(ub => ub.UserId == idUser).Select(ub => ub.Board).ToList();
                return boards;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
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