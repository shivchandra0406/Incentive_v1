namespace Incentive.Application.Common.Interfaces
{
    /// <summary>
    /// Application logger interface
    /// </summary>
    public interface IAppLogger<T>
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}
