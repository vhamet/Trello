using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Trello.Models;

namespace TrelloUnitTests
{
    public static class TestUtils
    {
        public static IQueryable<Board> GetBoardData()
        {
            return new List<Board>
            {
                new Board { BoardId = 1, Name = "Board A", isFavorite = false },
                new Board { BoardId = 2, Name = "Board B", isFavorite = true },
                new Board { BoardId = 3, Name = "Board Z", isFavorite = true },
            }.AsQueryable();
        }
        public static IQueryable<Board> GetNestedBoardData(IQueryable<List> lists)
        {
            return new List<Board>
            {
                new Board { BoardId = 1, Name = "Board A", isFavorite = false, Lists = lists.ToList() },
                new Board { BoardId = 2, Name = "Board B", isFavorite = true },
                new Board { BoardId = 3, Name = "Board Z", isFavorite = true },
            }.AsQueryable();
        }

        public static IQueryable<User> GetUserData()
        {
            return new List<User>
            {
                new User { UserId = 1, Username = "User1", Email = "user1@trello.com", Password = "1234" },
                new User { UserId = 2, Username = "User2", Email = "user2@trello.com", Password = "1234" },
            }.AsQueryable();
        }

        public static IQueryable<UserBoard> GetUserBoardData(IQueryable<Board> boards, IQueryable<User> users)
        {
            return new List<UserBoard>
            {
                new UserBoard { UserId = users.ElementAt(0).UserId, User = users.ElementAt(0), BoardId = boards.ElementAt(0).BoardId, Board = boards.ElementAt(0) },
                new UserBoard { UserId = users.ElementAt(0).UserId, User = users.ElementAt(0), BoardId = boards.ElementAt(1).BoardId, Board = boards.ElementAt(1) },
                new UserBoard { UserId = users.ElementAt(1).UserId, User = users.ElementAt(1), BoardId = boards.ElementAt(1).BoardId, Board = boards.ElementAt(1) },
                new UserBoard { UserId = users.ElementAt(1).UserId, User = users.ElementAt(1), BoardId = boards.ElementAt(2).BoardId, Board = boards.ElementAt(2) },
            }.AsQueryable();
        }
        
        public static IQueryable<List> GetListData()
        {
            return new List<List>
            {
                new List { ListId = 1, Title = "List A", Position = 1 },
                new List { ListId = 2, Title = "List B", Position = 2 },
                new List { ListId = 3, Title = "List Z", Position = 3 },
            }.AsQueryable();
        }
        
        public static IQueryable<List> GetNestedListData(IQueryable<Card> cards)
        {
            return new List<List>
            {
                new List { ListId = 1, Title = "List A", Position = 1, Cards = cards.ToList() },
                new List { ListId = 2, Title = "List B", Position = 2 },
                new List { ListId = 3, Title = "List Z", Position = 3 },
            }.AsQueryable();
        }
        
        public static IQueryable<Card> GetCardData()
        {
            return new List<Card>
            {
                new Card { CardId = 1, ListId = 1, Title = "Card A", Position = 1 },
                new Card { CardId = 2, ListId = 1, Title = "Card B", Position = 2 },
                new Card { CardId = 3, ListId = 1, Title = "Card Z", Position = 3 },
            }.AsQueryable();
        }

        public static Mock<TrelloDbContext> InitBoardMoqContext(IQueryable<Board> data)
        {
            var mockSet = new Mock<DbSet<Board>>();
            mockSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Board>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Board>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.BoardId == (int)ids[0]));

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblBoard).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<TrelloDbContext> InitUserBoardMoqContext(IQueryable<UserBoard> data)
        {
            var mockSet = new Mock<DbSet<UserBoard>>();
            mockSet.As<IQueryable<UserBoard>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<UserBoard>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<UserBoard>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<UserBoard>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblUserBoard).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<DbSet<List>> InitListMoqSet(IQueryable<List> data)
        {
            var mockSet = new Mock<DbSet<List>>();
            mockSet.As<IQueryable<List>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<List>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<List>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<List>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.ListId == (int)ids[0]));

            return mockSet;
        }

        public static Mock<DbSet<Card>> InitCardMoqSet(IQueryable<Card> data)
        {
            var mockSet = new Mock<DbSet<Card>>();
            mockSet.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.CardId == (int)ids[0]));

            return mockSet;
        }

        public static Mock<TrelloDbContext> InitListMoqContext(IQueryable<List> data)
        {
            var mockSet = new Mock<DbSet<List>>();
            mockSet.As<IQueryable<List>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<List>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<List>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<List>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.ListId == (int)ids[0]));

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblList).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<TrelloDbContext> InitCardMoqContext(IQueryable<Card> data)
        {
            var mockSet = new Mock<DbSet<Card>>();
            mockSet.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.CardId == (int)ids[0]));

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblCard).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<TrelloDbContext> InitUserMoqContext(IQueryable<User> data)
        {
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.UserId == (int)ids[0]));

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblUser).Returns(mockSet.Object);

            return mockContext;
        }
    }
}