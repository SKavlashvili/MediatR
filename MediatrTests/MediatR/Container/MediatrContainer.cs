using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR.Exceptions;
using System.Reflection.Metadata;

namespace MediatR
{
    public class MediatrContainer : IMediator
    {
        private Assembly _assembly;
        private IServiceCollection _services;
        private IServiceScope _scopedProvider;
        private List<Type> _handlerTypes;
        internal MediatrContainer(Assembly handlersAssembly, IServiceCollection services, List<Type> handlerTypes)
        {
            this._services = services;
            this._assembly = handlersAssembly;
            this._handlerTypes = handlerTypes;
            _scopedProvider = _services.BuildServiceProvider().CreateScope();
        }

        public async Task<T> Send<G, T>(G request) where G : IRequest<T>
        {
            //here throw exception if there is not handler like this
            Type appropriateType;
            try
            {
                appropriateType = _handlerTypes.Single((Type t) =>
                {
                    return MediatrContainerExtension.IsTypeChildOfInterface(t.GetInterfaces()[0].GetGenericArguments()[0],typeof(IRequest<T>)) 
                    && t.GetInterfaces()[0].GetGenericArguments()[0] == typeof(G);
                });
            }
            catch (InvalidOperationException ex)
            {
                throw new RequestHandlerNotFound();
            }
            IRequestHandler<G, T> handler = (IRequestHandler<G, T>)_scopedProvider.ServiceProvider.GetService(appropriateType);
            return await handler.Handle(request);
        }
    }

    public static class MediatrContainerExtension
    {
        public static bool IsTypeChildOfInterface(Type childType, Type interfaceType)
        {
            if (childType == null || interfaceType == null || !interfaceType.IsInterface)
                return false;

            if (interfaceType.IsAssignableFrom(childType))
                return true;

            Type[] interfaces = childType.GetInterfaces();
            foreach (Type @interface in interfaces)
            {
                if (IsTypeChildOfInterface(@interface, interfaceType))
                    return true;
            }

            return false;
        }
        public static void AddMediatr(this IServiceCollection services, Assembly assembly)
        {
            Type handlerType = typeof(IHandler);
            IEnumerable<Type> AllHandlerTypes = assembly.GetTypes().Where(t => IsTypeChildOfInterface(t, handlerType));
            List<Type> handlerTypes = new List<Type>();
            foreach (Type handler in AllHandlerTypes)
            {
                handlerTypes.Add(handler);
                services.AddScoped(handler);
            }

            services.AddScoped(typeof(IMediator), (IServiceProvider provider) =>
            {
                return new MediatrContainer(assembly, services, handlerTypes);
            });
        }
    }
}
/*
 public async Task<T> Send<G,K,T>(G request) where G : K where K : IRequest<T>
        {
            //here throw exception if there is not handler like this
            Type appropriateType;
            try
            {
                appropriateType = _handlerTypes.Single((Type t) =>
                {
                    return MediatrContainerExtension.IsTypeChildOfInterface(t.GetInterfaces()[0].GetGenericArguments()[0], typeof(IRequest<T>));
                });
            }
            catch(InvalidOperationException ex)
            {
                throw new RequestHandlerNotFound();
            }
            IRequestHandler<G, T> handler = (IRequestHandler<G, T>)_scopedProvider.ServiceProvider.GetService(appropriateType);
            return await handler.Handle(request);
        }
 */