namespace GolfScore.Contracts
{
    public interface IAppStarter
    {
        bool CanStart(IExtraInfo info);

        void StartApp(IExtraInfo info);
    }
}
