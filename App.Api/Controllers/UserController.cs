using App.Api.Abstractions;
using App.Application.Exceptions;
using App.Application.Features.Commands;
using App.Application.Features.Queries;
using Microsoft.AspNetCore.Mvc;
using App.Application.Features.Utils;
using App.Domain.DTOs;
using AspNetCoreHero.Results;
namespace App.Api.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : BaseController<UserController>
    {
        /// <summary>
        /// Creates test users.
        /// </summary>
        /// <param name="top">The number of users to create.</param>
        /// <returns>A list of created users.</returns>
        [HttpPost]
        [Route("CreateUsersTest")]
        public async Task<List<UserModel>> CreateUsersTestAsync(int top = 100)
        {
            List<UserModel> list = new List<UserModel>();

            var users = await GetAllAsync(true, top);

            foreach (var user in users)
            {
                var result = await CreateOrUpdateUserAsync(user);
                list.Add(result);
            }

            return list;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="test">Indicates if the data is fake.</param>
        /// <param name="top">The number of users to retrieve.</param>
        /// <returns>A list of users.</returns>
        [HttpGet]
        [Route("GetAll/{test?}/{top?}")]
        public async Task<List<UserModel>> GetAllAsync(bool test = false, int top = 0)
        {
            var response = await _mediator.Send(new GetAllUserQuery() { Faker = test, Top = top });

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }

            return response.Data;
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery() { Id = id });

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }
            else if (!string.IsNullOrEmpty(response.Message))
            {
                throw new ApiException(response.Message);
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns>The created user.</returns>
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> PostAsync([FromBody] UserModel model)
        {
            var user = await CreateOrUpdateUserAsync(model);

            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="model">The updated user model.</param>
        /// <returns>The updated user.</returns>
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> PutAsync([FromBody] UserModel model)
        {
            var user = await CreateOrUpdateUserAsync(model);

            return Ok(user);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>The deleted user.</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _mediator.Send(new DeleteUserCommand() { Id = id });

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }
            else if (!string.IsNullOrEmpty(response.Message))
            {
                throw new ApiException(response.Message);
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Gets the group age statistics.
        /// </summary>
        /// <returns>A list of group age statistics.</returns>
        [HttpGet]
        [Route("GetGroupAge")]
        public async Task<List<GroupAgeModel>> GetGroupAgeAsync()
        {
            var response = await _mediator.Send(new GetGroupAgeQuery());

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }

            return response.Data;
        }

        /// <summary>
        /// Gets the top group age statistics.
        /// </summary>
        /// <param name="top">The number of statistics to retrieve.</param>
        /// <returns>The top group age statistics.</returns>
        [HttpGet]
        [Route("GetGroupAgeTop/{top}")]
        public async Task<List<GroupAgeModel>> GetGroupAgeTopAsync(int top)
        {
            var list = await GetGroupAgeAsync();
            return list.OrderByDescending(x => x.Count).Take(top).ToList();
        }

        /// <summary>
        /// Gets the group host statistics.
        /// </summary>
        /// <returns>A list of group host statistics.</returns>
        [HttpGet]
        [Route("GroupHost")]
        public async Task<List<GroupHostModel>> GetGroupHostAsync()
        {
            var response = await _mediator.Send(new GetGroupHostQuery());

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }

            return response.Data;
        }

        /// <summary>
        /// Gets the top group host statistics.
        /// </summary>
        /// <param name="top">The number of statistics to retrieve.</param>
        /// <returns>The top group host statistics.</returns>
        [HttpGet]
        [Route("GetGroupHostTop/{top}")]
        public async Task<List<GroupHostModel>> GetGroupHostTopAsync(int top)
        {
            var list = await GetGroupHostAsync();
            return list.OrderByDescending(x => x.Count).Take(top).ToList();
        }

        private async Task<UserModel> CreateOrUpdateUserAsync(UserModel model)
        {
            Result<UserModel> response;

            if (model.Id > 0)
            {
                response = await _mediator.Send(_mapper.Map<UpdateUserCommand>(model));
            }
            else
            {
                response = await _mediator.Send(_mapper.Map<CreateUserCommand>(model));
            }

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }
            else if (!string.IsNullOrEmpty(response.Message))
            {
                throw new ApiException(response.Message);
            }

            return response.Data;
        }
    }
}
