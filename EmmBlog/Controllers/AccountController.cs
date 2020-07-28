using AutoMapper;
using Contracts;
using EmmBlog.Controllers.Interfaces;
using EmmBlog.Controllers.Logic.Implementations;
using EmmBlog.Controllers.Logic.Interfaces;
using Entities.EmmBlog.DataTransferObjects.In;
using Entities.EmmBlog.DataTransferObjects.Out.Reduced;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers
{
    [Route("api.net")]
    [ApiController]
    public class AccountController : DBController
    {
        private IAccountLogic AccountLgc { get; set; }
        private ICredentialLogic CredentialLgc { get; set; }

        public AccountController(

            ILoggerManager logger,
            IRepositoryWrapper repository,
            IMapper mapper,
            IRepoLoaderWrapper repoLoader

        ) : base(logger, repository, mapper, repoLoader)
        {
            AccountLgc = new AccountLogic();
            (AccountLgc as AccountLogic).Ctrl = this;
            CredentialLgc = new CredentialLogic();
            (CredentialLgc as CredentialLogic).Ctrl = this;
        }

        [HttpGet]
        public IActionResult _Ok()
        {
            Logger.LogInfo("Good here");
            return Ok("ça marche");
        }

        [HttpGet("cred/{id}", Name = "GetAccountById")]
        public IActionResult GetAccountById(Guid id)
        {
            var credentials = HandleOnId(id, CredentialLgc.GetAccountWithCredential<ProfileAccountDTO>); ;

            return credentials;
        }

        [HttpPost("account/register")]
        public IActionResult CreateAccount([FromBody]AccountForCreationDTO account)
        {
            return HandleOnDTO(account, AccountLgc.CreateAccount);
        }

        [HttpDelete("account/{id}")]
        public IActionResult DeleteAccount(Guid id)
        {
            return HandleOnId(id, AccountLgc.DeleteAccount);
        }
    }
}