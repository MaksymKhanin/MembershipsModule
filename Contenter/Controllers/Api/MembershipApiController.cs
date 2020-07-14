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
    public class MembershipApiController : RootController
    {
        private readonly IEntityRepository<Membership> _repository;

        [Inject]
        public MembershipApiController(IEntityRepository<Membership> repository) =>
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public MembershipApiController() { }
       
        [HttpGet]
        public async Task<IHttpActionResult> GetMembershipsAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var memberships = _repository.GetItems();

                var membershipsList =
                    await memberships.ToListAsync().ConfigureAwait(false);

                return Ok(membershipsList);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not get a list of memberships", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetMembershipsAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (id < 0)
                    return BadRequest();

                var memberships =
                    await _repository.GetWithInclude(x => x.PersonId == id, p => p.Person).ToListAsync().ConfigureAwait(false);

                var membershipViewModels = from membership in memberships
                                           select new MembershipViewModel
                                           {
                                               Id = membership.Id,
                                               AccountBalance = membership.AccountBalance,
                                               MemebrshipNumber = membership.MemebrshipNumber,
                                               Type = membership.Type.ToString()
                                           };


                return Ok(membershipViewModels);

            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not get a list of memberships", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetMembershipAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (id < 0)
                    return BadRequest();

                Membership membership =
                    await _repository.GetItemAsync(id).ConfigureAwait(false);

                if (membership == null) return NotFound();

                return Ok(membership);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not find a membership", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostMembershipAsync([FromBody]MembershipViewModel membershipViewModel)
        {

            var errorBlock = new ResponseMessage();
            try
            {

                _ = membershipViewModel ?? throw new ArgumentNullException(paramName: nameof(membershipViewModel),
                    message: "MembershipViewModel should not be null");

                var membership = new Membership();
                membership.Id = membershipViewModel.Id;
                membership.MemebrshipNumber = (Int32)membershipViewModel.MemebrshipNumber;
                membership.PersonId = membershipViewModel.PersonId;
                membership.Type = (MembershipType)Enum.Parse(typeof(MembershipType), membershipViewModel.Type);
                membership.AccountBalance = membershipViewModel.AccountBalance;

                _repository.Create(membership);
                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();


            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not add a membership", ex);
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutMembershipAsync([FromBody]MembershipViewModel membershipViewModel)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                _ = membershipViewModel ?? throw new ArgumentNullException(paramName: nameof(membershipViewModel),
                    message: "MembershipViewModel should not be null");

                var membership = new Membership();
                membership.Id = membershipViewModel.Id;
                membership.MemebrshipNumber = (Int32)membershipViewModel.MemebrshipNumber;
                membership.PersonId = membershipViewModel.PersonId;
                membership.Type = (MembershipType)Enum.Parse(typeof(MembershipType), membershipViewModel.Type);
                membership.AccountBalance = membershipViewModel.AccountBalance;

                _repository.Update(membership);

                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();

            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not add a membership", ex);
                return MakeCustomResponse(400, errorBlock);

            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteMembershipAsync(Membership membership)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await _repository.DeleteAsync(membership.Id).ConfigureAwait(false);
                await _repository.SaveAsync().ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Could not delete a membership", ex);
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
