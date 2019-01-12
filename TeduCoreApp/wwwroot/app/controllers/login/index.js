var loginController = function () {
    this.initialize = function () {
        registerEvents();
    };

    var registerEvents = function () {
        $('#frm-login').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
            }
        });

        $('#btn-login').on('click', function (e) {
            if ($('#frm-login').valid()) {
                e.preventDefault();
                var user = $('#txt-user-name-modal').val();
                var pass = $('#txt-password-modal').val();
                login(user, pass);
            }
        });

        $('#-user-name-modal').on('keypress', function (e) {           
            if (e.which === 13) {
                e.preventDefault();
                if ($('#frm-login').valid()) {                   
                    var user = $('#txt-user-name-modal').val();
                    var pass = $('#txt-password-modal').val();
                    login(user, pass);
                }
            }
        });

        $('#txt-password-modal').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                if ($('#frm-login').valid()) {
                    var user = $('#txt-user-name-modal').val();
                    var pass = $('#txt-password-modal').val();
                    login(user, pass);
                }
            }
        });
    };

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                Username: user,
                Password: pass
            },
            dateType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = '/Admin/Home/Index';
                } else {
                    tedu.notify("Login failed", 'error');
                }
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                tedu.notify(err.Message, 'error');
            }
        });
    };
};