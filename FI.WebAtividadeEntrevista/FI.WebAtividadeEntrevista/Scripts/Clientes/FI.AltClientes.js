var corpo_tabela = document.querySelector("tbody");

localStorage.clear();
const localStorageBeneficiarios = JSON.parse(localStorage.getItem('beneficiarios'));
let beneficiarios = localStorage.getItem('beneficiarios') !== null ? localStorageBeneficiarios : []

const updateLocalStorage = () => {
    localStorage.setItem('beneficiarios', JSON.stringify(beneficiarios));
}

$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
        $('#formCadastro #Cpf').val(obj.Cpf);
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "Cpf": $(this).find("#Cpf").val(),
                "Id": $(this).find("#GlobalIdCliente").val(),
                "Beneficiarios": beneficiarios
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                    window.location.href = urlRetorno;
                }
        });
    })

    $('#beneficiarioModal').submit(function (e) {
        e.preventDefault();

        var linha = document.createElement("tr");

        var campo_cpf = document.createElement("td");
        campo_cpf.setAttribute("id", "tdCpf");        

        var campo_nome = document.createElement("td");
        campo_nome.setAttribute("id", "tdNome");

        var campo_alterar = document.createElement("td");
        var campo_excluir = document.createElement("td");

        var botao_alterar = document.createElement("button");
        botao_alterar.setAttribute('type', 'button');
        botao_alterar.setAttribute('onclick', 'alterarBeneficiario(this)');
        botao_alterar.setAttribute('class', 'btn btn-sm btn-info');
        botao_alterar.innerText = "Alterar";

        var botao_excluir = document.createElement("button");
        botao_excluir.setAttribute('type', 'button');
        botao_excluir.setAttribute('onclick', 'excluirBeneficiario(this)');
        botao_excluir.setAttribute('class', 'btn btn-sm btn-info');
        botao_excluir.innerText = "Excluir";

        var texto_cpf = document.createTextNode($(this).find("#BeneCpf").val());
        var texto_nome = document.createTextNode($(this).find("#BeneNome").val());

        campo_cpf.appendChild(texto_cpf);
        campo_nome.appendChild(texto_nome);
        campo_alterar.appendChild(botao_alterar);
        campo_excluir.appendChild(botao_excluir);

        linha.appendChild(campo_cpf);
        linha.appendChild(campo_nome);
        linha.appendChild(campo_alterar);
        linha.appendChild(campo_excluir);

        corpo_tabela.appendChild(linha);

        const beneficiario = {
            Index: beneficiarios.length + 1,
            Cpf: $(this).find("#BeneCpf").val(),
            Nome: $(this).find("#BeneNome").val(),
            Id: $(this).find("#BeneId").val(),
            IdCliente: $(this).find("#BeneIdCliente").val()
        }

        beneficiarios.push(beneficiario);

        updateLocalStorage();

        document.getElementById("BeneCpf").value = "";
        document.getElementById("BeneNome").value = "";
        document.getElementById("BeneId").value = "";
        document.getElementById("BeneIdCliente").value = "";
    });

    excluirBeneficiario = function (row) {
        var tdId = $(row).parents('tr').children('#tdId').text();
        var tdCpf = $(row).parents('tr').children('#tdCpf').text();

        if (tdId == "") {
            var ls = JSON.parse(localStorage.getItem('beneficiarios'));

            findAndRemove(ls, 'Cpf', tdCpf);

            beneficiarios = ls;

            updateLocalStorage();
        }

        if (tdId != "") {
            const beneficiario = {
                Index: beneficiarios.length + 1,
                Id: tdId,
                Exclusao: true
            }

            beneficiarios.push(beneficiario);

            updateLocalStorage();
        }


        $(row).parents('tr').remove();
    };

    function findAndRemove(array, property, value) {
        array.forEach(function (result, index) {
            if (result[property] === value) {
                //Remove from array
                array.splice(index, 1);
            }
        });
    }

    alterarBeneficiario = function (row) {
                
        var tdCpf = $(row).parents('tr').children('#tdCpf').text();
        var tdNome = $(row).parents('tr').children('#tdNome').text();
        var tdId = $(row).parents('tr').children('#tdId').text();

        if (tdId == "") {
            var ls = JSON.parse(localStorage.getItem('beneficiarios'));

            findAndRemove(ls, 'Cpf', tdCpf);

            beneficiarios = ls;

            updateLocalStorage();
        }

        document.getElementById("BeneCpf").value = tdCpf;
        document.getElementById("BeneNome").value = tdNome;
        document.getElementById("BeneId").value = tdId;

        $(row).parents('tr').remove();
    };
});
    


function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
