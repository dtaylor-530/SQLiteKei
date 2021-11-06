using System.Reactive.Subjects;
using Utility.Common.Contracts;

namespace Utility.Service.Converter
{

    public class MenuWindowConverter : IWindowConverter
    {
        private readonly ReplaySubject<WindowRequest> subject = new(1);

        public MenuWindowConverter(IMenuWindowService menuWindowService)
        {
            subject.Subscribe(a =>
            {
                (a switch
                {
                    { WindowType: WindowType.About } => new Action(menuWindowService.OpenAbout),
                    { WindowType: WindowType.Preferences } => menuWindowService.OpenPreferences,
                    _ => throw new NotImplementedException(),
                }).Invoke();
            });
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(WindowRequest value)
        {
            subject.OnNext(value);
        }
    }
}
