var loginController = function () {
    this.initialize = function () {
        registerEvents();
    };

    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                var user = $('#txtUsername').val();
                var pass = $('#txtPassword').val();
                login(user, pass);
            }           
        });

        //$('body').on('keypress', function (e) {
        //    e.preventDefault();
        //    if (e.which === 13) {
        //        if ($('#frmLogin').valid()) {
        //            e.preventDefault();
        //            var user = $('#txtUsername').val();
        //            var pass = $('#txtPassword').val();
        //            login(user, pass);
        //        }
        //    }
        //});
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