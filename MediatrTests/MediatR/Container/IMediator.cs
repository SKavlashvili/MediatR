
namespace MediatR
{
    public interface IMediator
    {
        public Task<T> Send<G, T>(G request) where G : IRequest<T>;
    }
}
