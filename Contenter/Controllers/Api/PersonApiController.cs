using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;
using Contenter.Models.ViewModels;
using System.Data.Entity;
using Ninject;

namespace Contenter.Controllers.Api
{
    public class PersonApiController : RootController
    {
        private readonly IEntityRepository<Person> _repository;

        [Inject]
        public PersonApiController(IEntityRepository<Person> repository) =>
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public PersonApiController() { }


        [HttpGet]
        public async Task<IHttpActionResult> GetPersonsAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {

                var persons =
                    await _repository.GetItems().ToListAsync().ConfigureAwait(false);

                var personsViewModels = from person in persons
                                        select new PersonViewModel
                                        {
                                            Id = person.Id,
                                            Forename = person.Forename,
                                            Sirname = person.Sirname,
                                            Email = person.Email
                                        };

                return Ok(personsViewModels);

            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not get a list of people", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPersonAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {

                if (id < 0)
                    return BadRequest();

                var person =
                    await _repository.GetItemAsync(id).ConfigureAwait(false);

                if (person == null) return NotFound();

                return Ok(new PersonViewModel
                {
                    Id = person.Id,
                    Forename = person.Forename,
                    Sirname = person.Sirname,
                    Email = person.Email
                });
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not find the person", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostPersonAsync([FromBody]Person person)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                _ = person ?? throw new ArgumentNullException(paramName: nameof(person),
                    message: "Person should not be null");

                _repository.Create(person);
                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();

            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not add the person", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutPersonAsync([FromBody]Person person)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                _ = person ?? throw new ArgumentNullException(paramName: nameof(person),
                    message: "Person should not be null");

                _repository.Update(person);

                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();

            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not update the person", ex);
                return MakeCustomResponse(400, errorBlock);

            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePersonAsync(Person person)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await _repository.DeleteAsync(person.Id).ConfigureAwait(false);
                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not delete the person", ex);
                return MakeCustomResponse(400, errorBlock);

            }

        }

        protected override void Dispose(bool disposing)
        {
            if (_repository != null)
            {
                _repository.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}
