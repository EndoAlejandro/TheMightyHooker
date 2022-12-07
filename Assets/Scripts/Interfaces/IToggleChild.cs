namespace Interfaces
{
    public interface IToggleChild
    {
        IToggle Toggle { get; }
        bool State { get; }
        void OnToggle(bool value);
    }
}