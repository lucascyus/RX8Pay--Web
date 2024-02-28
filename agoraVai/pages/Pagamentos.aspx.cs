using comoPode;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Button = System.Web.UI.WebControls.Button;
using Document = iTextSharp.text.Document;

namespace agoraVai.pages
{
    public partial class Pagamentos : Page
    {
        string perfil = UserContext.Username;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherNomeUsuario(perfil); //obter @ do usuario

                DateTime currentDate = DateTime.Now;

                // Se a data atual for até o dia 5, atualiza o StatusPagamento para Pendente
                if (currentDate.Day <= 5)
                {
                    AtualizarStatusPagamento("Pendente");
                }
                else
                {
                    AtualizarStatusPagamento("Pago");
                }

                PreencherTabela();

                if (!string.IsNullOrEmpty(Request.QueryString["excluirId"]))
                {
                    string idParaExcluir = Request.QueryString["excluirId"];
                    ExcluirFuncionarioDoBanco(idParaExcluir);
                    Response.Redirect("Pagamentos.aspx");
                } //Excluir Funcionario
            }
        }

        private void ExcluirFuncionarioDoBanco(string funcionarioId)
        {
            // Define a string de conexão com o banco de dados SQL Server.
            string connectionString = "Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=SALARY_SYNC;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define a consulta SQL para excluir o funcionário com base no Id.
                string query = "DELETE FROM tbl_funcionario WHERE Id = @FuncionarioId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FuncionarioId", funcionarioId);

                    // Executa a consulta para excluir o funcionário.
                    command.ExecuteNonQuery();
                }
            }
        }

        private void AtualizarStatusPagamento(string novoStatus)
        {
            // Define a string de conexão com o banco de dados SQL Server.
            string connectionString = "Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=SALARY_SYNC;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define a consulta SQL para atualizar o StatusPagamento com base na DataAdmissao.
                string query = "UPDATE tbl_funcionario SET StatusPagamento = @statuspagamento WHERE DataAdmissao <= @DataAdmissao";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@statuspagamento", novoStatus);

                    // Define a data do primeiro dia do mês atual
                    DateTime primeiroDiaDoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    command.Parameters.AddWithValue("@DataAdmissao", primeiroDiaDoMes);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void EfetuarPagamentos()
        {
            // Define a string de conexão com o banco de dados SQL Server.
            string connectionString = "Data Source=DESKTOP-7092F7U;Initial Catalog=SALARY_SYNC;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtém o valor da cota mensal da empresa do banco de dados.
                decimal cotaMensal = ObterCotaMensal(connection);

                // Obtém a lista de funcionários elegíveis para pagamento (por exemplo, aqueles com StatusPagamento = "Pendente").
                List<Funcionario> funcionariosElegiveis = ObterFuncionariosElegiveis(connection);

                // Calcula o valor total dos pagamentos.
                decimal totalPagamentos = 0;

                foreach (var funcionario in funcionariosElegiveis)
                {
                    // Subtrai o salário do funcionário do valor da cota mensal.
                    cotaMensal -= funcionario.Salario;

                    // Atualiza o StatusPagamento do funcionário para "Pago".
                    AtualizarStatusPagamentoFuncionario(connection, funcionario.Id, "Pago");

                    // Registra o pagamento (você pode criar um registro de pagamento no banco de dados).
                    RegistrarPagamentoNoBanco(connection, funcionario.Id, funcionario.Salario);

                    // Adiciona o valor do pagamento ao total.
                    totalPagamentos += funcionario.Salario;
                }

                // Atualiza a cota mensal da empresa no banco de dados com o novo valor após os pagamentos.
                AtualizarCotaMensal(connection, cotaMensal);

                // Aqui, você pode usar o valor totalPagamentos como necessário.
            }
        } //Em desenvolvimento

        private decimal ObterCotaMensal(SqlConnection connection) //Utilizando uma lógica de definir a cota pelo valor armazenado em um banco de dados para simular uma conta bancária
        {
            // Lógica para obter a cota mensal do banco de dados      
            string query = "SELECT valor from cotamensal";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object resultado = command.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    decimal cotaMensal = Convert.ToDecimal(resultado);
                    return cotaMensal;
                }
            }

            return 0;
        }

        private List<Funcionario> ObterFuncionariosElegiveis(SqlConnection connection)
        {
            List<Funcionario> funcionariosElegiveis = new List<Funcionario>();

            // Defina a consulta SQL para selecionar funcionários elegíveis com StatusPagamento "Pendente".
            string query = "SELECT Id, Salario FROM tbl_funcionario WHERE StatusPagamento = 'Pendente'";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Funcionario funcionario = new Funcionario
                        {
                            Id = reader["Id"].ToString(), // Converte o ID para string
                            Salario = Convert.ToDecimal(reader["Salario"])
                        };
                        funcionariosElegiveis.Add(funcionario);
                    }
                }
            }

            return funcionariosElegiveis;
        }

        private void RegistrarPagamentoNoBanco(SqlConnection connection, string Id, decimal salario) 
        {
            
            string query = "INSERT INTO tabela_pagamentos (FuncionarioId, ValorPagamento, DataPagamento) VALUES (@FuncionarioId, @ValorPagamento, @DataPagamento)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@salario", salario);
                command.Parameters.AddWithValue("@DataPagamento", DateTime.Now); 

                command.ExecuteNonQuery();
            }
        }

        private void AtualizarStatusPagamentoFuncionario(SqlConnection connection, string Id, string novoStatus) // Atualizar o status do pagamento do funcionário
        {
            
            string query = "UPDATE tbl_funcionario SET StatusPagamento = @novoStatus WHERE Id = @FId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@novoStatus", novoStatus);
                command.ExecuteNonQuery();
            }
        }

        private void AtualizarCotaMensal(SqlConnection connection, decimal novoValorCota) // Manter a cota mensal após virada mensal
        {
            string query = "UPDATE cotamensal SET valor = @NovoValorCota";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NovoValorCota", novoValorCota);

                command.ExecuteNonQuery();
            }
        }

        private void PreencherTabela()
        {

            string connectionString = "Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=SALARY_SYNC;Integrated Security=True"; // Substitua pela sua string de conexão ao SQL Server
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT DataAdmissao, Nome, CPF, Id, Cargo, Salario, StatusPagamento FROM tbl_funcionario";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TableRow row = new TableRow();

                        // Crie células para os dados do funcionário (como antes)
                        TableCell cellDataAdmissao = new TableCell { Text = Convert.ToDateTime(reader["DataAdmissao"]).ToString("dd/MM/yyyy") };
                        TableCell cellNome = new TableCell { Text = reader["Nome"].ToString() };
                        TableCell cellCpf = new TableCell { Text = CensurarCPF(reader["CPF"].ToString()) };
                        TableCell cellId = new TableCell { Text = reader["Id"].ToString() };
                        TableCell cellCargo = new TableCell { Text = reader["Cargo"].ToString() };
                        TableCell cellSalarioTotal = new TableCell { Text = reader["Salario"].ToString() };
                        TableCell cellStatusPagamento = new TableCell { Text = reader["StatusPagamento"].ToString() };

                        row.Cells.Add(cellDataAdmissao);
                        row.Cells.Add(cellNome);
                        row.Cells.Add(cellCpf);
                        row.Cells.Add(cellId);
                        row.Cells.Add(cellCargo);
                        row.Cells.Add(cellSalarioTotal);
                        row.Cells.Add(cellStatusPagamento);

                        HyperLink lnkExcluir = new HyperLink();
                        lnkExcluir.ID = $"lnkExcluir_{reader["Id"]}";
                        lnkExcluir.Text = "Excluir";
                        lnkExcluir.CssClass = "lnkExcluir";
                        lnkExcluir.NavigateUrl = $"javascript:confirmarExclusao('{reader["Id"]}');";

                        TableCell cellExcluir = new TableCell();
                        cellExcluir.Controls.Add(lnkExcluir);
                        row.Cells.Add(cellExcluir);

                        tabelaPagamentos.Rows.Add(row);

                    }
                }
            }
        }

        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            LinkButton lnkExcluir = (LinkButton)sender;
            string funcionarioId = lnkExcluir.ID.Split('_')[1]; // Obtém o Id do funcionário da ID do LinkButton

            // Exibe um popup de confirmação antes de excluir
            string script = $"if (confirm('Tem certeza que deseja excluir o funcionário?')) {{ window.location.href = 'Pagamentos.aspx?excluirId={funcionarioId}'; }}";
            ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmacaoExclusao", script, true);
        }

        protected void GerarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                Relatorio.GerarRelatorio();
                MessageBox.Show("Relatório gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //Logger.Log($"Erro ao gerar relatório: {ex.Message}");

                // Exibir uma mensagem de erro geral
                MessageBox.Show($"Ocorreu um erro ao gerar o relatório. Detalhes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Exibir informações adicionais (opcional)
                Console.WriteLine($"Tipo de exceção: {ex.GetType().FullName}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }

            Response.Redirect("Pagamentos.aspx");
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        private Dictionary<string, decimal> ConsultarGastosPorCargo(SqlConnection connection)
        {
            // Consulte o banco de dados para obter os gastos por cargo e retorne um dicionário
            // onde a chave é o cargo e o valor é a quantia gasta nesse cargo
            Dictionary<string, decimal> cargoGastos = new Dictionary<string, decimal>();

            // Consulta SQL para obter gastos por cargo
            string query = "SELECT Cargo, SUM(Salario) AS Gasto FROM tbl_funcionario GROUP BY Cargo";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string cargo = reader["Cargo"].ToString();
                    decimal gasto = Convert.ToDecimal(reader["Gasto"]);
                    cargoGastos[cargo] = gasto;
                }
            }

            return cargoGastos;
        } //em desenvolvimento

        private decimal ConsultarCotaMensal(SqlConnection connection) //obter cota mensal
        {
            string cotaQuery = "SELECT Valor FROM cotamensal";
            using (SqlCommand cotaCommand = new SqlCommand(cotaQuery, connection))
            {
                object result = cotaCommand.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                return 0M; 
            }
        }

        private decimal CalcularBeneficios(decimal salario)
        {
            // Implemente a lógica para calcular os benefícios aqui
            decimal dsr = salario * 0.05M;
            decimal valeTransporte = salario * 0.06M;
            decimal valeRefeicao = salario * 0.08M;
            decimal seguroDeVida = 150M;
            return dsr + valeTransporte + valeRefeicao + seguroDeVida;
        }

        private decimal CalcularGastosComuns(decimal salario)
        {
            // Implemente a lógica para calcular os gastos/descontos comuns da empresa aqui
            // Exemplo: Impostos, FGTS, etc.
            decimal percentualFGTS = salario * 0.08m;
            return percentualFGTS;
        }

        protected void ExportarPDF_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-E16QRJ4\\SQLEXPRESS01;Initial Catalog=SALARY_SYNC;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Nome, Cargo, Salario FROM tbl_funcionario";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string pdfFolderPath = Server.MapPath("~/PDFs"); // Caminho para PDF

                    if (!Directory.Exists(pdfFolderPath))
                    {
                        Directory.CreateDirectory(pdfFolderPath);
                    }

                    List<string> pdfFiles = new List<string>(); // Lista para armazenar caminhos dos PDF

                    while (reader.Read())
                    {
                        string nomeFuncionario = reader["Nome"].ToString();
                        string cargoFuncionario = reader["Cargo"].ToString();
                        decimal salario = Convert.ToDecimal(reader["Salario"]);

                        Document document = new Document();
                        string pdfFilePath = Server.MapPath($"~/PDFs/{nomeFuncionario}_Demonstrativo.pdf");
                        pdfFiles.Add(pdfFilePath); // Adicione o caminho do PDF
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFilePath, FileMode.Create));
                        document.Open();

                        // Cabeçalho
                        PdfPCell headerCell = new PdfPCell(new Phrase("Demonstrativo de Pagamento", new Font(Font.FontFamily.HELVETICA, 16f, Font.BOLD)));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPTable headerTable = new PdfPTable(1);
                        headerTable.DefaultCell.Border = 0;
                        headerTable.AddCell(headerCell);
                        document.Add(headerTable);

                        // Informações do funcionário
                        Paragraph nomeParagrafo = new Paragraph();
                        nomeParagrafo.Alignment = Element.ALIGN_CENTER;

                        nomeParagrafo.Add(new Chunk(nomeFuncionario, new Font(Font.FontFamily.HELVETICA, 12f)));
                        document.Add(nomeParagrafo);

                        Paragraph cargoParagrafo = new Paragraph();
                        cargoParagrafo.Alignment = Element.ALIGN_CENTER;

                        cargoParagrafo.Add(new Chunk(cargoFuncionario, new Font(Font.FontFamily.HELVETICA, 12f)));
                        document.Add(cargoParagrafo);

                        document.Add(new Paragraph(" "));

                        // Cálculos de benefícios (valores fictícios)
                        decimal dsr = salario * 0.05M;
                        decimal valeTransporte = salario * 0.06M;
                        decimal valeRefeicao = salario * 0.08M;
                        decimal seguroDeVida = 150M;

                        decimal inss = 0M;
                        if (salario <= 1302.00M)
                        {
                            inss = salario * 0.075M;
                        }
                        else if (salario <= 2571.29M)
                        {
                            inss = salario * 0.09M;
                        }
                        else if (salario <= 3856.94M)
                        {
                            inss = salario * 0.12M;
                        }
                        else if (salario <= 7507.49M)
                        {
                            inss = salario * 0.14M;
                        }
                        // Lida com salários acima de 7507.49, se necessário.

                        decimal percentualFGTS = salario * 0.08M;
                        decimal baseFGTS = salario;
                        decimal baseCalculoIRRF = salario - inss;

                        // Tabela de benefícios
                        PdfPTable beneficiosTable = new PdfPTable(2);
                        beneficiosTable.AddCell(new PdfPCell(new Phrase("Descrição", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
                        beneficiosTable.AddCell(new PdfPCell(new Phrase("Valor (R$)", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
                        beneficiosTable.AddCell("DSR");
                        beneficiosTable.AddCell(dsr.ToString("F2"));
                        beneficiosTable.AddCell("Vale Transporte");
                        beneficiosTable.AddCell(valeTransporte.ToString("F2"));
                        beneficiosTable.AddCell("Vale Refeição");
                        beneficiosTable.AddCell(valeRefeicao.ToString("F2"));
                        beneficiosTable.AddCell("Seguro de Vida");
                        beneficiosTable.AddCell(seguroDeVida.ToString("F2"));
                        beneficiosTable.AddCell("INSS Folha");
                        beneficiosTable.AddCell($"-{inss.ToString("F2")}");

                        document.Add(beneficiosTable);

                        // Adicionar os dados
                        PdfPTable dadosTable = new PdfPTable(2);
                        dadosTable.AddCell("Salário Base");
                        dadosTable.AddCell(salario.ToString("F2"));
                        dadosTable.AddCell("Salário Contribuinte INSS");
                        dadosTable.AddCell(inss.ToString("F2"));
                        dadosTable.AddCell("Base Cálculo FGTS");
                        dadosTable.AddCell(baseFGTS.ToString("F2"));
                        dadosTable.AddCell("FGTS do Mês");
                        dadosTable.AddCell(percentualFGTS.ToString("F2"));
                        dadosTable.AddCell("Base Cálculo IRRF");
                        dadosTable.AddCell(baseCalculoIRRF.ToString("F2"));

                        document.Add(dadosTable);

                        document.Add(new Paragraph(" "));

                        // Valor total dos benefícios
                        decimal totalBeneficios = dsr + valeTransporte + valeRefeicao + seguroDeVida;

                        // Adicionar o valor total de benefícios no PDF
                        Paragraph totalBeneficiosParagrafo = new Paragraph();
                        totalBeneficiosParagrafo.Alignment = Element.ALIGN_CENTER;
                        totalBeneficiosParagrafo.Add(new Chunk("Total de Benefícios: ", new Font(Font.FontFamily.HELVETICA, 12f)));
                        totalBeneficiosParagrafo.Add(new Chunk(totalBeneficios.ToString("F2"), new Font(Font.FontFamily.HELVETICA, 12f)));
                        document.Add(totalBeneficiosParagrafo);

                        // Salário Líquido
                        decimal salarioLiquido = salario - inss;
                        Paragraph liquidoSalario = new Paragraph();
                        liquidoSalario.Alignment = Element.ALIGN_CENTER;
                        liquidoSalario.Add(new Chunk("Salário Líquido: ", new Font(Font.FontFamily.HELVETICA, 12f)));
                        liquidoSalario.Add(new Chunk(salarioLiquido.ToString("F2"), new Font(Font.FontFamily.HELVETICA, 12f)));
                        document.Add(liquidoSalario);

                        document.Add(new Paragraph(" "));
                        document.Add(new Paragraph(" "));

                        // Espaço para preencher com caneta
                        PdfPTable assinaturaTable = new PdfPTable(1);
                        PdfPCell cell = new PdfPCell(new Phrase("Assinatura do Funcionário: _______________________________"));
                        cell.Border = 0;
                        assinaturaTable.AddCell(cell);

                        document.Add(assinaturaTable);

                        document.Close();
                    }

                    string zipFolderPath = Server.MapPath("~/PDFs");
                    string zipFileName = "funcionarios.zip";
                    string zipFilePath = Path.Combine(zipFolderPath, zipFileName);

                    int count = 1;
                    while (File.Exists(zipFilePath))
                    {
                        zipFileName = $"funcionarios_{count}.zip";
                        zipFilePath = Path.Combine(zipFolderPath, zipFileName);
                        count++;
                    }

                    using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        foreach (var pdfFile in pdfFiles)
                        {
                            archive.CreateEntryFromFile(pdfFile, Path.GetFileName(pdfFile));
                        }
                    }

                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment;filename=funcionarios.zip");
                    Response.TransmitFile(zipFilePath);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        private string CensurarCPF(string cpf) //logica para censurar o cpf na interface
        {
            if (cpf.Length == 11)
            {
                return $"***.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-**";
            }
            return cpf;
        }
        public void PreencherNomeUsuario(string perfil) //lógica para obter o @ do usuario
        {
            if (!string.IsNullOrEmpty(perfil))
            {
                userName.InnerText = perfil; 
            }
            else
            {               
                userName.InnerText = "";
            }
        }

    }
}
