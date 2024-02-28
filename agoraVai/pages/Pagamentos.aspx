<!-- Código HTML no arquivo Pagamentos.aspx -->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pagamentos.aspx.cs" Inherits="agoraVai.pages.Pagamentos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pagamentos - RX8Pay</title>
    <link rel="stylesheet" type="text/css" href="~/css/stylesPag.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap" rel="stylesheet" />
    
    <script src="../js/script.js"></script>
    <script src="../js/getUser.js"></script>
    <script src="../js/ee.js"></script>

    <script type="text/javascript">
        function confirmarExclusao(funcionarioId) {
            var confirmacao = confirm('Tem certeza que deseja excluir o funcionário?');

            if (confirmacao) {
                window.location.href = 'Pagamentos.aspx?excluirId=' + funcionarioId;
            }
        }
    </script>



</head>
<body>
    <form id="form1" runat="server">
            <div class="navbar">
                <ul>
                    <li><a href="Pagamentos.aspx" id="Pagamentos"><strong>Pagamentos</strong></a></li>
                    <li><a href="CadastroFuncionario.aspx">Cadastro de Funcionários</a></li>
                </ul>
                <div class="user-icon" id="userIcon" onclick="toggleUserPanel()">
                    <img src="../img/user_icon.png" alt="Ícone do Usuário" />
                </div>
            </div>
            <div id="userPanel" class="user-panel">
                <div class="usuario">@<span id="userName" runat="server"></span></div>                
                <asp:Button ID="btnLogout" runat="server" Text="Sair" OnClick="Logout_Click" CssClass="botao-sair" />
            </div>   
        <div class="funcionarios-panel">
            <h1 id="pag">Pagamentos</h1>
            <asp:Button ID="btnExportarTodosPDF" runat="server" Text="Exportar PDFs" OnClick="ExportarPDF_Click" CssClass="botao-pdf"/>
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="GerarRelatorio_Click" CssClass="botao-relatorio" />

                <tbody>
                    <!-- Linhas da tabela com dados dos funcionários serão preenchidas no código-behind -->
                    <asp:Table ID="tabelaPagamentos" runat="server" OnRowCommand="tabelaPagamentos_RowCommand"  CssClass="tabela-pagamentos">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Data de Admissão</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Nome</asp:TableHeaderCell>
                            <asp:TableHeaderCell>CPF</asp:TableHeaderCell>
                            <asp:TableHeaderCell>ID</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cargo</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Salário Total</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Status de Pagamento</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
