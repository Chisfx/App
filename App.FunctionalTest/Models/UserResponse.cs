using App.Domain.DTOs;
namespace App.FunctionalTest.Models
{
    internal class UserResponse
    {
        public List<UserModel> Users { get; set; }
        public UserModel User { get; set; }
        public string StatusCode { get; set; }
    }
}
