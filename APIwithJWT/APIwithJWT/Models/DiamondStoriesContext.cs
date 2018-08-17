using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using APIwithJWT.Controllers;
using Newtonsoft.Json.Linq;

namespace APIwithJWT.Models
{
    public class DiamondStoriesContext
    {
        public string ConnectionString { get; set; }

        public DiamondStoriesContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Launcher_Mods> GetAllMods()
        {
            List<Launcher_Mods> list = new List<Launcher_Mods>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from launcher_mods", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Launcher_Mods()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Modname = reader["modname"].ToString(),
                            Modurl = reader["modurl"].ToString(),
                            Installpath = reader["installpath"].ToString(),
                            Md5sum = reader["md5sum"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return list;
        }
        public List<Accounts> GetAllAccounts()
        {
            List<Accounts> list = new List<Accounts>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from accounts", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Accounts()
                        {
                            Id = Convert.ToInt32(reader["uid"]),
                            Login = reader["login"].ToString(),
                            Password = reader["password"].ToString(),
                            Email = reader["email"].ToString(),
                            Name = reader["name"].ToString(),
                            Surname = reader["surname"].ToString(),
                            Age = Convert.ToInt32(reader["age"]),
                            Sessionid = reader["sessionid"].ToString(),
                            Sessionip = reader["sessionip"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return list;
        }

        public List<Accounts> GetAccount(string Login)
        {
            List<Accounts> list = new List<Accounts>();

            using (MySqlConnection conn = GetConnection())
            {
                if (Login.IndexOf("'") == -1 && Login.IndexOf("-") == -1)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(String.Format("select * from accounts where login = '{0}'", Login), conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Accounts()
                            {
                                Id = Convert.ToInt32(reader["uid"]),
                                Login = reader["login"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString(),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Age = Convert.ToInt32(reader["age"]),
                                Sessionid = reader["sessionid"].ToString(),
                                Sessionip = reader["sessionip"].ToString()
                            });
                        }
                    }
                    conn.Close();
                }
            }
            return list;
        }
        public int GetUserUID(string Login)
        {
            List<Accounts> list = new List<Accounts>();
            int UserID = 0;
            using (MySqlConnection conn = GetConnection())
            {
                if (Login.IndexOf("'") == -1 && Login.IndexOf("-") == -1)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(String.Format("select uid from accounts where login = '{0}'", Login), conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserID = Convert.ToInt32(reader["uid"]);
                        }
                    }
                    conn.Close();
                }
            }
            return UserID;
        }

        public void SetSession(int id, string Sessionid, string Sessionip)
        {
            using (MySqlConnection conn = GetConnection())
            {
                    System.Diagnostics.Debug.WriteLine(String.Format("UPDATE accounts SET sessionid = '{0}', sessionip = '{1}' WHERE uid = {2};", Sessionid, Sessionip, id));
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE accounts SET sessionid = '{0}', sessionip = '{1}' WHERE uid = {2};", Sessionid, Sessionip, id), conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                
            }
        }

        public List<Accounts> Login(string Login, string Password)
        {
            List<Accounts> list = new List<Accounts>();

            using (MySqlConnection conn = GetConnection())
            {
                if (Login.IndexOf("'") == -1 && Login.IndexOf("-") == -1 && Password.IndexOf("'") == -1 && Password.IndexOf("-") == -1)
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(String.Format("select * from accounts where login = '{0}' AND password = '{1}'", Login, Password), conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Accounts()
                            {
                                Id = Convert.ToInt32(reader["uid"]),
                                Login = reader["login"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString(),
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Age = Convert.ToInt32(reader["age"]),
                                Sessionid = reader["sessionid"].ToString(),
                                Sessionip = reader["sessionip"].ToString()
                            });
                        }
                    }
                    conn.Close();
                }
            }
            return list;
        }
    }


}
