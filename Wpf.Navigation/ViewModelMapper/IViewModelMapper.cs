using Wpf.Core.Navigation.Datas;

namespace Wpf.Navigation.ViewModelMapper
{
    public interface IViewModelMapper
    {
        void Register<TView, TViewModel>() where TView : IView where TViewModel : class;

        List<Type> GetViewTypes();

        Type? GetViewType<TView>() where TView : IView;

        Type? GetViewModelType(Type viewType);

    }
}
