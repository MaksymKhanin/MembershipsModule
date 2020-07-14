using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


namespace Contenter.Controllers.Api
{

    public class UsersApiController : RootController
    {

        private readonly IEntityRepository<User> _repository;

        [Inject]
        public UsersApiController(IEntityRepository<User> repository) =>
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public UsersApiController() { }

        [HttpGet]
        public async Task<IHttpActionResult> GetUsersAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var users = _repository.GetItems();

                var usersList =
                    await users.ToListAsync().ConfigureAwait(false);

                return Ok(usersList);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not get a list of users", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (id < 0)
                    return BadRequest();

                var user = await _repository.GetItemAsync(id).ConfigureAwait(false);

                if (user == null) return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not get the user", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }


        [HttpPost]
        public async Task<IHttpActionResult> PostUserAsync([FromBody]User user)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    _repository.Create(user);
                    await _repository.SaveAsync().ConfigureAwait(false);

                    return Ok();
                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Could not add the user. User object was null or not valid");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not add the user", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutUserAsync([FromBody]User user)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    if (user.Id != 0)
                    {
                        _repository.Update(user);
                        await _repository.SaveAsync().ConfigureAwait(false);

                        return Ok();
                    }
                    else
                    {
                        errorBlock = MakeErrorBlock("CLO001", "Could not edit the user. Id was null");
                        return MakeCustomResponse(400, errorBlock);
                    }

                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Could not edit the user. User object was null or not valid");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not edit the user", ex);
                return MakeCustomResponse(400, errorBlock);

            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUserAsync(User user)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await _repository.DeleteAsync(user.Id).ConfigureAwait(false);
                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not delete the user", ex);
                return MakeCustomResponse(400, errorBlock);

            }

        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}