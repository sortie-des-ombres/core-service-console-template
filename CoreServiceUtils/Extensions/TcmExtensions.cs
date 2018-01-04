using System;

namespace CoreServiceUtils.Extensions
{
    public static class TcmExtensions
    {

        public static string GetWebdavPath(this string path, string publicationName)
        {
            return $"/webdav/{publicationName}{path}";
        }

        public static string GetParentFolderPath(this string tempFolderWebdavPath)
        {
            int index = tempFolderWebdavPath.LastIndexOf("/", StringComparison.CurrentCulture);

            if (index <= 0)
            {
                return tempFolderWebdavPath;
            }

            return tempFolderWebdavPath.Substring(0, index);
        }

        public static string GetFolderNameFromPath(this string tempFolderWebdavPath)
        {
            int index = tempFolderWebdavPath.LastIndexOf("/", StringComparison.CurrentCulture);

            if (index <= 0)
            {
                return tempFolderWebdavPath;
            }

            return tempFolderWebdavPath.Substring(index + 1);
        }
    }
}
