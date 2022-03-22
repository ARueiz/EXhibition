using System;

namespace EXhibition.Models
{
    public class DBConnector : ExhibitionEntities
    {
        public DBConnector() : base()
        {
            base.Configuration.ProxyCreationEnabled = false;
            Database.Connection.ConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");
        }
    }
}