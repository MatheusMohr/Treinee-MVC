var escola = function () {

    return {
        carregarCidadePorUF: function (uf) {
            if (!uf) return;

            var select = document.getElementById("Endereco_Cidade");
            select.innerHTML = '<option value="">Carregando...</option>';

            fetch(`/Escola/ObterCidadesPorUf?uf=${uf}`)
                .then(response => response.json())
                .then(data => {
                    select.innerHTML = '<option value="">--Selecione a Cidade--</option>';

                    data.forEach(function (cidade) {
                        var option = document.createElement("option");
                        option.value = cidade.id;
                        option.text = cidade.nome;
                        select.appendChild(option);
                    });
                })
                .catch(err => {
                    console.error("Erro ao carregar cidades: ", err);
                    select.innerHTML = '<option value="">Erro ao carregar</option>';
    });
        }
    };

}();