function apagarTorre(id) {
    Swal.fire({
        title: "Confirmação",
        text: "Deseja realmente excluir a torre?",
        icon: "warning",
        iconColor: "#1693a5",
        showCancelButton: true,
        confirmButtonColor: "#1693a5",
        cancelButtonColor: "#7ececa",
        confirmButtonText: "Sim, excluir!",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Torre Deletada!",
                text: "A torre foi deletada com sucesso.",
                icon: "success",
                iconColor: "#1693a5"
            }).then((result) => {
                if (result.isConfirmed) {
                    location.href = 'torre/Delete?id=' + id;
                }
            });
        }
    });
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
    Swal.fire({
        title: "Confirmação",
        text: "Deseja realmente excluir o apartamento?",
        icon: "warning",
        iconColor: "#1693a5",
        showCancelButton: true,
        confirmButtonColor: "#1693a5",
        cancelButtonColor: "#7ececa",
        confirmButtonText: "Sim, excluir!",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Apartamento Deletado!",
                text: "O apartamento foi deletado com sucesso.",
                icon: "success",
                iconColor: "#1693a5"
            }).then((result) => {
                if (result.isConfirmed) {
                    location.href = 'apartamento/Delete?id=' + id;
                }
            });
        }
    });
}

function buscaApartamentos() {
    var codigo = document.getElementById("codigo").value;
    var nome = document.getElementById("nome").value;
    var dataInicial = document.getElementById("dataInicial").value;
    var dataFinal = document.getElementById("dataFinal").value;
    var torre = document.getElementById("torre").value;
    var sensor= document.getElementById("sensor").value;

    $.ajax({
        url: "/Apartamento/BuscaApartamentos",
        data: {
            codigo: codigo,
            nome: nome,
            dataInicial: dataInicial,
            dataFinal: dataFinal,
            torre: torre,
            sensor: sensor
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
    Swal.fire({
        title: "Confirmação",
        text: "Deseja realmente excluir o habitante?",
        icon: "warning",
        iconColor: "#1693a5",
        showCancelButton: true,
        confirmButtonColor: "#1693a5",
        cancelButtonColor: "#7ececa",
        confirmButtonText: "Sim, excluir!",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Habitante Deletado!",
                text: "O habitante foi deletado com sucesso.",
                icon: "success",
                iconColor: "#1693a5"
            }).then((result) => {
                if (result.isConfirmed) {
                    location.href = 'habitante/Delete?id=' + id;
                }
            });
        }
    });
}

function buscaHabitantes() {
    var codigo = document.getElementById("codigo").value;
    var nome = document.getElementById("nome").value;
    var dataInicial = document.getElementById("dataInicial").value;
    var dataFinal = document.getElementById("dataFinal").value;
    var apartamento = document.getElementById("apartamento").value;
    var torre = document.getElementById("torre").value;

    $.ajax({
        url: "/Habitante/BuscaHabitantes",
        data: {
            codigo: codigo,
            nome: nome,
            dataInicial: dataInicial,
            dataFinal: dataFinal,
            apartamento: apartamento,
            torre: torre
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

function apagarSensor(id) {
    Swal.fire({
        title: "Confirmação",
        text: "Deseja realmente excluir o sensor?",
        icon: "warning",
        iconColor: "#1693a5",
        showCancelButton: true,
        confirmButtonColor: "#1693a5",
        cancelButtonColor: "#7ececa",
        confirmButtonText: "Sim, excluir!",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "Sensor Deletado!",
                text: "O sensor foi deletado com sucesso.",
                icon: "success",
                iconColor: "#1693a5"
            }).then((result) => {
                if (result.isConfirmed) {
                    location.href = 'sensor/Delete?id=' + id;
                }
            });
        }
    });
}

function buscaSensores() {
    var codigoSensor = document.getElementById("codigoSensor").value;
    var nomeSensor = document.getElementById("nomeSensor").value;
    var entidadeSensor = document.getElementById("entidadeSensor").value;

    $.ajax({
        url: "/Sensor/BuscaSensores",
        data: {
            codigo: codigoSensor,
            nome: nomeSensor,
            entidade: entidadeSensor
        },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById("resultadoConsultaSensores").innerHTML = dados;
            }
        }
    })
}