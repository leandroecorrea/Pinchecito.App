using System;

namespace Pinchecito.Infrastructure.Repositories
{
    public static class Constants
    {
        public const string DatabaseFilename = "PinchecitoDB.db3";

        public const SQLite.SQLiteOpenFlags Flags =            
            SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath()
        {
            try
            {
                var appdataDirectory = FileSystem.CacheDirectory;                
                return Path.Combine(appdataDirectory, DatabaseFilename);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
