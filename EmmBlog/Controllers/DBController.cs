using AutoMapper;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using System;

namespace EmmBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class DBController : ControllerBase
    {
        internal ILoggerManager Logger { get; private set; }
        internal IRepositoryWrapper Repository { get; private set; }
        internal IRepoLoaderWrapper RepoLoader { get; private set; }
        internal IMapper Mapper { get; private set; }

        public DBController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IRepoLoaderWrapper loader)
        {
            Logger = logger;
            Repository = repository;
            RepoLoader = loader;
            Mapper = mapper;
        }

        protected delegate IActionResult ReqUpdateDTO<ID, DTO>(ID id, DTO dto);

        protected delegate IActionResult ReqOn<T>(T id);

        protected delegate IActionResult NoParameterActionResult();

        protected IActionResult HandleOnId<T>(T g, ReqOn<T> r, IActionResult result = null, ReqOn<IActionResult> onErrorHandling = null)
        {
            try { return r((T)g); }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        protected IActionResult HandleOnDTO<ID, DTO>(ID id, DTO dto, ReqUpdateDTO<ID, DTO> r)
        {
            try { return r(id, dto); }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        protected IActionResult HandleOnDTO<T>(T dto, ReqOn<T> r, IActionResult result = null, ReqOn<IActionResult> onErrorHandling = null)
        {
            try { return r(dto); }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        protected IActionResult HandleNoParameter(NoParameterActionResult r, IActionResult result = null, ReqOn<IActionResult> onErrorHandling = null)
        {
            try
            {
                return r();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        private IActionResult DefaultErrorHandling(Exception ex)
        {
            var st = new System.Diagnostics.StackTrace();
            var sf = st.GetFrame(3); // 0 for current ; 1 for current -1
            var currentMethodName = sf.GetMethod().Name;

            Logger.LogError($"Something went wrong inside {currentMethodName} action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }

        private IActionResult HandleError(Exception ex)
        {
            // default error handling
            if (ex is RepositoryException)
            {
                return (ex as RepositoryException).Result;
            }

            // custom error handling
            else
            {
                return DefaultErrorHandling(ex);
            }
        }
    }
}