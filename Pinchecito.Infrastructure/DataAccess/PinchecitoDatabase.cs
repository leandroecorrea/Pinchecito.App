using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.Repositories;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinchecito.Infrastructure.DataAccess
{
    public class PinchecitoDatabase
    {
        private SQLiteAsyncConnection database;

        public async Task<SQLiteAsyncConnection> GetDatabase()
        {
            await Init();
            return database;
        }


        public PinchecitoDatabase()
        {
        }

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath(), Constants.Flags);            
            await database.CreateTableAsync<User>();
            await database.CreateTableAsync<SessionCookie>();
            await database.CreateTableAsync<TrackedFile>();
        }
    }
}

