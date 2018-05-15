using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CodeElements.BizRunner.Generic;
using CodeElements.BizRunner.Project;

namespace CodeElements.BizRunner
{
    /// <summary>
    ///     This interface defines all various features for error reporting and status items that the business logic must
    ///     implement
    /// </summary>
    public interface IBizActionStatus
    {
        /// <summary>
        ///     Contains list of errors
        /// </summary>
        IImmutableList<ValidationResult> Errors { get; }

        /// <summary>
        ///     Is true if there are errors
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        ///     This contains a human readable message telling you whether the business logic succeeded or failed
        /// </summary>
        string Message { get; set; }
    }

    public class ActionServiceInOutAsync<TBizIn, TBizOut> : ActionServiceBase
    {
        private readonly IGenericActionAsync<TBizIn, TBizOut> _action;
        private readonly DbContext _context;

        internal ActionServiceInOutAsync(bool requireSaveChanges, IGenericActionAsync<TBizIn, TBizOut> action,
            DbContext context) : base(requireSaveChanges)
        {
            _action = action;
            _context = context;
        }

        public async Task<TBizOut> ExecuteAsync(TBizIn input)
        {
            var result = await _action.BizActionAsync(input);
            await SaveChangedIfRequiredAndNoErrorsAsync(_context, _action); //this already checks for errors
            if (_action.HasErrors)
                return default;

            return result;
        }
    }

    public class ProjectServiceInOutAsync<TBizIn, TBizOut> : ActionServiceBase
    {
        private readonly IProjectActionAsync<TBizIn, TBizOut> _action;
        private readonly DbContext _context;

        internal ProjectServiceInOutAsync(bool requireSaveChanges, IProjectActionAsync<TBizIn, TBizOut> action,
            DbContext context) : base(requireSaveChanges)
        {
            _action = action;
            _context = context;
        }

        public async Task<TBizOut> ExecuteAsync(TBizIn input, Guid projectId)
        {
            var result = await _action.BizActionAsync(input, projectId);
            await SaveChangedIfRequiredAndNoErrorsAsync(_context, _action); //this already checks for errors
            if (_action.HasErrors)
                return default;

            return result;
        }
    }

    public class ProjectServiceOutOnlyAsync<TBizOut> : ActionServiceBase
    {
        private readonly IProjectActionOutOnlyAsync<TBizOut> _action;
        private readonly DbContext _context;

        internal ProjectServiceOutOnlyAsync(bool requireSaveChanges, IProjectActionOutOnlyAsync<TBizOut> action,
            DbContext context) : base(requireSaveChanges)
        {
            _action = action;
            _context = context;
        }

        public async Task<TBizOut> ExecuteAsync(Guid projectId)
        {
            var result = await _action.BizActionAsync(projectId);
            await SaveChangedIfRequiredAndNoErrorsAsync(_context, _action); //this already checks for errors
            if (_action.HasErrors)
                return default;

            return result;
        }
    }

    public static class ActionExtensions
    {
        public static ActionServiceInOutAsync<TIn, TOut> ToRunner<TIn, TOut>(
            this IGenericActionAsync<TIn, TOut> businessAction, DbContext context)
        {
            return new ActionServiceInOutAsync<TIn, TOut>(businessAction is IGenericActionWriteDbAsync<TIn, TOut>,
                businessAction, context);
        }

        public static ProjectServiceInOutAsync<TIn, TOut> ToRunner<TIn, TOut>(
            this IProjectActionAsync<TIn, TOut> businessAction, DbContext context)
        {
            return new ProjectServiceInOutAsync<TIn, TOut>(businessAction is IProjectActionWriteDbAsync<TIn, TOut>,
                businessAction, context);
        }
    }

    public class ActionServiceBase
    {
        protected ActionServiceBase(bool requireSaveChanges)
        {
            RequireSaveChanges = requireSaveChanges;
        }

        /// <summary>
        /// This contains info on whether SaveChanges (with validation) should be called after a succsessful business logic has run
        /// </summary>
        private bool RequireSaveChanges { get; }

        /// <summary>
        /// This a) handled optional save to database and b) calling SetupSecondaryData if there are any errors
        /// It also makes sure that the runStatus is used at the primary return so that warnings are passed on.
        /// Note: if it did save successfully to the database it alters the message
        /// </summary>
        /// <param name="db"></param>
        /// <param name="bizStatus"></param>
        /// <returns></returns>
        protected async Task SaveChangedIfRequiredAndNoErrorsAsync(DbContext db, IBizActionStatus bizStatus)
        {
            if (!bizStatus.HasErrors && RequireSaveChanges)
            {
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}