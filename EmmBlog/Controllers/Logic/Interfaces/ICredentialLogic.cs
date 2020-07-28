using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers.Logic.Interfaces
{
    internal interface ICredentialLogic
    {
        public IActionResult GetAccountWithCredential<DTO>(Guid accountId);
    }
}