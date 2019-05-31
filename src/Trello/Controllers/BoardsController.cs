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
            if (board != null)
            {
                var success = new BoardService(context).UpdateIsFavorite(board);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateNameAsync([FromBody] Board board)
        {
            if (board != null)
            {
                var success = new BoardService(context).UpdateBoardName(board);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult CreateBoardAsync([FromBody] Board board)
        {
            if (board != null)
            {
                board = new BoardService(context).CreateBoard(board);
                return Json(board);
            }

            return Json(false);
        }

        public IActionResult Board(int id)
        {
            var service = new BoardService(context);
            var board = service.GetBoard(id);
            if (!service.isAuthorized(IdUser(), board.BoardId))
                return Unauthorized();

            board.Lists = board.Lists.OrderBy(l => l.Position).ToList();
            foreach (var list in board.Lists)
                list.Cards = list.Cards.OrderBy(c => c.Position).ToList();

            return View(new BoardViewModel(board));
        }

        [HttpPost]
        public JsonResult AddListAsync([FromBody] List list)
        {
            if (list != null)
            {
                list = new BoardService(context).CreateList(list);
                return Json(list);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateListTitleAsync([FromBody] List list)
        {
            if (list != null)
            {
                var success = new BoardService(context).UpdateListTitle(list);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateListPositionAsync([FromBody] List list)
        {
            if (list != null)
            {
                var success = new BoardService(context).UpdateListPosition(list);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteListAsync([FromBody] List list)
        {
            if (list != null)
            {
                var success = new BoardService(context).DeleteList(list);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult AddCardAsync([FromBody] Card card)
        {
            if (card != null)
            {
                card = new BoardService(context).CreateCard(card);
                return Json(card);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateCardPositionAsync([FromBody] Card card)
        {
            if (card != null)
            {
                var success = new BoardService(context).UpdateCardPosition(card);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateCardTitleAsync([FromBody] Card card)
        {
            if (card != null)
            {
                var success = new BoardService(context).UpdateCardTitle(card);
                return Json(success);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteCardAsync([FromBody] Card card)
        {
            if (card != null)
            {
                var success = new BoardService(context).DeleteCard(card);
                return Json(success);
            }

            return Json(false);
        }
    }
}
