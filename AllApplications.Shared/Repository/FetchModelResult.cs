namespace Core
{
    public class FetchModelResult<T> : CommandResult where T : ModelBase
    {
        public T Model { get; set; }
    }
}