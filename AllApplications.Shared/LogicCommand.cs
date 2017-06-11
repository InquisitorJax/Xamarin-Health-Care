using System;
using System.Threading.Tasks;

namespace Core
{
    public interface IAsyncLogicCommand<TIn, TOut> : ILogicCommand<TIn, TOut> where TOut : CommandResult
    {
        Task<TOut> ExecuteAsync(TIn request);
    }

    public interface ILogicCommand<in TIn, out TOut>
    {
        TOut Execute(TIn request);
    }

    /// <summary>
    /// Base class for which async commands can inherit.
    /// Created to enforce single responsibility design principal in application logic implementation
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public abstract class AsyncLogicCommand<TIn, TOut> : LogicCommand<TIn, TOut>, IAsyncLogicCommand<TIn, TOut> where TOut : CommandResult
    {
        public override TOut Execute(TIn request)
        {
            TOut result = ExecuteAsync(request).Result;
            return result;
        }

        public abstract Task<TOut> ExecuteAsync(TIn request);
    }

    public class CommandResult
    {
        public CommandResult()
        {
            Notification = Notification.Success();
        }

        public Notification Notification { get; set; }

        public virtual bool IsValid(NotificationSeverity severity = NotificationSeverity.Error)
        {
            return Notification.IsValid(severity);
        }
    }

    /// <summary>
    /// Base class for which commands can inherit.
    /// Created to enforce single responsibility design principal in application logic implementation
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public abstract class LogicCommand<TIn, TOut> : ILogicCommand<TIn, TOut> where TOut : CommandResult
    {
        public abstract TOut Execute(TIn request);
    }
}