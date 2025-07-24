$(document).ready(function () {

   
    $('#loginForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: '/api/auth/login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                email: $('#email').val(),
                password: $('#password').val()
            }),
            success: function (res) {
                localStorage.setItem('token', res.token);
                location.href = '/Index';
            },
            error: function (err) {
                $('#loginAlert').addClass('alert-danger').text("Error al iniciar sesión").show();
            }
        });
    });

    $('#registerForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: '/api/auth/register',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                nombreUsuario: $('#nombreUsuario').val(),
                email: $('#email').val(),
                password: $('#password').val()
            }),
            success: function () {
                $('#registerAlert').removeClass('alert-danger').addClass('alert-success')
                    .text("Usuario registrado correctamente").show();
            },
            error: function () {
                $('#registerAlert').removeClass('alert-success').addClass('alert-danger')
                    .text("No se pudo registrar el usuario").show();
            }
        });
    });

    
     

});