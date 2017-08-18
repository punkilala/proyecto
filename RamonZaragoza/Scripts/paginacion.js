var enlace;
var clase;
$(document).ready(function () {

    //escuchar botones
    $('body').on('click', 'button', function () {
        var url;
        var boton = $(this).attr('id');
        //si se pulso el boton para borrar algun registro
        if (boton == "btnEliminar") {
            //comprobar si se seleccionaros registros
            if ($(".seleccion:checked").length == 0) {
                alert('Debe seleccionar algún registro');
                return false;
            }
            if (!confirm('¿Esta seguro de realizar esta acción?')) return false;
        } else {
            //es boton para borrar input
            var elInput = $(this).attr('id').substring(4);
            $('#' + elInput).val('');
        }
        var form = $(this).closest('form');
        inicioAjax(form);
    });

    //escuchar  select
    $('body').on('change', 'select', function () {
        var form = $(this).closest('form');
        inicioAjax(form);
    });

    //escuchar input
    $('body').on('change', 'input', function () {
        //evito que se envie  si se clickea los checkbox... estos van por el boton eliminar
        if ($(this).attr('name') == "idEliminar") return false;
        var form = $(this).closest('form');
        inicioAjax(form);
    });

    //escuchar enlaces solo envio por ajax las ordenaciones y la paginacion
    $('body').on('click', 'a', function () {;
        //es paginacion
        //la paginacion su ul tiene una clase llamada pagination
        var ul = $(this).closest('ul');
        if (ul.attr('class') == "pagination") {
            var href = $(this).attr('href');
            if (href != undefined) {
                url = href.split('&')
                //para evitar que me cambie de pagina
                $(this).attr('href', '#');
                pagina = url[0].split('=');
                //pagina donde quiero ir
                $('#pagina').val(pagina[1]);
                var form = $(this).closest('form');
                inicioAjax(form);
            }
        } else if ($(this).hasClass("filtro")) {
            //es ordenación
            // para enviar por ajax el enlace tiene que tener una clase llamada filtro
            //componer el nombre del input oculto relacionado con cada enlace que acabo de clickear
            var campo = $(this).attr('id') + "By";
            enlace = $(this).attr('id');
            switch ($('#' + campo).val()) {
                case "":
                    $('#' + campo).val('Desc')
                    clase = "glyphicon glyphicon-chevron-up";
                    break;
                case "Desc":
                    $('#' + campo).val('Asc')
                    clase = "glyphicon glyphicon-chevron-down"
                    break;
                default:
                    $('#' + campo).val('');
                    clase = "";
                    break;
            }

            QuitarOrdenacion();
            var form = $(this).closest('form');
            inicioAjax(form);
        }   
    });

});

var inicioAjax = function (form) {

    // Creo un div que bloqueara todo el formulario
    var block = $('<div class="block-loading" />');
    form.prepend(block)
    url = form.attr('action');

    //empiezo ajax
    $.ajax({
        type: 'POST',
        url: url,
        data: form.serialize(),
        success: function (respuesta) {
            block.remove();
            $('#parcial').html(respuesta);
            $('#' + enlace).addClass(clase);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ' ' + errorThrown);
        }
    });
}
//solo ordeno por el concepto que hago click , el resto lo borro por si tiene algo;
var QuitarOrdenacion = function () {
    //en enlace que acabo de hacer click
    var enlaceClick = enlace + "By";
    //recorrro todos los enlaces que hay dentro de mi tabla que son los enlaces de ordenacion
    $("#miTabla a").each(function () {
        var suId = ($(this).attr('id'));
        //obtengo el campo  oculto relacionado con el enlace
        var campoOculto = $('#' + suId + "By").attr('id');
        //borro todo la ordenacion que pueda haber en los campos ocultos menos el que acabo de hacer click
        if (enlaceClick != campoOculto) $('#' + campoOculto).val('');
    });
}