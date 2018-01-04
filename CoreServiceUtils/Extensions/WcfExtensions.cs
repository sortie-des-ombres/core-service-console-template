using System;
using CoreServiceUtils.Interfaces;

namespace CoreServiceUtils.Extensions
{
    public static class WcfExtensions
    {
        public static void Using<T>(this T client, Action<T> work)
            where T :  ITridionClientService
        {
            try
            {
                client.Open();
                work(client);
                client.Close();
            }
            catch
            {
                client.Abort();
                throw;
            }
        }
    }
}
