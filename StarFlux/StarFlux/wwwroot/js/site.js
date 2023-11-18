function apagarTorre(id) {
    if (confirm('Confirma a exclusão da torre?'))
        location.href = 'torre/Delete?id=' + id;
}

function buscaTorres() {
    var codigoTorre = document.getElementById("codigoTorre").value;
    var nomeTorre = document.getElementById("nomeTorre").value;

    $.ajax({
        url: "/Torre/BuscaTorres",
        data: {
            codigo: codigoTorre,
            nome: nomeTorre
        },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById("resultadoConsultaTorres").innerHTML = dados;
            }
        }
    })
}