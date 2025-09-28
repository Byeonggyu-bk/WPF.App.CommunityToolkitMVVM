using Wpf.Core.Navigation.Datas;

namespace Wpf.Navigation.ViewModelMapper
{
    internal class ViewModelMapper : IViewModelMapper
    {
        private readonly Dictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        public ViewModelMapper() { }

        public void Register<TView, TViewModel>()
            where TView : IView
            where TViewModel : class
        {
            mappings[typeof(TView)] = typeof(TViewModel);
        }

        public List<Type> GetViewTypes() => mappings.Keys.ToList();

        public Type? GetViewType<TView>() where TView : IView
        {
            return mappings.Keys.FirstOrDefault(x => x == typeof(TView));
        }

        public Type? GetViewModelType(Type viewType)
        {
            Type value;
            return mappings.TryGetValue(viewType, out value) ? value : null;
        }
    }
}
