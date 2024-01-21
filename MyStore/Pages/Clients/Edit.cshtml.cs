using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                
                const string connectionString = "Data Source=TECH-KAKRAMM\\SQLEXPRESS;Initial Catalog=StoreDB;User ID=sa;Password=****;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("Connection opened successfully.");

                    
                    string sql = "SELECT * FROM Clients WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            command.Parameters.AddWithValue("@id", id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    clientInfo.id = reader.GetInt32(0).ToString();
                                    clientInfo.name = reader.GetString(1);
                                    clientInfo.email = reader.GetString(2);
                                    clientInfo.phone = reader.GetString(3);
                                    clientInfo.address = reader.GetString(4);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
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

            try
            {
               
                const string connectionString = "Data Source=TECH-KAKRAMM\\SQLEXPRESS;Initial Catalog=StoreDB;User ID=sa;Password=****;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("Connection opened successfully.");

                    
                    string sql = "UPDATE Clients " +
                                 "SET name=@name, email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                Debug.WriteLine($"Name: {clientInfo.name}");
                Debug.WriteLine($"Email: {clientInfo.email}");
                Debug.WriteLine($"Phone: {clientInfo.phone}");
                Debug.WriteLine($"Address: {clientInfo.address}");
                Debug.WriteLine($"Client ID: {clientInfo.id}");
            }

            successMessage = "Client updated successfully";
            
            Response.Redirect("/Clients/Index");
        }
    }
}
