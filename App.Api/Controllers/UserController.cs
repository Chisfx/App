using App.Api.Abstractions;
using App.Application.Exceptions;
using App.Application.Features.Commands;
using App.Application.Features.Queries;
using Microsoft.AspNetCore.Mvc;
using App.Application.Features.Utils;
using App.Domain.DTOs;
namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<UserController>
    {
        [HttpPost]
        [Route("CreateUsersTest")]
        public async Task<List<UserModel>> CreateUsersTestAsync(int top = 100)
        {
            List<UserModel> list = new List<UserModel>();

            var users = await GetAllAsync(true, top);

            foreach (var user in users)
            {
                var result = await CreateUserAsync(user);
                list.Add(result);
            }

            return list;
        }

        [HttpGet]
        [Route("GetAll/{test?}")]
        public async Task<List<UserModel>> GetAllAsync(bool test = false, int top = 0)
        {
            var response = await _mediator.Send(new GetAllUserQuery() { Faker = test, Top = top });

            if (!response.Succeeded)
            {
                throw new ApiException(response.Message);
            }

            return response.Data;
        }

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

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> PostAsync([FromBody] UserModel model)
        {
            var response = await _mediator.Send(_mapper.Map<CreateUserCommand>(model));

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

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> PutAsync([FromBody] UserModel model)
        {
            var user = await CreateUserAsync(model);

            return Ok(user);
        }

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

        [HttpGet]
        [Route("GetGroupAgeTop/{top}")]
        public async Task<List<GroupAgeModel>> GetGroupAgeTopAsync(int top)
        {
            var list = await GetGroupAgeAsync();
            return list.OrderByDescending(x => x.Count).Take(top).ToList();
        }

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

        [HttpGet]
        [Route("GetGroupHostTop/{top}")]
        public async Task<List<GroupHostModel>> GetGroupHostTopAsync(int top)
        {
            var list = await GetGroupHostAsync();
            return list.OrderByDescending(x => x.Count).Take(top).ToList();
        }

        private async Task<UserModel> CreateUserAsync(UserModel model)
        {
            var response = await _mediator.Send(_mapper.Map<CreateUserCommand>(model));

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
