using App.Infrastructure.Shared;
using App.SharedTest.DTOs;
namespace App.UnitTest.Shared
{
    public class CompareObjectTests
    {
        [Fact]
        public void Compare_TwObject_ReturnEquals()
        {
            #region Arrange
            var userRandomFaker = new UserFaker();
            var user = userRandomFaker.Generate();
            var user1 = user;
            var user2 = user;
            var compareObjectService = new CompareObjectService();
            #endregion

            #region Act
            var result = compareObjectService.Compare(user1, user2);
            #endregion

            #region Assert
            Assert.True(result);
            #endregion
        }

        [Fact]
        public void Compare_TwObject_ReturnNotEquals()
        {
            #region Arrange
            var userRandomFaker = new UserFaker();

            var user1 = userRandomFaker.Generate();
            var user2 = userRandomFaker.Generate();
            var compareObjectService = new CompareObjectService();
            #endregion

            #region Act
            var result = compareObjectService.Compare(user1, user2);
            #endregion

            #region Assert
            Assert.False(result);
            #endregion
        }

        [Fact]
        public void Compare_TwObject_ReturnNotEqualsObject()
        {
            #region Arrange
            var userRandomFaker = new UserFaker();

            var user1 = userRandomFaker.Generate();
            var user2 = new object();
            var compareObjectService = new CompareObjectService();
            #endregion

            #region Act
            var result = compareObjectService.Compare(user1, user2);
            #endregion

            #region Assert
            Assert.False(result);
            #endregion
        }
    }
}
