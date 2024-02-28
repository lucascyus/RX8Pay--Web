function toggleUserPanel() {
    var userPanel = document.getElementById('userPanel');
    if (userPanel.style.display === 'none' || userPanel.style.display === '') {
        userPanel.style.display = 'block';

        // Aqui, você pode preencher o nome do usuário dinamicamente (suponha que o nome do usuário seja armazenado em uma variável 'nomeUsuario')
        var userName = document.getElementById('userName');
        userName.innerHTML = nomeUsuario;
    } else {
        userPanel.style.display = 'none';
    }
}

function logout() {
    // Implemente a ação de logout aqui
    alert("Usuário deslogado."); // Exemplo: exibe um alerta
}
