using System.Threading.Tasks;

namespace CodeElements.BizRunner.Generic
{
    /// <summary>
    ///     This is an Action that returns a result object
    /// </summary>
    /// <typeparam name="TOut">Output of the business logic</typeparam>
    public interface IGenericActionOutOnlyAsync<TOut> : IBizActionStatus
    {
        /// <summary>
        ///     Async method containing business logic that will be called
        /// </summary>
        Task<TOut> BizActionAsync();
    }
}
