using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using Wpf.Core.Navigation.Datas;
using Wpf.Navigation.ViewModelMapper;

namespace Wpf.Navigation
{
    public abstract class Navigator
    {
        protected readonly IViewModelMapper Mapper;

        protected readonly IServiceCollection services;

        protected Navigator()
        {
            Mapper = new Wpf.Navigation.ViewModelMapper.ViewModelMapper();

            services = new ServiceCollection();
        }

        public void Run()
        {
            Registers(Mapper);

            Registers();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        public IView GetView(Type type)
        {
            var viewType = Mapper.GetViewTypes().FirstOrDefault(x => x == type);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IView v = (IView)serviceProvider.GetRequiredService(type);
            if (v.DataContext != null) return v;

            var viewmodelType = Mapper.GetViewModelType(type);
            if (viewmodelType != null)
            {
                v.DataContext = serviceProvider.GetRequiredService(viewmodelType);
            }

            return v;
        }

        public IView GetView<TView>() where TView : IView
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var v = serviceProvider.GetRequiredService<TView>();
            if (v.DataContext != null) return v;

            var viewmodelType = Mapper.GetViewModelType(typeof(TView));
            if(viewmodelType != null)
            {
                v.DataContext = serviceProvider.GetRequiredService(viewmodelType);
            }

            return v;
        }

        //public object? GetViewModel(Type type)
        //{
        //    var view = GetView(type);

        //    return view.DataContext;
        //}

        protected abstract void Registers(IViewModelMapper viewModelMapper);

        private void Registers()
        {
            var types = Mapper.GetViewTypes();

            types.ForEach(x => services.AddSingleton(x));
            foreach(var type in types)
            {
                var viewmodelType = Mapper.GetViewModelType(type);
                if (viewmodelType is null) continue;

                services.AddSingleton(viewmodelType);
            }
        }
    }
}
