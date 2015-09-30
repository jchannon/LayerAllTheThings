namespace QueryHandler
{
    public interface IMediate
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);

        TResponse Send<TResponse>(ICommand<TResponse> cmd);
    }
}
