using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using JNCC.Microsite.GCR.Models.Data;

namespace JNCC.Microsite.GCR.Data
{
    public class DatabaseOperations
    {
        private readonly string _databasePath;

        public DatabaseOperations(string databasePath)
        {
            _databasePath = databasePath;
        }

        private DatabaseConnection GetDatabaseconnection()
        {
            return new DatabaseConnection(_databasePath);
        }
        public List<Site> GetSiteList()
        {
            var gcrs = new List<Site>();

            using (DatabaseConnection conn = GetDatabaseconnection())
            {
                var queryString = "SELECT " +
                    "g.GCR_NUMBER, g.GCR_NAME, g.GCR_BLOCK_CODE, b.BLOCK_NAME, g.GRID_REF, g.LATITUDE, g.LONGITUDE, g.FILE_LINK, a.ADMIN_AREA, c.Country, b.INTRODUCTION, v.TITLE, v.AUTHORS " +
                    "FROM (((GCR g " +
                    "INNER JOIN Admin_area a ON g.GCR_NUMBER=a.GCR_NUMBER) " +
                    "INNER JOIN \"Country Lookup\" c ON g.AGENCY=c.AGENCY) " +
                    "INNER JOIN block b ON g.GCR_BLOCK_CODE=b.GCR_BLOCK_CODE) " +
                    "INNER JOIN volume v ON b.VOL_NUMBER=v.VOL_NUMBER";
                OdbcCommand cmd = conn.CreateCommand(queryString);
                using (OdbcDataReader reader = conn.RunCommand(cmd))
                {
                    while (reader.Read())
                    {
                        gcrs.Add(new Site
                        {
                            Code = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            BlockCode = reader.IsDBNull(2) ? null : reader.GetString(2),
                            BlockName = reader.GetString(3),
                            GridReference = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Latitude = reader.IsDBNull(5) ? null : (double?)reader.GetDouble(5),
                            Longitude = reader.IsDBNull(6) ? null : (double?)reader.GetDouble(6),
                            ReportFilePath = reader.IsDBNull(7) ? null : reader.GetString(7),
                            UnitaryAuthority = reader.GetString(8),
                            Country = reader.GetString(9),
                            Publication = new Publication
                            {
                                VolumeFilePath = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Title = reader.GetString(11),
                                Authors = reader.GetString(12)
                            }
                        });
                    }
                }
            }
            
            return gcrs;
        }
        
        public List<Block> GetBlockList()
        {
            var blocks = new List<Block>();

            using (DatabaseConnection conn = GetDatabaseconnection())
            {
                var queryString = "SELECT " +
                    "b.GCR_BLOCK_CODE, b.BLOCK_NAME, b.DEFINITION, v.TITLE, v.AUTHORS, b.INTRODUCTION " +
                    "FROM block b " +
                    "INNER JOIN volume v on v.VOL_NUMBER=b.VOL_NUMBER";
                OdbcCommand cmd = conn.CreateCommand(queryString);
                using (OdbcDataReader reader = conn.RunCommand(cmd))
                {
                    while (reader.Read())
                    {
                        blocks.Add(new Block
                        {
                            Code = reader.GetString(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Publication = new Publication
                            {
                                Title = reader.GetString(3),
                                Authors = reader.IsDBNull(4) ? null : reader.GetString(4), // TODO: remove the null check once the data is cleaned
                                VolumeFilePath = reader.IsDBNull(5) ? null : reader.GetString(5)
                            }
                        });
                    }
                }
            }

            return blocks;
        }
    }
}