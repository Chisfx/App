using App.Domain.DTOs;
using App.SharedTest.DTOs;
namespace App.UnitTest.DTOs
{
    public class UserTests : BaseTests
    {
        [Theory]
        [ClassData(typeof(UserData))]
        public void Validate_Model_ReturnNumberErrors(UserModel model, int numberExpectedErrors)
        {
            var errorList = ValidateModel(model);

            Assert.Equal(numberExpectedErrors, errorList.Count);
        }
    }
}
