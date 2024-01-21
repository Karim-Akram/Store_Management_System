using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> ListClients=new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=TECH-KAKRAMM\\SQLEXPRESS;Initial Catalog=StoreDB;User ID=sa;Password=****;";
                using(SqlConnection connection=new SqlConnection(connectionString))
                {
                    connection.Open();
                    Debug.WriteLine("Connection opened successfully.");
                    string sql = "SELECT * FROM Clients";
                    using (SqlCommand command=new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader=command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3); 
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at= reader.GetDateTime(5).ToString();

                                ListClients.Add(clientInfo);
                            }
                        }

                    }

                }
            }catch
            (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
        }
    }

    public class ClientInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }

}
