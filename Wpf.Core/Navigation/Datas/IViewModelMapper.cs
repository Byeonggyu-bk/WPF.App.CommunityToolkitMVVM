
namespace Wpf.Core.Navigation.Datas
{
    /// <summary>
    /// View와 ViewModel의 맵핑 타입 정보를 관리하는 클래스의 인터페이스입니다.
    /// </summary>
    public interface IViewModelMapper
    {
        /// <summary>
        /// View와 ViewModel 타입을 등록합니다.
        /// </summary>
        /// <typeparam name="TView"> View 타입</typeparam>
        /// <typeparam name="TViewModel"> ViewModel 타입</typeparam>
        void Register<TView, TViewModel>() where TView : IView where TViewModel : class;

        /// <summary>
        /// View의 타입 목록을 가져옵니다.
        /// </summary>
        /// <returns></returns>
        List<Type> GetViewTypes();

        /// <summary>
        /// ViewModel 타입을 가져옵니다.
        /// </summary>
        /// <param name="viewType"> view 타입</param>
        /// <returns></returns>
        Type? GetViewModelType(Type viewType);

    }
}
