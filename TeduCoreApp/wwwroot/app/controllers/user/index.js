var userController = function () {
    this.initialize = function () {
        loadUsers();
        registerEvents();
    };

    function registerEvents() {
        //Init validation
        $('#form-maintainance-modal').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                'txt-full-name-modal': { required: true },
                'txt-user-name-modal': { required: true },
                'txt-password-modal': {
                    required: true,
                    minlength: 6
                },
                'txt-confirm-password-modal': {
                    equalTo: "#txt-password-modal"
                },
                'txt-email-modal': {
                    required: true,
                    email: true
                }
            }
        });

        //Todo: binding events to controls
        $('#ddl-show-page').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadUsers(true);
            return false;
        });

        $('#txt-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadUsers();
            }
            return false;
        });

        $('#btn-search').on('click', function () {
            loadUsers();
            return false;
        });

        $('#btn-create').on('click', function () {
            resetFormMaintainance();
            initRoleList();
            $('#modal-add-edit').modal('show');
            return false;
        });

        $('#btn-save').on('click', function () {
            if ($('#form-maintainance-modal').valid()) {
                var that = $('#hidden-id-modal').val();
                saveUser(that);
            }
            return false;
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadUser(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var username = $(this).data('username');
            var that = $(this).data('id');
            deleteUser(that, username);
        });
    }

    function loadUsers(isPageChanged) {
        var template = $('#table-template').html();
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txt-keyword').val(),
                page: tedu.configs.pageIndex,
                pageSize: tedu.configs.pageSize
            },
            url: '/Admin/User/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var render = "";
                $.each(response.Results, function (_i, item) {
                    render += Mustache.render(template, {
                        FullName: item.FullName,
                        Id: item.Id,
                        UserName: item.UserName,
                        Avatar: item.Avatar === null ? '<img src="/admin-side/images/user.png" width=25 />' : '<img src="' + item.Avatar + '" width=25 />',
                        DateCreated: tedu.dateTimeFormatJson(item.DateCreated),
                        Status: tedu.getStatus(item.Status)
                    });
                });
                $('#lbl-total-records').text(response.RowCount);
                $('#tbl-content').html(render);
                //if (render !== "") {
                //    $('#tbl-content').html(render);
                //}
                pagination.wrapPaging(response.RowCount, function () {
                    loadUsers();
                }, isPageChanged);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading product data.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function loadUser(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/User/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidden-id-modal').val(data.Id);
                $('#txt-full-name-modal').val(data.FullName);
                $('#txt-user-name-modal').val(data.UserName);
                $('#txt-email-modal').val(data.Email);
                $('#txt-phone-number-modal').val(data.PhoneNumber);
                $('#ck-status-modal').prop('checked', data.Status === 1);

                initRoleList(data.Roles);

                disableFieldEdit(true);
                $('#modal-add-edit').modal('show');
            },
            error: function (status) {
                console.log("Has an error in loading a user: ", status);
                tedu.notify('Has an error in loading a user', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveUser(id) {
        var fullName = $('#txt-full-name-modal').val();
        var userName = $('#txt-user-name-modal').val();
        var password = $('#txt-password-modal').val();
        var email = $('#txt-email-modal').val();
        var phoneNumber = $('#txt-phone-number-modal').val();
        var roles = [];
        $.each($('input[name="roles"]'), function (_i, item) {
            if ($(item).prop('checked') === true)
                roles.push($(item).prop('value'));
        });
        var status = $('#ck-status-modal').prop('checked') === true ? 1 : 0;
        $.ajax({
            type: "POST",
            url: "/Admin/User/SaveEntity",
            data: {
                Id: id,
                FullName: fullName,
                UserName: userName,
                Password: password,
                Email: email,
                PhoneNumber: phoneNumber,
                Status: status,
                Roles: roles
            },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function () {
                tedu.notify('Save user succesful', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                loadUsers(true);
            },
            error: function (status) {
                console.log("Has an error in saving a users: ", status);
                tedu.notify('Has an error in saving a users', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function deleteUser(id, username) {
        tedu.confirm("Are you sure to delete user: " + username + "?", function () {
            $.ajax({
                type: "POST",
                url: "/Admin/User/Delete",
                data: { id: id },
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function () {
                    tedu.notify('Delete user ' + username + ' successful', 'success');
                    loadUsers(true);
                },
                error: function (status) {
                    console.log("Has an error in deleting a user: ", status);
                    tedu.notify('Has an error in deleting a user ' + username, 'error');
                },
                complete: function () {
                    tedu.stopLoading();
                }
            });
        });
    }

    function disableFieldEdit(disabled) {
        $('#txt-user-name-modal').prop('disabled', disabled);
        $('#txt-password-modal').prop('disabled', disabled);
        $('#txt-confirm-password-modal').prop('disabled', disabled);

    }

    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidden-id-modal').val('');
        initRoleList();
        $('#txt-full-name-modal').val('');
        $('#txt-user-name-modal').val('');
        $('#txt-password-modal').val('');
        $('#txt-confirm-password-modal').val('');
        $('input[name="roles"]').removeAttr('checked');
        $('#txt-email-modal').val('');
        $('#txt-phone-number-modal').val('');
        $('#ck-status-modal').prop('checked', true);

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                //var data = response;
                var render = '';
                $.each(response, function (_i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            },
            error: function (status) {
                console.log("Has an error in loading roles", status);
                tedu.notify("Cannot loading roles.", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }
};