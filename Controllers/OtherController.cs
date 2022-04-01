﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EXhibition.Models;

namespace EXhibition.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OtherController : ApiController
    {

        DBConnector db = new DBConnector();

        public IHttpActionResult GetTagList()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTSTRING");

            string queryString =
                "select top(10) count(A.tagId) , A.TagId , B.tagName from eventTags as A inner join TagsName as B on A.tagID = B.id group by A.tagId ,B.tagName order by 1 desc";

            // 先將 id 撈成 陣列後 用 entity framework 去找資料

            List<TagsName> tagList = new List<TagsName>();
            List<int> eventIdList = new List<int>();

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
                        eventIdList.Add((int)reader[1]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            List<string> tList = db.TagsName.Where(item => eventIdList.Contains(item.id)).Select(e=>e.tagName).ToList();

            return Ok(tList);
        }
    }
}