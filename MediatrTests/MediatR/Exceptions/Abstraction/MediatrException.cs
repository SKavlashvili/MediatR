namespace MediatR.Exceptions.Abstractions
{
    public class MediatrException : Exception
    {
        public MediatrException(string message) : base(message)
        {

        }
    }
}
