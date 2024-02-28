using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace agoraVai.pages
{
    public partial class CriarConta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CadastrarButton_Click(object sender, EventArgs e)
        {
            string username = login.Text;
            string email = eemail.Text;
            string password = senha.Text;
            string password2 = confirmarSenha.Text;

            if (password != password2)
            {
                MensagemErroDados.Visible = true;
            }
            else
            {
                try
                {
                    SqlConnection con = new SqlConnection("Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=loginform;Integrated Security=True");
                    con.Open();
                    string query = "select count(*) from tbl_login where username = @username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MensagemErroDados.Visible = true;
                            return;
                        }
                    }
                    string query1 = "select count(*) from tbl_login where email = @email";
                    using (SqlCommand cmd = new SqlCommand(query1, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MensagemErroDados.Visible = true;
                            return;
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand("insert into tbl_login (username, password, email) values (@username, @password, @email)", con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@email", email);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        con.Close();
                        if (rowsAffected > 0)
                        {
                            MensagemSucesso.Visible = true;

                        }
                        else
                        {
                            Console.WriteLine("Erro");
                        }
                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }
    }
}