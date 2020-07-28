using EmmBlog.Controllers.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers.Logic.Implementations
{
    public class CredentialLogic : DBLogic, ICredentialLogic
    {
        public IActionResult GetAccountWithCredential<DTO>(Guid accountId)
        {
            var credential = Ctrl.Repository.Credential.GetAccountWithCredential(accountId);

            if (credential == null)
            {
                Ctrl.Logger.LogError($"Account with id: {accountId}, hasn't been found in db.");
                return Ctrl.NotFound();
            }
            else
            {
                Ctrl.Logger.LogInfo($"Returned acccount with id: {accountId}");

                return Ctrl.Ok(Ctrl.Mapper.Map<DTO>(credential));
            }
        }
    }
}