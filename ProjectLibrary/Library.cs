using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;


public class Database
{
    private SQLiteConnection connection = null;
    private SQLiteCommand cmd;
    private SQLiteDataReader reader;


    static public string[] FieldHeads = { " ID", "FirstName", "LastName", "Email", "GameType" };
    public int[] FieldLens = { 3, 15, 15, 30, 5 };
    static string[] FieldNames = { " PlayerID", "First", "Last", "Email", "GameID", "Status", "StatusDateTime" };
    string ConnectString = "Data Source = Games.db";
    public Database()
    {
        Connect(ConnectString);
    }

    ~Database()
    {
        Close();
    }
    void Connect(string ConnectString)
    {
        try
        {
            connection = new SQLiteConnection(ConnectString);
            connection.Open();
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message + "\nOccurred in DBConnect");
        }
    }
    void Close()
    {
        if (connection != null) connection.Close();
    }
    private string[][] GetData(string commandString)
    {
        string[][] data = null;
        int index = 0;
        reader = null;

        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = commandString;
            cmd.Connection = connection;

            int records = 0;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                records++;

            reader.Close();
            data = new string[records][];
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data[index] = new string[reader.FieldCount];
                for (int col = 0; col < reader.FieldCount; col++)
                    data[index][col] = reader.GetValue(col).ToString();
                index++;
            }
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message + "\nOccurred in GetData");
        }

        finally
        {
            if (reader != null) reader.Close();
        }

        return data;
    }

    public string[] GetRecord(string key)
    {
        string commandString;
        string[] Record = null;

        commandString = "select * from Players" +
            " where PlayerID = " + "'" + key + "'" + " and Status = 'N'";

        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = commandString;
            cmd.Connection = connection;
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Record = new string[reader.FieldCount];
                int col;
                for (col = 0; col < reader.FieldCount - 1; ++col)
                    Record[col] = reader.GetValue(col).ToString();

            }
            else
            {
                Record = new string[1]
                { "no record found" };
            }
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message + "\nOccurred in GetRecord");
        }

        finally
        {
            if (reader != null) reader.Close();
        }

        return Record;
    }

    public bool RecordExists(string key)
    {
        string commandString;
        bool Exists = false;

        commandString = "select * from players" +
            " where playerID = " + "'" + key + "'";


        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = commandString;
            cmd.Connection = connection;

            reader = cmd.ExecuteReader();
            Exists = reader.Read();
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message + "\nOccurred in RecordExists");
        }

        finally
        {
            if (reader != null) reader.Close();
        }

        return Exists;
    }
    void InsertRecord(string[] Record)
    {
        string str1 = "insert into Players values (";
        for (int index = 0; index < Record.Length; ++index)
        {
            str1 = str1 + "'" + Record[index] + "'";
            if (index < Record.Length - 2)
                str1 += ",";
        }
        string str2 = str1 + ")";


        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = str2;
            cmd.Connection = connection;


            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message + "\nOccurred in InsertRecord");
        }
    }
    public void UpdateRecord(string[] Record)
    {
        string commandString;
        string commandString2 = Record[0];
        int i;

        commandString = "update players set ";
        for (i = 0; i < Record.Length; i++)
        {
            commandString = commandString + FieldNames[i] + " = '" + Record[i] + "'";
            if (i < Record.Length - 1)
                commandString += ",";
        }
        string str3 = commandString + " where PlayerID = '" + commandString2 + "'";
        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = str3;
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void SaveRecord(string[] Record)
    {
        if (RecordExists(Record[0]))
            UpdateRecord(Record);
        else
            InsertRecord(Record);
    }
    public void Purge()
    {
        string commandString = "delete from Players where Status = 'D'";
        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = commandString;
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }
    public static int GetLongestFieldLabel()
    {
        int i;
        int Longest = 0;

        for (i = 0; i < FieldHeads.Length; i++)
            if (FieldHeads[i].Length > Longest)
                Longest = FieldHeads[i].Length;

        return Longest;
    }
    public void SetStatus(string key, char Status)
    {
        string str = "update Players set Status = '" + Status.ToString() + "', " + "StatusDateTime = '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' where PlayerID = '" + key + "'";
        try
        {
            cmd = new SQLiteCommand();
            cmd.CommandText = str;
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void UnDeleteRecord(string key) => SetStatus(key, 'N');
    public void LogicalDeleteRecord(string key) => SetStatus(key, 'D');
    public string[][] GetDeletedPlayers()
     {
        string[][] data = null;
    string commandString;


        commandString = "select PlayerID,First,Last from Players" + " where Status = 'D' " + " order by Last, First";
        data = GetData(commandString);

        return data;
    }
public string[][] GetAllPlayers()
    {
        string[][] data = null;
        string commandString;


        commandString = "Select PlayerID,First,Last,email,gametype from Players,games " +
            "where Players.GameID = Games.GameID and Status = 'N' order by PlayerID";
        data = GetData(commandString);

        return data;
    }
    public string[][] GetGames()
    {
        string[][] data = null;
        string commandString;

        commandString = "select * from Games";

        data = GetData(commandString);

        return data;
    }
    public string[][] GetPlayers()
    {
        string[][] data = null;
        string commandString;

        commandString = "select PlayerID,First,Last from Players" +
            " where Status = 'N' " + " order by Last, First";

        data = GetData(commandString);

        return data;
    }
    public string[][] GetCurrentPlayers()
    {
        string[][] data = null;
        string commandString;

        commandString = "select PlayerID,First,Last from Players" + " order by PlayerID";

        data = GetData(commandString);

        return data;
    }
    
}









