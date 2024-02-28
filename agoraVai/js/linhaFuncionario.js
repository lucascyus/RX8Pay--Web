<script type="text/javascript">
    $(document).ready(function () {
        // Manipula o clique duplo nas linhas da tabela
        $(".linhaFuncionario").dblclick(function () {
            // Obtém o Id do funcionário da célula correspondente
            var funcionarioId = $(this).find('td:eq(3)').text(); // Supondo que o Id esteja na quarta coluna

            // Exibe um diálogo de confirmação
            var confirmacao = confirm("Deseja realmente excluir o funcionário?");

            // Se o usuário confirmar, envie uma solicitação para excluir o funcionário no lado do servidor
            if (confirmacao) {
                excluirFuncionario(funcionarioId);
            }
        });

    // Função para enviar uma solicitação AJAX para excluir o funcionário
    function excluirFuncionario(funcionarioId) {
        $.ajax({
            type: "POST",
            url: "Pagamentos.aspx/ExcluirFuncionario",
            data: "{funcionarioId: '" + funcionarioId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // Se a exclusão for bem-sucedida, recarregue a tabela ou faça as alterações necessárias
                alert("Funcionário excluído com sucesso!");
                location.reload();
            },
            error: function (response) {
                // Se ocorrer um erro na exclusão, exiba uma mensagem de erro
                alert("Erro ao excluir o funcionário. Detalhes: " + response.responseText);
            }
        });
        }
    });
</script>
