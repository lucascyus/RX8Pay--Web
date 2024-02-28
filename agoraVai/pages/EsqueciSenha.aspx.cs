using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agoraVai.pages
{
    public partial class EsqueciSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RedefinirButton_Click(object sender, EventArgs e)
        {

            string username = login.Text;
            string email = eemail.Text;
            string password = novaSenha.Text;

            try
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=loginform;Integrated Security=True");
                con.Open();
                string query = "update tbl_login set password = @password where username = @username and email = @email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MensagemSucesso.Visible = true;
                        //System.Threading.Thread.Sleep(3000);
                        //Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        MensagemErroDados.Visible = true;
                        //System.Threading.Thread.Sleep(10000);
                        //MensagemErroDados.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex);
            }
        }
    }
}