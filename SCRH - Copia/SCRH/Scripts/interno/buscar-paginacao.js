$(function () {

    "use script";

    $('#form-search').removeClass('d-none');

    window.Paginacao = window.Paginacao || {};

    var divQueCarregaItens = $(".carregar-itens");

    Paginacao.Carregar = function () {
        $(document).on("keyup", "#input-pesquisa", function () {
            divQueCarregaItens.LoadingOverlay("show");
            Paginacao.BuscarInformacao();
        });

        $(document).on("click", "#pageLink a[href]", function () {
            divQueCarregaItens.LoadingOverlay("show");
            var paginaAtual = $("#pageLink").data("paginaatual");
            var paginaHref = $(this).attr("href");
            var pg = Paginacao.TipoHaSerBuscado(paginaHref);
            var pagina = $(this).text();
            switch (pagina) {
                case "»":
                    pagina = paginaAtual + 1;
                    break;
                case "«":
                    pagina = paginaAtual - 1;
                    break;
                case "»»":
                    pagina = pg;
                    break;
                case "««":
                    pagina = pg;
                    break;
                default:
            }
            Paginacao.BuscarInformacao(pagina);
            return false;
        });
    };

    Paginacao.BuscarInformacao = function (pagina = 1) {
        divQueCarregaItens.empty();
        var url = $(".carregar-itens").data("url");
        var textoDigitado = $("#input-pesquisa").val();
        $.ajax({
            url: url,
            data: { textoDigitado: textoDigitado, pagina },
            method: "POST",
            success: function (result) {
                divQueCarregaItens.empty();
                divQueCarregaItens.append(result);
                Mask.Values();
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