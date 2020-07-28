using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Repository.Exceptions
{
    public class RepositoryException : DbUpdateException
    {
        public ObjectResult Result { get; private set; }

        public RepositoryException(IActionResult result) : base(result.ToString())
        {
            Result = (ObjectResult)result;
        }
    }

    public class NonPublicVisibilityException : InvalidOperationException
    {
        public NonPublicVisibilityException(string message) : base(message)
        { }
    }
}