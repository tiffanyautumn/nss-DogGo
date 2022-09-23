using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DogGo.Models;
using System.Collections.Generic;
using Microsoft.Identity.Client;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Date, Duration, WalkerId, DogId FROM Neighborhood";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Walk> walks = new List<Walk>();

                        while (reader.Read())
                        {
                            Walk walk = new Walk()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                            };
                            walks.Add(walk);
                        }

                        return walks;
                    }
                }
            }

            
        }
        public List <Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT W.Id, W.Date, W.Duration, W.WalkerId, W.DogId, O.Name, O.Id AS OId
                FROM Walks W
                LEFT JOIN Dog D ON D.Id = W.DogId
                LEFT JOIN Owner O ON D.OwnerId = O.Id
                WHERE WalkerId = @walkerId
                ORDER BY O.Name
            ";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Walk> walks = new List<Walk>();

                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Dog = new Dog 
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                    Owner = new Owner
                                    {
                                       Id = reader.GetInt32(reader.GetOrdinal("OId")),
                                       Name = reader.GetString(reader.GetOrdinal("Name"))
                                    }
                                }
                                
                            };

                            walks.Add(walk);
                        }

                        return walks;
                    }
                }
            }
        }
    }
}
