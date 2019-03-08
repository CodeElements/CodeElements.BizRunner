using System;
using System.Threading.Tasks;

namespace CodeElements.BizRunner
{
    /// <summary>
    ///     Handle the exception that occurred on a database save to prevent critical error
    /// </summary>
    public interface IHandleDbException
    {
        /// <summary>
        ///     Handle the exception by adding an error to the <see cref="IBizActionStatus.Errors"/>. If an error was added, the exception will be swallowed
        /// </summary>
        /// <param name="e">The exception that occurred when saving</param>
        Task HandleDbSaveException(Exception e);
    }
}
