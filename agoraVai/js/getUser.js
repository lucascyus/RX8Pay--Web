// Coloque isso em seu arquivo JavaScript (getUser.js) ou em uma seção de script no seu HTML
document.addEventListener("DOMContentLoaded", function () {
    getLoggedInUser();
});

function getLoggedInUser() {
    var username = document.cookie.replace(/(?:(?:^|.*;\s*)username\s*=\s*([^;]*).*$)|^.*$/, "$1");
    if (username) {
        // Preencha o nome do usuário no painel
        var userNameElement = document.getElementById("userName");
        userNameElement.textContent = username;
    }
}

function PreencherNomeUsuario(username) {
    // Verifique se o argumento username não está vazio ou nulo antes de preenchê-lo na página
    if (username) {
        var userNameElement = document.getElementById("userName");
        userNameElement.textContent = username;
    } else {
        // Caso o username seja vazio ou nulo, você pode definir um valor padrão ou lidar com isso de outra forma.
        var userNameElement = document.getElementById("userName");
        userNameElement.textContent = "Usuário Desconhecido";
    }
}
