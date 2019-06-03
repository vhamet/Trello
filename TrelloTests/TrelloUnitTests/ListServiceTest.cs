using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Xunit;
using Moq;

using Trello.Models;

namespace TrelloUnitTests
{
    public class ListServiceTest
    {
        [Fact]
        public void Create_saves_a_list_via_context()
        {
            var mockSet = new Mock<DbSet<List>>();

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(m => m.tblList).Returns(mockSet.Object);

            var service = new ListService(mockContext.Object);
            service.Create(new List() { Title = "List A" });
            
            mockSet.Verify(m => m.Add(It.IsAny<List>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void UpdateTitle_should_update_list_title()
        {
            var data = TestUtils.GetListData();
            var mockContext = TestUtils.InitListMoqContext(data);
            var service = new ListService(mockContext.Object);
            service.UpdateTitle(new List() { ListId = 2, Title = "Test" });

            Assert.True(data.First(b => b.ListId == 1).Title == "List A");
            Assert.True(data.First(b => b.ListId == 2).Title == "Test");
            Assert.True(data.First(b => b.ListId == 3).Title == "List Z");
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void UpdatePosition_should_update_list_position()
        {
            var data = TestUtils.GetListData();
            var mockContext = TestUtils.InitListMoqContext(data);
            var service = new ListService(mockContext.Object);
            service.UpdatePosition(new List() { ListId = 2, Position = 4 });

            Assert.True(data.First(b => b.ListId == 1).Position == 1);
            Assert.True(data.First(b => b.ListId == 2).Position == 4);
            Assert.True(data.First(b => b.ListId == 3).Position == 3);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void Delete_removes_a_list_and_nested_cards_via_context()
        {
            var cards = TestUtils.GetCardData();
            var mockSetCard = TestUtils.InitCardMoqSet(cards);
            var lists = TestUtils.GetNestedListData(cards);
            var mockSetList = TestUtils.InitListMoqSet(lists);

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(c => c.tblCard).Returns(mockSetCard.Object);
            mockContext.Setup(c => c.tblList).Returns(mockSetList.Object);

            var service = new ListService(mockContext.Object);
            service.Delete(new List() { ListId = 1 });
            
            mockSetCard.Verify(m => m.Remove(It.IsAny<Card>()), Times.Exactly(3));
            mockSetList.Verify(m => m.Remove(It.IsAny<List>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
