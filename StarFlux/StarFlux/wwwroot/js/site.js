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

function apagarApartamento(id) {
    if (confirm('Confirma a exclusão do apartamento?'))
        location.href = 'apartamento/Delete?id=' + id;
}

function buscaApartamentos() {
    var codigo = document.getElementById("codigo").value;
    var nome = document.getElementById("nome").value;
    var dataInicial = document.getElementById("dataInicial").value;
    var dataFinal = document.getElementById("dataFinal").value;
    var torre = document.getElementById("torre").value;

    $.ajax({
        url: "/Apartamento/BuscaApartamentos",
        data: {
            codigo: codigo,
            nome: nome,
            dataInicial: dataInicial,
            dataFinal: dataFinal,
            torre: torre
        },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById("resultadoConsulta").innerHTML = dados;
            }

        }
    })
}

function apagarHabitante(id) {
    if (confirm('Confirma a exclusão do habitante?'))
        location.href = 'apartamento/Delete?id=' + id;
}

function buscaHabitantes() {
    var codigo = document.getElementById("codigo").value;
    var nome = document.getElementById("nome").value;
    var dataInicial = document.getElementById("dataInicial").value;
    var dataFinal = document.getElementById("dataFinal").value;
    var apartamento = document.getElementById("apartamento").value;

    $.ajax({
        url: "/Habitante/BuscaHabitantes",
        data: {
            codigo: codigo,
            nome: nome,
            dataInicial: dataInicial,
            dataFinal: dataFinal,
            apartamento: apartamento
        },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById("resultadoConsultaHabitantes").innerHTML = dados;
            }

        }
    })
}