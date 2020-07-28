using EmmBlog.Controllers.Interfaces;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataTransferObjects;
using Entities.EmmBlog.DataTransferObjects.In;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using System;

namespace EmmBlog.Controllers.Logic.Implementations
{
    public class AccountLogic : DBLogic, IAccountLogic
    {
        public IActionResult CreateAccount(AccountForCreationDTO dto)
        {
            if (dto == null)
            {
                Ctrl.Logger.LogError("Owner object sent from client is null.");
                return Ctrl.BadRequest("Owner object is null");
            }

            if (!Ctrl.ModelState.IsValid)
            {
                Ctrl.Logger.LogError("Invalid owner object sent from client.");
                return Ctrl.BadRequest("Invalid model object");
            }

            var credential = Ctrl.Repository.Account.GetAccountCredential(dto.Id);
            if (credential != null)
            {
                string mailAddress = dto.Credential.MailAddress;
                string message = $"An account with the email {mailAddress} is already registered ";

                throw new RepositoryException(Ctrl.Conflict(message));
            }

            dto.Credential.RegistrationDate = DateTime.Now;
            var accountEntity = Ctrl.Mapper.Map<Account>(dto);

            accountEntity.Credential.AccountId = accountEntity.Id;

            Ctrl.Repository.Account.CreateAccount(accountEntity);
            Ctrl.Repository.Save();

            var createdAccount = Ctrl.Mapper.Map<AccountDTO>(accountEntity);

            return Ctrl.CreatedAtRoute("GetAccountById", new { id = createdAccount.Id }, createdAccount);
        }

        public IActionResult GetAccountById(Guid id)
        {
            var account = Ctrl.Repository.Account.GetAccountById(id);

            if (account == null)
            {
                Ctrl.Logger.LogError($"Account with id: {id}, hasn't been found in db.");
                return Ctrl.NotFound();
            }
            else
            {
                Ctrl.Logger.LogInfo($"Returned acccount with id: {id}");

                var ownerResult = Ctrl.Mapper.Map<AccountDTO>(account);
                return Ctrl.Ok(ownerResult);
            }
        }

        public IActionResult DeleteAccount(Guid id)
        {
            var account = Ctrl.Repository.Account.GetAccountById(id);
            if (account == null) //owner.IsEmptyObject())
            {
                Ctrl.Logger.LogError($"Account with id: {id}, hasn't been found in db.");
                return Ctrl.NotFound();
            }
            //if (Ctrl.Repository.Account.AccountsByOwner(id).Any())
            /*{
                Ctrl.Logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                return Ctrl.BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
            }*/

            Ctrl.Repository.Account.DeleteAccount(account);
            Ctrl.Repository.Save();

            return Ctrl.NoContent();
        }

        private void handleExisting(Exception ex)
        {
        }
    }
}