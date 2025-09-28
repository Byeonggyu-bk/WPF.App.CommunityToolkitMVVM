namespace Wpf.Core.Navigation.Datas.Service
{
    /// <summary>
    /// View와 ViewModel의 맵핑 타입 정보를 관리하는 클래스입니다.
    /// </summary>
    internal class ViewModelMapper : IViewModelMapper
    {
        private readonly Dictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        public ViewModelMapper() { }

        /// <summary>
        /// View와 ViewModel 타입을 등록합니다.
        /// </summary>
        /// <typeparam name="TView"> View 타입</typeparam>
        /// <typeparam name="TViewModel"> ViewModel 타입</typeparam>
        public void Register<TView, TViewModel>()
            where TView : IView
            where TViewModel : class
        {
            mappings[typeof(TView)] = typeof(TViewModel);
        }

        /// <summary>
        /// View의 타입 목록을 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public List<Type> GetViewTypes() => mappings.Keys.ToList();

        /// <summary>
        /// ViewModel 타입을 가져옵니다.
        /// </summary>
        /// <param name="viewType"> view 타입</param>
        /// <returns></returns>
        public Type? GetViewModelType(Type viewType)
        {
            Type value;
            return mappings.TryGetValue(viewType, out value) ? value : null;
        }
    }
}
