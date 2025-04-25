using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebServer
{
    internal class DataBaseGetter
    {

        public static Dictionary<int, string> Players = new Dictionary<int, string>();
        public static readonly string connectionString;
        // make the connection with database and we could make intersection with database between server and client.
        static DataBaseGetter()
        {
            var builder = new ConfigurationBuilder();

            builder.AddUserSecrets<WebServer>();
            IConfigurationRoot Configuration = builder.Build();
            var SelectedSecrets = Configuration.GetSection("GameSecrets");

            connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = SelectedSecrets["ServerURL"],
                InitialCatalog = SelectedSecrets["DBName"],
                UserID = SelectedSecrets["UserName"],
                Password = SelectedSecrets["DBPassword"],
                Encrypt = false
            }.ConnectionString;
        }


        /// <summary>
        /// this used to highscores URL and it will return the highest scores in the database
        /// </summary>
        /// <returns></returns>
        public static string HighScore()
        {
            try
            {
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                using SqlCommand command = new SqlCommand($@"SELECT Players.PlayerID, players.Name,PlayersMaxMass.PlayersMaxMass FROM Players
                                                            INNER JOIN PlayersMaxMass ON PlayersMaxMass.PlayerID = Players.PlayerID", con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    result += $@"{reader.GetInt32(0)}
                                 {reader.GetString(1)}
                                 {reader.GetDouble(2)}
";
                }
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// this method is used to get particular person's scores and return this string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ParticularPlayerScore(string name)
        {
            try
            {
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                using SqlCommand command = new SqlCommand($@"SELECT Players.PlayerID, players.Name,PlayersMaxMass.PlayersMaxMass FROM Players
                    INNER JOIN PlayersMaxMass ON PlayersMaxMass.PlayerID = Players.PlayerID
                    WHERE Players.Name LIKE '%{name}%'", con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    result += $@"{reader.GetInt32(0)}
                                 {reader.GetString(1)}
                                 {reader.GetDouble(2)}
";
                }
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// this method create a table for into database
        /// </summary>
        /// <returns></returns>
        public static string createTable()
        {
            try
            {
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                using SqlCommand command = new SqlCommand($@"CREATE TABLE players_GamesIDAndName (
                                                            PlayerID int,
                                                            GameID int,
                                                            Name varchar(50)
                                                            );
                                                            INSERT INTO players_GamesIDAndName (PlayerID, GameID, Name)
                                                            VALUES (10,15,'JJ');", con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    result += $@"{reader.GetInt32(0)}
                                 {reader.GetString(1)}
                                 {reader.GetDouble(2)}
";
                }
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// this one return a fancy page and this one is different with other webpage
        /// </summary>
        /// <returns></returns>
        public static string fancy()
        {
            try
            {
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                using SqlCommand command = new SqlCommand($@"SELECT PlayerID, Name FROM Players", con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    Players.Add(reader.GetInt32(0), reader.GetString(1));
                    result += $@"{reader.GetInt32(0)}
                                 {reader.GetString(1)}

";  
                }
                return result;
            }
            catch (SqlException exception)
            {
                return "error";
            }
        }
        /// <summary>
        /// this one insert a bunch of data into database
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string insertion(List<string> content)
        {
            try
            {
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();
                string sendText = $@"";
                // send name to database and get correspinding playerID
                DataBaseGetter.insertionPlayerID(content[2]);
                string PlayerID = DataBaseGetter.selectPlayerID(content[2]);
                // insert rest of data into database.
                for (int i =3; i < content.Count; i++)
                {
                    // this one parse collection of data, and we add the playerID and mass into query for our database.
                    if (i == 3)
                    {
                        float highMass = float.Parse(content[3]);
                        sendText += $@"INSERT INTO PlayersMaxMass
                                      VALUES ({PlayerID}, {highMass}) {"\r\n"}";
                        // this one parse message and assign into query for sending to database
                    }else if (i == 4)
                    {
                        int rank = int.Parse(content[4]);
                        sendText += $@"INSERT INTO PlayersRank
                                      VALUES ({PlayerID}, {rank}) {"\r\n"}";
                        // this one parse date and assign into query
                    }else if (i == 5)
                    {
                        long starttime = long.Parse(content[5]);
                        DateTime startTime = new DateTime(DateTime.UnixEpoch.Ticks + starttime / 10);
                        string sqlFormattedStartDate = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        sendText += $@"INSERT INTO PlayersJoinedTime
                                    VALUES ({PlayerID}, '{sqlFormattedStartDate}') {"\r\n"}";
                    }
                    else
                    {
                        // this one get endtime and parse it into query
                        long endtime = long.Parse(content[6]);
                        DateTime endTime = new DateTime(DateTime.UnixEpoch.Ticks + endtime / 10);
                        string sqlFormattedEndDate = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        sendText += $@"INSERT INTO PlayersEndTime
                                      VALUES ({PlayerID}, '{sqlFormattedEndDate}')";
                    }
                }
                // sending assigned message into database
                using SqlCommand command = new SqlCommand(sendText, con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    Players.Add(reader.GetInt32(0), reader.GetString(1));
                    result += $@"{reader.GetInt32(0)}
                                 {reader.GetString(1)}
";
                }
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// this method service for insertion
        /// we get playerID in this method for insertion
        /// </summary>
        /// <returns></returns>
        public static string insertionPlayerID(string name)
        {
            try
            {
                // use SELECT query and send data to database
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                using SqlCommand command = new SqlCommand($@"INSERT INTO Players (Name)
                                                            VALUES ('{name}')", con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    Players.Add(reader.GetInt32(0), reader.GetString(1));
                    result += $@"{reader.GetInt32(0)}
                                 {reader.GetString(1)}
";
                }
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// this one select playerID from database
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string selectPlayerID(string name)
        {
            try
            {
                // use SELECT query send data into database 
                using SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                using SqlCommand command = new SqlCommand($@"SELECT PlayerID FROM Players
                                                            WHERE Players.Name = '{name}'", con);
                using SqlDataReader reader = command.ExecuteReader();
                string result = "";
                while (reader.Read())
                {
                    result += $@"{reader.GetInt32(0)}
";
                }
                return result;
            }
            catch (SqlException exception)
            {
                throw exception;
            }
        }

    }
}
