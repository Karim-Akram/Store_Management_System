using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (string.IsNullOrWhiteSpace(clientInfo.name) || string.IsNullOrWhiteSpace(clientInfo.email) ||
                string.IsNullOrWhiteSpace(clientInfo.phone) || string.IsNullOrWhiteSpace(clientInfo.address))
            {
                errorMessage = "All fields are required";
                return;
            }
            //Save data to database

            try
            {
                string connectionString = "Data Source=TECH-KAKRAMM\\SQLEXPRESS;Initial Catalog=StoreDB;User ID=sa;Password=****;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("Connection opened successfully.");
                    string sql = "insert into Clients" +
                        "(name, email, phone, address)" +
                        "values(@name, @email, @phone, @address);";

                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }




                }
            }
            catch
            (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";

            successMessage = "New client added successfully";
            Response.Redirect("/Clients/Index");
        }
    }
}
