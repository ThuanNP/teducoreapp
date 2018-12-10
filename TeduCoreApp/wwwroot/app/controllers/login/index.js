var loginController = function () {
    this.initialize = function () {
        registerEvents();
    };

    var registerEvents = function () {
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            var user = $('#txtUsername').val();
            var pass = $('#txtPassword').val();
            login(user, pass);
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
                    tedu.notify('Đăng nhập không đúng.', 'error');
                }
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                tedu.notify(err.Message, 'error');
            }
        });
    };
};