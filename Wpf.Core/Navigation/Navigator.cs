using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Core.Navigation.Datas;
using Wpf.Core.Navigation.Datas.Service;

namespace Wpf.Core.Navigation
{
    /// <summary>
    /// View, ViewModel을 ServiceCollection에 등록하는 추상클래스입니다.
    /// </summary>
    public abstract class Navigator
    {
        protected readonly IViewModelMapper Mapper;

        protected readonly IServiceCollection services;

        protected Navigator()
        {
            Mapper = new ViewModelMapper();

            services = new ServiceCollection();
        }

        public void Run()
        {
            RegisterViewModels(Mapper);

            Registers();
        }

        /// <summary>
        /// view와 ViewModel의 맵핑 타입 등록합니다.
        /// </summary>
        /// <param name="viewModelMapper">view와 viewModel 맵핑정보 관리</param>
        protected abstract void RegisterViewModels(IViewModelMapper viewModelMapper);

        /// <summary>
        /// ServiceCollection에 등록합니다.
        /// </summary>
        private void Registers()
        {

            var types = Mapper.GetViewTypes();

            types.ForEach(x => services.AddSingleton(x));
            foreach (var type in types)
            {
                var viewmodelType = Mapper.GetViewModelType(type);
                if (viewmodelType is null) continue;

                services.AddSingleton(viewmodelType);
            }

            var provider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(provider);

            var method = typeof(ServiceProviderServiceExtensions)
                .GetMethod(
                "GetRequiredService",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                types: new[] { typeof(IServiceProvider), typeof(Type) });

            foreach (var type in types)
            {
                var viewmodelType = Mapper.GetViewModelType(type);
                if (viewmodelType is null) continue;

                if (method?.Invoke(null, new object[] { provider, type}) is IView view)
                {
                    object? viewModel = method?.Invoke(null, new object[] { provider, viewmodelType});

                    view.DataContext = viewModel;
                }

            }
        }
    }
}
