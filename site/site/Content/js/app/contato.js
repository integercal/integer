$().ready(function () {
    jQuery.extend(jQuery.validator.messages, {
        required: ""
    });

    $('#formContato').validate({
        errorClass:'error',
        rules: {
            nome: { required: true },
            email: { required: true, email: true },
            paroquia: { required: true },
            comentario: { required: true }
        },
        submitHandler: function (form) {
            $.post("contato", $(form).serialize(), function () {
                $('#msgerror').fadeOut();
                $('#msgsuccess').fadeIn();
            }).fail(function () {
                $('#msgsuccess').fadeOut();
                $('#msgerror').fadeIn();
            }).done(function () {
                var alertTop = $(".alert:visible").offset().top;
                var windowTop = $(window).scrollTop();
                if (alertTop < windowTop)
                {
                    $('html, body').animate({
                        scrollTop: $(".alert:visible").offset().top
                    }, 1000);
                }
            });
        },
        errorPlacement: function (error, element) {
            element.closest(".control-group").addClass('error'); 
        },
        highlight: function(element) {
            $(element).closest(".control-group").addClass('error');
        },
        unhighlight: function(element) {
            $(element).closest(".control-group").removeClass("error");
        }
    });
});
