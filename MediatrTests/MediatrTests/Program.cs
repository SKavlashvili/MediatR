using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestConsoleApplication;

namespace TestConsoleApplication
{
    public class TestRequest : IRequest<string>
    {
        public string Name { get; set; }
    }

    public class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public async Task<string> Handle(TestRequest request)
        {
            Console.WriteLine(request.Name);
            return request.Name;
        }
    }
    public class Test2Request : IRequest<int>
    {
        public int Age { get; set; }
    }
    public class Test2RequestHandler : IRequestHandler<Test2Request, int>
    {
        public async Task<int> Handle(Test2Request request)
        {
            Console.WriteLine("Nice");
            return 12;
        }
    }
    public class Test3Request : IRequest<int>
    {
        public string TestData { get; set; }
    }
    public class Test3RequestHandler : IRequestHandler<Test3Request, int>
    {
        public async Task<int> Handle(Test3Request request)
        {
            Console.WriteLine(request.TestData);
            return 123;
        }
    }
    public class Program
    {
        public static async Task Main()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMediatr(typeof(Program).Assembly);
            IServiceProvider x = services.BuildServiceProvider();
            IMediator mediator = x.GetService<IMediator>();
            TestRequest request = new TestRequest() { Name = "Test" };
            Test2Request req = new Test2Request() { Age = 19 };
            Test3Request req3 = new Test3Request() { TestData = "Test" };
            await mediator.Send<TestRequest, string>(request);
            await mediator.Send<Test2Request, int>(req);
            Console.WriteLine(await mediator.Send<Test3Request, int>(req3));
        }
    }
}