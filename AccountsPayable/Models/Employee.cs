using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountsPayable.Models
{
    public class Employee
    {
        [Key]
        public Int32 employee_id { get; set; }

        public String employee_first_name { get; set; }

        public String employee_last_name { get; set; }

        public object GetConString { get; private set; }

        public int SaveDetails()
        {
            SqlConnection con = new SqlConnection(GetConString.ToString());

            string query = "INSERT INTO employee(employee_first_name, employee_last_name) values (?, ?, ?)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue()

            con.Open();

            int i = cmd.ExecuteNonQuery();

            con.Close();

            return i;
        }
    }
}