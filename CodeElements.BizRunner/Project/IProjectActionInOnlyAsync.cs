using System;
using System.Threading.Tasks;

namespace CodeElements.BizRunner.Project
{
    /// <summary>
    ///     This is an Action that takes an input and returns a Task
    /// </summary>
    /// <typeparam name="TIn">Input to the business logic</typeparam>
    public interface IProjectActionInOnlyAsync<in TIn> : IBizActionStatus
    {
        /// <summary>
        ///     Async method containing business logic that will be called
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="projectId"></param>
        Task BizActionAsync(TIn inputData, Guid projectId);
    }
}