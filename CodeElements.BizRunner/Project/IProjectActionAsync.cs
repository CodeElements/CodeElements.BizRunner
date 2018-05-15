using System;
using System.Threading.Tasks;

namespace CodeElements.BizRunner.Project
{
    /// <summary>
    ///     This is an async Action that takes an input and returns a Task containing the result TOut
    /// </summary>
    /// <typeparam name="TIn">Input to the business logic</typeparam>
    /// <typeparam name="TOut">Output from the business logic</typeparam>
    public interface IProjectActionAsync<in TIn, TOut> : IBizActionStatus
    {
        /// <summary>
        ///     Async method containing business logic that will be called
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="projectId"></param>
        Task<TOut> BizActionAsync(TIn inputData, Guid projectId);
    }

    /// <summary>
    /// This is an async Action that returns a Task containing a status with a result TOut
    /// </summary>
    /// <typeparam name="TOut">Output from the business logic</typeparam>
    public interface IProjectActionOutOnlyAsync<TOut> : IBizActionStatus
    {
        /// <summary>
        /// Async method containing business logic that will be called
        /// </summary>
        /// <returns>Task containing result, or default value if fails</returns>
        Task<TOut> BizActionAsync(Guid projectId);
    }
}