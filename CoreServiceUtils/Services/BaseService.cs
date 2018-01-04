using log4net;

namespace CoreServiceUtils.Services
{
    public abstract class BaseService
    {

        protected ILog Log { get; }

        public BaseService(string logName)
        {
            Log =
                LogManager.GetLogger(logName);
        }
    }
}
