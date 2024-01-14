using App.Domain.DTOs;
using App.FunctionalTest.Models;
using App.FunctionalTest.Shared;
using App.SharedTest.DTOs;
using Newtonsoft.Json;
using System.Text;
using Xunit.Abstractions;
namespace App.FunctionalTest.Controllers
{
    [TestCaseOrderer(
    ordererTypeName: "App.FunctionalTest.Shared.PriorityOrderer",
    ordererAssemblyName: "App.FunctionalTest.Controllers")]
    public class UserControllerTests : BaseControllerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public UserControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact, TestPriority(1)]
        public async Task GetUsers_ReturnsAllRecords()
        {
            var client = GetClient();

            var result = await GetUsersAllAsync(client);

            Assert.Equal("OK", result.StatusCode);
            Assert.True(result.Users?.Count > 0);
        }

        [Fact, TestPriority(2)]
        public async Task GetUserById_DoesNotExist_ReturnBadRequest()
        {
            var client = GetClient();
            var getAll = await GetUsersAllAsync(client);
            var userId = getAll.Users?.Max(k => k.Id) + 1;

            var userResponse = await GetUserByIdAsync(client, userId);

            Assert.Equal("BadRequest", userResponse.StatusCode);
        }

        [Fact, TestPriority(3)]
        public async Task GetUserById_Exist_ReturnCorrectUser()
        {
            var client = GetClient();

            var getAll = await GetUsersAllAsync(client);
            var min = getAll.Users?.Min(k => k.Id) ?? 0;
            var max = getAll.Users?.Max(k => k.Id) ?? 0;
            var random = new Bogus.Randomizer();
            var userId = random.Number(min, max);

            var userResponse = await GetUserByIdAsync(client, userId);

            Assert.Equal("OK", userResponse.StatusCode);
            Assert.Equal(userId, userResponse.User?.Id);
        }

        [Fact, TestPriority(4)]
        public async Task Post_User_ReturnCorrectUser()
        {
            int userId;
            UserModel entity = null;
            var client = GetClient();
            var model = new UserFaker(0).Generate();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/User/Post", stringContent);
            
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var stringResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<UserModel>(stringResponse);
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                _testOutputHelper.WriteLine(result);
            }

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.NotNull(entity);

            userId = entity.Id;
            var userResponse = await GetUserByIdAsync(client, userId);

            Assert.Equal("OK", userResponse.StatusCode);
            Assert.Equal(model.Name, userResponse.User.Name);
            Assert.Equal(model.Email, userResponse.User.Email);
            Assert.Equal(model.Age, userResponse.User.Age);         
        }

        [Fact, TestPriority(5)]
        public async Task Put_User_ReturnCorrectUser()
        {
            var client = GetClient();

            UserModel entity = null;
            var getAll = await GetUsersAllAsync(client);
            var min = getAll.Users?.Min(k => k.Id) ?? 0;
            var max = getAll.Users?.Max(k => k.Id) ?? 0;
            var random = new Bogus.Randomizer();
            var userId = random.Number(min, max);
            var model = new UserFaker(0).Generate();

            var userResponse = await GetUserByIdAsync(client, userId);

            Assert.Equal("OK", userResponse.StatusCode);

            model.Id = userId;
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/User/Put", stringContent);

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var stringResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<UserModel>(stringResponse);
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                _testOutputHelper.WriteLine(result);
            }

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.Equal(model.Name, entity.Name);
            Assert.Equal(model.Email, entity.Email);
            Assert.Equal(model.Age, entity.Age);
        }

        [Fact, TestPriority(6)]
        public async Task Delete_User_ReturnBadRequest()
        {
            var client = GetClient();

            bool ok = false; ;
            var getAll = await GetUsersAllAsync(client);
            var min = getAll.Users?.Min(k => k.Id) ?? 0;
            var max = getAll.Users?.Max(k => k.Id) ?? 0;
            var random = new Bogus.Randomizer();
            var userId = random.Number(min, max);

            var response = await client.DeleteAsync($"/api/User/Delete/{userId}");

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var stringResponse = await response.Content.ReadAsStringAsync();
                ok = JsonConvert.DeserializeObject<bool>(stringResponse);
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                _testOutputHelper.WriteLine(result);
            }

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(ok);

            var userResponse = await GetUserByIdAsync(client, userId);

            Assert.Equal("BadRequest", userResponse.StatusCode);
        }
        private async Task<UserResponse> GetUsersAllAsync(HttpClient client)
        {
            List<UserModel> users = null;
            var response = await client.GetAsync("/api/User/GetAll");

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var stringResponse = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<UserModel>>(stringResponse)?.ToList();
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                _testOutputHelper.WriteLine(result);
            }

            var statusCode = response.StatusCode.ToString();

            return new UserResponse()
            {
                Users = users,
                StatusCode = statusCode
            };
        }

        private async Task<UserResponse> GetUserByIdAsync(HttpClient client, int? id)
        {
            UserModel user = null;
            string statusCode = null;

            if (id.HasValue)
            {
                var response = await client.GetAsync($"/api/User/{id.Value}");

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<UserModel>(stringResponse);
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    _testOutputHelper.WriteLine(result);
                }

                statusCode = response.StatusCode.ToString();
            }
            
            return new UserResponse()
            {
                User = user,
                StatusCode = statusCode
            };
        }
    }
}
