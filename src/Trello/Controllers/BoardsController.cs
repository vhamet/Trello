using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Models;

namespace Trello.Controllers
{
    [Authorize]
    public class BoardsController : Controller
    {
        private TrelloDbContext context;
        
        public BoardsController(TrelloDbContext tdb)
        {
            context = tdb;
        }

        private int IdUser() 
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        public IActionResult Index()
        {
            var data = new BoardService(context).GetUserBoards(IdUser());
            return View("Boards", new BoardsViewModel(data));
        }

        [HttpPost]
        public JsonResult UpdateFavoriteAsync([FromBody] Board board)
        {
            var success = new BoardService(context).UpdateIsFavorite(board);
            return Json(success);
        }

        [HttpPost]
        public JsonResult UpdateNameAsync([FromBody] Board board)
        {
            var success = new BoardService(context).UpdateBoardName(board);
            return Json(success);
        }

        [HttpPost]
        public JsonResult CreateBoardAsync([FromBody] Board board)
        {
            board = new BoardService(context).CreateBoard(board);
            return Json(board);
        }

        [HttpPost]
        public JsonResult AddListAsync([FromBody] List list)
        {
            list = new BoardService(context).CreateList(list);
            return Json(list);
        }
        
        public IActionResult Board(int id)
        {
            var service = new BoardService(context);
            var board = service.GetBoard(id);
            if (!service.isAuthorized(IdUser(), board.BoardId))
                return Unauthorized();

            board.Lists = board.Lists.OrderBy(l => l.Position).ToList();

            return View(new BoardViewModel(board));
        }

        [HttpPost]
        public JsonResult UpdateListTitleAsync([FromBody] List list)
        {
            var success = new BoardService(context).UpdateListTitle(list);
            return Json(success);
        }

        [HttpPost]
        public JsonResult UpdateListPositionAsync([FromBody] List list)
        {
            var success = new BoardService(context).UpdateListPosition(list);
            return Json(success);
        }

        [HttpPost]
        public JsonResult DeleteListAsync([FromBody] List list)
        {
            var success = new BoardService(context).DeleteList(list);
            return Json(success);
        }
    }
}
