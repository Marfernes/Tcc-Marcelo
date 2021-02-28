$(function () {

    "use string";

    window.EventoModal = window.EventoModal || {};

    EventoModal.AbrirModal = function () {
        $(document).ready(function () {
            $(document).on("click", ".btn-abrirModal", function () {
                var carregarModal = $(".modal.abrir-modal");
                carregarModal.empty();
                var url = $(this).data("abrirmodal");
                $.ajax({
                    url: url,
                    method: "POST",
                    success: function (result) {
                        carregarModal.append(result);
                        carregarModal.modal("show");

                        Mask.Values();
                        $("form").validate({
                            invalidHandler: function (event, validator) {
                                var errors = validator.numberOfInvalids();
                                if (errors) {
                                    var message = 'Este campo é obrigatório.';
                                    $("div.error span").html(message);
                                    $("div.error").show();
                                } else {
                                    $("div.error").hide();
                                }
                            }
                        });
                        $("body").LoadingOverlay("hide");
                    },
                    error: function () {
                        Lobibox.notify('error', { position: 'top right', sound: '../../sounds/sound4', delay: 3000, msg: 'Erro ao abrir informações', title: 'Erro', icon: 'frown icon' });
                        carregarModal.modal("hide");
                        $("body").LoadingOverlay("hide");
                    }
                });
            });

            $(document).on("click", "#btn-fechar-modal", function () {
                $(".modal.abrir-modal").modal("hide");
            });
        });
    };

    EventoModal.AbrirModal();
});