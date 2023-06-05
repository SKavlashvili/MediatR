
namespace MediatR
{
    public interface IRequestHandler<T, G> : IHandler where T : IRequest<G>
    {
        public Task<G> Handle(T request);
    }
}
