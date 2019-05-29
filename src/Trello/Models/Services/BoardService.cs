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

        public Board GetBoard(int id)
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

        public bool UpdateBoardName(Board board)
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

        public bool isAuthorized(int idUser, int idBoard)
        {
            try{
                return db.tblUserBoard.Any(ub => ub.UserId == idUser && ub.BoardId == idBoard);
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }

        public List CreateList(List list) {
            try
            {
                db.tblList.Add(list);
                db.SaveChanges();
                
                return list;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        }

        public bool UpdateListTitle(List list)
        {
            try
            {
                var record = db.tblList.Find(list.ListId);
                if (record != null)
                {
                    record.Title = list.Title;
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

        public bool UpdateListPosition(List list)
        {
            try
            {
                var record = db.tblList.Find(list.ListId);
                if (record != null)
                {
                    record.Position = list.Position;
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


        public bool DeleteList(List list)
        {
            try
            {
                var cards = db.tblCard.Where(c => c.ListId == list.ListId);

                foreach (var card in cards)
                    db.tblCard.Remove(card);

                db.tblList.Remove(list);
                db.SaveChanges();  

                return true;
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }
    }
}