$(function () {

    "use script";

    window.Paginacao = window.Paginacao || {};

    var divQueCarregaItens = $(".carregar-itens");

    Paginacao.Carregar = function () {
        $(document).on("click", ".btn-buscar-data", function () {
            divQueCarregaItens.LoadingOverlay("show");
            Paginacao.BuscarInformacao();
        });
    };

    Paginacao.BuscarInformacao = function (pagina = 1) {
        divQueCarregaItens.empty();
        var url = $(".carregar-itens").data("url");
        var dataInicio = $("#de").val();
        var dataFim = $("#ate").val();
        $.ajax({
            url: url,
            data: { dataInicio, dataFim },
            method: "POST",
            success: function (result) {
                divQueCarregaItens.empty();
                divQueCarregaItens.append(result);
                //Mask.Values();
                divQueCarregaItens.LoadingOverlay("hide");
            },
            error: function () {
                Lobibox.notify('error', { position: 'top right', sound: '../../sounds/sound4', delay: 3000, msg: 'Erro ao pesquisar informação', title: 'Erro', icon: 'frown icon' });
                divQueCarregaItens.LoadingOverlay("hide");
            }
        });
    };

    Paginacao.TipoHaSerBuscado = function (paginaHref) {
        return paginaHref.replace(/\D/g, '');
    };

    Paginacao.Carregar();
});