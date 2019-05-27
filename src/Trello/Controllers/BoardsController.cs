﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            var idUser = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var data = new BoardService(context).GetUserBoards(idUser);
            return View("Boards", new BoardsViewModel(data));
        }

        [HttpPost]
        public JsonResult UpdateFavoriteAsync([FromBody] Board board)
        {
            var success = new BoardService(context).UpdateIsFavorite(board.BoardId, board.isFavorite);
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
