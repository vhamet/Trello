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

        public List<Board> GetAll()
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

        public Board Get(int id)
        {
            try
            {
                var board = db.tblBoard
                    .Include(b => b.Lists)
                    .ThenInclude(l => l.Cards)
                    .FirstOrDefault(b => b.BoardId == id);
                return board;
            }
            catch (Exception e)
            { 
                throw e;
                // return null;
            }
        } 

        public Board Create(Board board)
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

        public bool UpdateIsFavorite(Board board)
        {
            try
            {
                var record = db.tblBoard.Find(board.BoardId);
                if (record != null)
                {
                    record.isFavorite = board.isFavorite;
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

        public bool UpdateName(Board board)
        {
            try
            {
                var record = db.tblBoard.Find(board.BoardId);
                if (record != null)
                {
                    record.Name = board.Name;
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