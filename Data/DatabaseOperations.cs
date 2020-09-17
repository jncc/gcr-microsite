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
        public List<Site> GetFullGCRList()
        {
            List<Site> gcrs = new List<Site> { };

            using (DatabaseConnection conn = GetDatabaseconnection())
            {
                string queryString = "SELECT" +
                    "GCR_NUMBER, GCR_NAME, UNITARY_AUTHORITY, GCR_BLOCK_CODE, GRID_REF, LATITUDE, LONGITUDE, FILE_LINK" +
                    "FROM GCR INNER JOIN Admin_area" +
                    "ON GCR.GCR_NAME = Admin_area.GCR_NAME";
                OdbcCommand cmd = conn.CreateCommand(queryString);
                using (OdbcDataReader reader = conn.RunCommand(cmd))
                {
                    while (reader.Read())
                    {
                        gcrs.Add(new Site
                        {
                            Code = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Block = reader.IsDBNull(2) ? null : new Block { Code = reader.GetString(2) },
                            GridReference = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Latitude = reader.IsDBNull(4) ? null : (double?)reader.GetDouble(4),
                            Longitude = reader.IsDBNull(5) ? null : (double?)reader.GetDouble(5),
                            ReportFilePath = reader.IsDBNull(6) ? null : reader.GetString(6)
                        });
                    }

                }
            }

            foreach (Site site in gcrs)
            {
                using (DatabaseConnection conn = GetDatabaseconnection())
                {
                    string queryString = $"SELECT ADMIN_AREA FROM Admin_area WHERE GCR_NUMBER={site.Code}";
                    OdbcCommand cmd = conn.CreateCommand(queryString);
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        string unitaryAuthority = reader.IsDBNull(0) ? null : reader.GetString(0);
                        Console.Out.WriteLine($"Site: {site.Code}, unitary authority: {unitaryAuthority}");
                        site.UnitaryAuthority = unitaryAuthority;
                    }
                }
            }

            return gcrs;
        }
    }
}