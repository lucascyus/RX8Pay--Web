function configurarSalario() {
    var cargoSelect = document.getElementById("cargo");
    var salarioDisplay = document.getElementById("salarioDisplay");

    cargoSelect.addEventListener("change", function () {
        var escolha = cargoSelect.value;
        var salario = 0.00;

        if (escolha === "AssistenteAdministrativo") {
            salariodois.textContent = 3200.00;
        } else if (escolha === "AssistenteVendas") {
            salario = 2800.00;
        } else {
            // Você pode adicionar outras condições para os diferentes cargos aqui.
        }

        salarioDisplay.textContent = "R$" + salario.toFixed(2);
    });
}

var cargoSelect = document.getElementById("cargo");
cargoSelect.addEventListener("change", configurarSalario);
