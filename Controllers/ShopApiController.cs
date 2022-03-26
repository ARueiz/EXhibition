using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace EXhibition.Controllers
{
    public class ShopApiController : ApiController
    {

        Models.DBConnector db = new Models.DBConnector();

        public IHttpActionResult GetTicketList()
        {
            var b = (from eve in db.events orderby eve.startdate descending select eve).Take(12).ToList();
            return Json(b);
        }

        public IHttpActionResult GetNewTicketList()
        {
            var b = (from eve in db.events orderby eve.startdate descending select eve).Take(4).ToList();
            return Json(b);
        }

        public IHttpActionResult GetHotTicketList()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select top(5) count(A.TID) , B.name " +
                "from Tickets as A inner join events as B on A.EVID = B.EVID " +
                "group by B.EVID , B.name order by 1 desc";
           
            List<Models.events> eventlist = new List<Models.events>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        eventlist.Add(new Models.events() { 

                        });
                        Console.WriteLine("\t{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Json(eventlist);
        }

    }
}
