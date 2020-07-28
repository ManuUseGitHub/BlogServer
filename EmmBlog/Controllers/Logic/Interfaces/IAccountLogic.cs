using Entities.EmmBlog.DataTransferObjects.In;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers.Interfaces
{
    internal interface IAccountLogic
    {
        public IActionResult GetAccountById(Guid id);

        IActionResult CreateAccount(AccountForCreationDTO entity);

        IActionResult DeleteAccount(Guid id);
    }
}