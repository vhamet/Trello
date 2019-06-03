using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Xunit;
using Moq;

using Trello.Models;

namespace TrelloUnitTests
{
    public class BoardServiceTest
    {
        [Fact]
        public void GetAll_should_return_all_boards()
        {
            var data = TestUtils.GetBoardData();
            var service = new BoardService(TestUtils.InitBoardMoqContext(data).Object);
            var boards = service.GetAll();

            Assert.True(3 == boards.Count);
        }

        [Fact]
        public void Get_should_return_corresponding_board()
        {
            var data = TestUtils.GetBoardData();
            var service = new BoardService(TestUtils.InitBoardMoqContext(data).Object);
            var board = service.Get(1);

            Assert.True(board.BoardId == 1);
            Assert.True(board.Name == "Board A");
        }

        [Fact]
        public void Get_should_return_null_if_no_matching_id()
        {
            var data = TestUtils.GetBoardData();
            var service = new BoardService(TestUtils.InitBoardMoqContext(data).Object);
            var board = service.Get(4);

            Assert.True(board == null);
        }

        [Fact]
        public void Get_should_return_nested_lists_and_cards()
        {
            var cards = TestUtils.GetCardData();
            var lists = TestUtils.GetNestedListData(cards);
            var data = TestUtils.GetNestedBoardData(lists);
            var service = new BoardService(TestUtils.InitBoardMoqContext(data).Object);
            var board = service.Get(1);

            Assert.True(board != null);
            Assert.True(board.Lists.Count() > 0);
            Assert.True(board.Lists[0].Cards.Count() > 0);
        }
        
        [Fact]
        public void UpdateIsFavorite_should_update_isFavorite()
        {
            var data = TestUtils.GetBoardData();
            var mockContext = TestUtils.InitBoardMoqContext(data);
            var service = new BoardService(mockContext.Object);
            service.UpdateIsFavorite(new Board() { BoardId = 1, isFavorite = true });
            service.UpdateIsFavorite(new Board() { BoardId = 2, isFavorite = false });

            Assert.True(data.First(b => b.BoardId == 1).isFavorite);
            Assert.False(data.First(b => b.BoardId == 2).isFavorite);
            mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }

        [Fact]
        public void Create_saves_a_board_via_context()
        {
            var mockSet = new Mock<DbSet<Board>>();

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(m => m.tblBoard).Returns(mockSet.Object);

            var service = new BoardService(mockContext.Object);
            service.Create(new Board() { Name = "BoardTest" });
            
            mockSet.Verify(m => m.Add(It.IsAny<Board>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void UpdateName_should_update_board_name()
        {
            var data = TestUtils.GetBoardData();
            var mockContext = TestUtils.InitBoardMoqContext(data);
            var service = new BoardService(mockContext.Object);
            service.UpdateName(new Board() { BoardId = 2, Name = "Test" });

            Assert.True(data.First(b => b.BoardId == 1).Name == "Board A");
            Assert.True(data.First(b => b.BoardId == 2).Name == "Test");
            Assert.True(data.First(b => b.BoardId == 3).Name == "Board Z");
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
