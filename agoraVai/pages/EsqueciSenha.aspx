<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EsqueciSenha.aspx.cs" Inherits="agoraVai.pages.EsqueciSenha" %>

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
                    <h2><span>Esqueceu&nbsp;</span> sua senha?</h2>
                    <h3 class="texto">Insira aqui os seus dados<br> e sua nova senha.</h3>
                    <form>
                        <label for="login">Login</label>
                        <asp:TextBox ID="login" runat="server" CssClass="input-padrao" required="required" />
                        <label for="email">Email</label>
                        <asp:TextBox ID="eemail" runat="server" CssClass="input-padrao" TextMode="Email" required="required" />
                        <label for="password" class="nova-senha">Nova Senha</label>
                        <asp:TextBox ID="novaSenha" runat="server" CssClass="input-padrao" TextMode="Password" required="required" />
                        <div id="MensagemErroDados" class="mensagem-erro" runat="server" visible="false">
                            OS DADOS NÃO CONFEREM
                        </div>
                        <div id="MensagemSucesso" class="mensagem-sucesso" runat="server" visible="false">
                            SENHA REDEFINIDA
                        </div>
                        <asp:Button ID="RedefinirButton" runat="server" Text="Redefinir Senha" CssClass="Entrar" OnClick="RedefinirButton_Click" />
                        <a href="Login.aspx" class="container__botao">Lembrei minha senha</a>
                    </form>
                </div>
            </div>
        </main>
    </form>
</body>
</html>
