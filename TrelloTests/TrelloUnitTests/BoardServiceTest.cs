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
        public void CreateBoard_saves_a_board_via_context()
        {
            var mockSet = new Mock<DbSet<Board>>();

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(m => m.tblBoard).Returns(mockSet.Object);

            var service = new BoardService(mockContext.Object);
            service.CreateBoard(new Board() { Name = "BoardTest" });
            
            mockSet.Verify(m => m.Add(It.IsAny<Board>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void UpdateFavorite_should_switch_isFavorite_value()
        {
           var data = new List<Board>
            {
                new Board { Id = 1, isFavorite = true },
                new Board { Id = 2, isFavorite = false },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Board>>();
            mockSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Board>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Board>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblBoard).Returns(mockSet.Object);

            var service = new BoardService(mockContext.Object);
            service.UpdateFavorite(1);
            service.UpdateFavorite(2);

            Assert.False(data.First(b => b.Id == 1).isFavorite);
            Assert.True(data.First(b => b.Id == 2).isFavorite);
        }

        [Fact]
        public void GetAllBoards_should_return_all_boards()
        {
            var data = new List<Board>
            {
                new Board { Name = "AAA" },
                new Board { Name = "BBB" },
                new Board { Name = "ZZZ" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Board>>();
            mockSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Board>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Board>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblBoard).Returns(mockSet.Object);

            var service = new BoardService(mockContext.Object);
            var boards = service.GetAllBoards();

            Assert.True(3 == boards.Count);
        }
    }
}
