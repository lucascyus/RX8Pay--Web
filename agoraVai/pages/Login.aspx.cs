using System;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace agoraVai.pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MensagemErro.Visible = false;
        }

        protected void EntrarButton_Click(object sender, EventArgs e)
        {
            string username = login.Text;
            string password = senha.Text;

            if (VerificarCredenciais(username, password))
            {
                UserContext.SetUser(username, username);
                Response.Redirect("Pagamentos.aspx");
            }
            else
            {
                // Se as credenciais estiverem incorretas, você pode exibir uma mensagem de erro para o usuário.
                // Por exemplo:
                MensagemErro.Visible = true;
            }
        }

        private bool VerificarCredenciais(string username, string password)
        {
            string connectionString = "Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=loginform;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM tbl_login WHERE username = @username AND password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int result = (int)command.ExecuteScalar();

                    if (result > 0)
                    {
                        return true; // Credenciais corretas
                    }
                }
            }
            return false; // Credenciais incorretas
        }
    }
}
