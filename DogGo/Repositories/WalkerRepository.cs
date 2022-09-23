using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    
        public class WalkerRepository : IWalkerRepository
        {
            private readonly IConfiguration _config;

            // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
            public WalkerRepository(IConfiguration config)
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

            public List<Walker> GetAllWalkers()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        SELECT W.Id, W.[Name], W.ImageUrl, W.NeighborhoodId, N.Name AS Neighborhood, N.Id AS NId
                        FROM Walker W
                        LEFT JOIN Neighborhood N ON N.Id = W.NeighborhoodId
                    ";

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Walker> walkers = new List<Walker>();
                            while (reader.Read())
                            {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("NId")),
                                    Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                                }
                                };

                                walkers.Add(walker);
                            }

                            return walkers;
                        }
                    }
                }
            }

            public Walker GetWalkerById(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        SELECT W.Id, W.[Name], W.ImageUrl, W.NeighborhoodId, N.Name AS Neighborhood, N.Id AS NId
                        FROM Walker W
                        LEFT JOIN Neighborhood N ON N.Id = W.NeighborhoodId 
                        WHERE W.Id = @id
                    ";

                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Walker walker = new Walker
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    Neighborhood = new Neighborhood
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("NId")),
                                        Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                                    }
                                };

                                return walker;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }

        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };

                            walkers.Add(walker);
                        }

                        return walkers;
                    }
                }
            }
        }
    }
    }

