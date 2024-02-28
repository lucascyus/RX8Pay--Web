<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CriarConta.aspx.cs" Inherits="agoraVai.pages.CriarConta" %>

<!DOCTYPE html>
<html lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Redefinir Senha - RX8Pay</title>
    <link rel="stylesheet" type="text/css" href="~/css/styleSenha.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap" rel="stylesheet" />

    <script src="../js/getUser.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <div class="retangulo">
                <div class="retangulo_roxo">
                    <img src="../img/logo_paia__1_-removebg.png" alt="Logo RX8Pay" />
                </div>
                <div class="formulario">
                    <h2><span>Cadastre&nbsp;</span> sua conta</h2>
                    <form>
                        <label for="login">Login</label>
                        <asp:TextBox ID="login" runat="server" CssClass="input-padrao" required="required" />
                        <label for="email">Email</label>
                        <asp:TextBox ID="eemail" runat="server" CssClass="input-padrao" TextMode="Email" required="required" />
                        <label for="password" class="senha">Senha</label>
                        <asp:TextBox ID="senha" runat="server" CssClass="input-padrao" TextMode="Password" required="required" />
                        <label for="password" class="senha">Confirmar Senha</label>
                        <asp:TextBox ID="confirmarSenha" runat="server" CssClass="input-padrao" TextMode="Password" required="required" />
                        <div id="MensagemSucesso" class="mensagem-sucesso" runat="server" visible="false">
                            CONTA CADASTRADA
                        </div>
                        <div id="MensagemErroDados" class="mensagem-erro" runat="server" visible="false">
                            OS DADOS NÃO CONFEREM E/OU<br> JÁ ESTÃO VINCULADOS
                        </div>
                        <asp:Button ID="CadastrarButton" runat="server" Text="Cadastrar" CssClass="Entrar" OnClick="CadastrarButton_Click" />
                        <a href="Login.aspx" class="container__botao">Já tenho uma conta</a>
                    </form>
                </div>
            </div>
        </main>
    </form>
</body>
</html>

