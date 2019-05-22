using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trello.Models;

namespace Trello.Controllers
{
    public class BoardsController : Controller
    {
        private TrelloDbContext context;
        public BoardsController(TrelloDbContext tdb)
        {
            context = tdb;
        }
        
        public IActionResult Index()
        {
            var data = new BoardService(context).GetAllBoards();
            return View("Boards", new BoardViewModel(data));
        }

        [HttpPost]
        public JsonResult UpdateFavoriteAsync([FromBody] int id)
        {
            var success = new BoardService(context).UpdateFavorite(id);
            return Json(success);
        }

        [HttpPost]
        public JsonResult CreateBoardAsync([FromBody] Board board)
        {
            board = new BoardService(context).CreateBoard(board);
            return Json(board);
        }
    }
}
