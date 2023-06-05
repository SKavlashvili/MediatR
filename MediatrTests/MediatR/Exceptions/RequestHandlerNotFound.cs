using MediatR.Exceptions.Abstractions;

namespace MediatR.Exceptions
{
    public class RequestHandlerNotFound : MediatrException
    {
        public RequestHandlerNotFound() : base("There is not message request with this abstraction")
        {

        }
    }
}
