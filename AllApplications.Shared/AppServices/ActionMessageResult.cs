namespace Core.AppServices
{
    public class ActionMessageResult
    {
        public ActionMessageResult(TaskResult result)
        {
            Result = result;
        }

        public string MessageId { get; set; }

        public TaskResult Result { get; private set; }
    }
}