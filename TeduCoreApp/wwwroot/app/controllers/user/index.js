var userController = function () {
    this.initialize = function () {
        //loadCategories();
        loadUsers();
        registerEvents();
        //registerControls();
    };

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });

        //Todo: binding events to controls
        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadUsers(true);
        });

        $('#txtKeyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadUsers();
            }
        });

        $("#btnSearch").on('click', function () {
            loadUsers();
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            initRoleList();
            $('#modal-add-edit').modal('show');
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
            tedu.confirm("Are you sure to delete user: " + username + "?", function () {
                deleteUser(that, name);
            });
        });
    }

    function loadUsers(isPageChanged) {
        var template = $('#table-template').html();
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val(),
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
                $('#lblTotalRecords').text(response.RowCount);
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
                $('#hidId').val(data.Id);
                $('#txtFullName').val(data.FullName);
                $('#txtUserName').val(data.UserName);
                $('#txtEmail').val(data.Email);
                $('#txtPhoneNumber').val(data.PhoneNumber);
                $('#ckStatus').prop('checked', data.Status === 1);

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

    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);

    }

    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidId').val('');
        initRoleList();
        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ckStatus').prop('checked', true);

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
}