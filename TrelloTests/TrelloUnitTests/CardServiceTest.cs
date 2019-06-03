using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Xunit;
using Moq;

using Trello.Models;

namespace TrelloUnitTests
{
    public class CardServiceTest
    {
        [Fact]
        public void Create_saves_a_card_via_context()
        {
            var mockSet = new Mock<DbSet<Card>>();

            var mockContext = new Mock<TrelloDbContext>();
            mockContext.Setup(m => m.tblCard).Returns(mockSet.Object);

            var service = new CardService(mockContext.Object);
            service.Create(new Card() { Title = "Card A" });
            
            mockSet.Verify(m => m.Add(It.IsAny<Card>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void UpdateTitle_should_update_Card_title()
        {
            var data = TestUtils.GetCardData();
            var mockContext = TestUtils.InitCardMoqContext(data);
            var service = new CardService(mockContext.Object);
            service.UpdateTitle(new Card() { CardId = 2, Title = "Test" });

            Assert.True(data.First(b => b.CardId == 1).Title == "Card A");
            Assert.True(data.First(b => b.CardId == 2).Title == "Test");
            Assert.True(data.First(b => b.CardId == 3).Title == "Card Z");
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Fact]
        public void UpdatePosition_should_update_Card_position()
        {
            var data = TestUtils.GetCardData();
            var mockContext = TestUtils.InitCardMoqContext(data);
            var service = new CardService(mockContext.Object);
            service.UpdatePosition(new Card() { CardId = 2, Position = 4 });

            Assert.True(data.First(b => b.CardId == 1).Position == 1);
            Assert.True(data.First(b => b.CardId == 2).Position == 4);
            Assert.True(data.First(b => b.CardId == 3).Position == 3);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
