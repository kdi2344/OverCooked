using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System;

public class Userinfo
{
    public string name { get; private set; }
    //public string password { get; private set; }

    public Userinfo(string name, string password)
    {
        this.name = name;
        //this.password = password;
    }
}

public class DBManager : MonoBehaviour
{
    public static DBManager instance = null;

    //config json path
    public string DBPath = string.Empty;
    public string ServerIP = string.Empty;

    //mySQL 변수들
    MySqlConnection connection;
    MySqlDataReader reader;

    public Userinfo info;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        try
        {
            DBPath = Application.dataPath + "/Database";
            string ServerInfo = ServerSet(DBPath);
            if (ServerInfo == null)
            {
                Debug.Log("SQL Server Null");
                return;
            }
            connection = new MySqlConnection(ServerInfo);
            connection.Open();
            Debug.Log("SQL Server Open");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public string ServerSet(string path)
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string jsonString = File.ReadAllText(path + "/config.json");
        JsonData item = JsonMapper.ToObject(jsonString);
        string serverInfo = string.Format("Server= {0}; Database={1}; Uid={2}; Pwd={3}; Port={4};CharSet=utf8;", item[0]["IP"].ToString(), item[0]["TableName"].ToString(), item[0]["ID"].ToString(), item[0]["PW"].ToString(), item[0]["PORT"].ToString());

        string jsonStringServer = File.ReadAllText(path + "/Server.json");
        JsonData itemServer = JsonMapper.ToObject(jsonStringServer);
        ServerIP = itemServer[0]["Server_IP"].ToString();
        Debug.Log(ServerIP);

        return serverInfo;
    }

    private bool ConnectionCheck(MySqlConnection connection)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }

    public bool Login(string id, string PW)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand =
                string.Format(@"SELECT User_Name,U_Password FROM user_info
WHERE User_Name='{0}' AND U_Password='{1}';", id, PW);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) //읽어온 데이터에 행이 한개 이상이면
            {
                while (reader.Read())
                {
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_Name"].ToString();
                    string pass = (reader.IsDBNull(0)) ? string.Empty : (string)reader["U_Password"].ToString();

                    if (!name.Equals(string.Empty) || !pass.Equals(string.Empty))
                    {
                        //로그인 성공
                        info = new Userinfo(name, pass);
                        if (!reader.IsClosed) reader.Close();
                        return true;
                    }
                    else
                    {
                        //로그인 실패
                        break;
                    }
                } //while문 끝
            }//if문 끝
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }
}
