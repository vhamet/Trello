using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Xunit;
using Moq;

using Trello.Models;

namespace TrelloUnitTests
{
    public class UserBoardServiceTest
    {
        [Fact]
        public void GetUserBoards_should_return_users_board()
        {
            var data = TestUtils.GetUserBoardData(TestUtils.GetBoardData(), TestUtils.GetUserData());
            var service = new UserBoardService(TestUtils.InitUserBoardMoqContext(data).Object);
            var boards = service.GetUserBoards(2);

            Assert.True(2 == boards.Count);
            Assert.Contains(boards, b => b.BoardId == 2);
            Assert.Contains(boards, b => b.BoardId == 3);
        }

        [Fact]
        public void IsAuthorized_should_return_true_if_authorized()
        {
            var data = TestUtils.GetUserBoardData(TestUtils.GetBoardData(), TestUtils.GetUserData());
            var service = new UserBoardService(TestUtils.InitUserBoardMoqContext(data).Object);
            var shouldBeAuthorized = service.isAuthorized(1, 1);
            var shouldNotBeAuthorized = service.isAuthorized(1, 3);

            Assert.True(shouldBeAuthorized);
            Assert.False(shouldNotBeAuthorized);
        }
    }
}
