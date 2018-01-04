namespace CoreServiceUtils.Interfaces
{
    public interface IHostWriter
    {
        void Write(string msg, bool writeToLog = true);
        void Warn(string msg, bool writeToLog = true);
        void Error(string msg, bool writeToLog = true);
        void Success(string msg, bool writeToLog = true);
    }
}
