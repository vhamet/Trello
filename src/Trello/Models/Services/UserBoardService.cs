using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models
{
    public class UserBoardService
    {
        private TrelloDbContext db;

        public UserBoardService(TrelloDbContext context)
        {
            db = context;
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
    }
}