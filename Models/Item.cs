using System.Collections.Generic;
using MySql.Data.MySqlClient;
using World;
using System;

namespace World.Models
{
    public class Item
    {
        private int _id;
        private string _description;
        public Item(string Description, int Id = 0)
        {
          _id = Id;
          _description = Description;
        }
        public int GetID()
        {
            return _id;
        }
        public string GetDesc()
        {
            return _description;
        }
        public void Save()
        {
          Console.WriteLine("Save()");
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          Console.WriteLine("Description()");
          cmd.CommandText = @"INSERT INTO testTable (description) VALUES (@ItemDescription);";
          MySqlParameter description = new MySqlParameter();
          description.ParameterName = "@ItemDescription";
          description.Value = _description;
          cmd.Parameters.Add(description);
          Console.WriteLine("Command Text" + cmd.CommandText);

          cmd.ExecuteNonQuery();
          _id = (int) cmd.LastInsertedId;
          Console.WriteLine("ID SAVE: " + _id);

          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }

        public static List<Item> GetAll()
        {
            //Opening Database Connection.
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            //Casting and Executing Commands.
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM testTable;";
            //End of CommandText
            //Using Data Reader Object(Represents actual reading of SQL database.)
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            //Contains built in method .Read()
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string itemDescription = rdr.GetString(1);
              Item newItem = new Item(itemDescription, itemId);
              allItems.Add(newItem);
            }
            //Close connection
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }
        public static string DisplayList()
        {
          List<Item> allItems = new List<Item>{};
          string outputString = "";
          allItems = GetAll();
          for(int i = 0; i < allItems.Count; i ++)
          {
            outputString += "(" + allItems[i].GetID() + ", " + allItems[i].GetDesc() + ") ";
          }
          return outputString;
        }
    }

}
