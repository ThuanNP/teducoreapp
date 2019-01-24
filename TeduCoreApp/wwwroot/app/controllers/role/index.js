var roleController = function () {
    this.initialize = function () {
        loadRoles();
        registerEvents();
    };

    function registerEvents() {
        //Init validation
        $('#form-maintainance-modal').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                'txt-name-modal': { required: true }
            }
        });

        //Todo: binding events to controls
        $('#ddl-show-page').on('change', function (e) {
            e.preventDefault();
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadUsers(true);
        });

        $('#btn-search').on('click', function (e) {
            e.preventDefault();
            loadRoles();
        });

        $('#txt-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadRoles();
            }
        });

        $('#btn-create').on('click', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            resetFormMaintainance();
            loadRole(that);

        });

        $('#btn-save').on('click', function (e) {
            if ($('#form-maintainance-modal').valid()) {
                e.preventDefault();
                var that = $(this).data('id');
                saveRole(that);
            }
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var name = $(this).data('name');
            var that = $(this).data('id');
            deleteRole(that, name);
        });
    }

    function resetFormMaintainance() {
        $('#hidden-id-modal').val('');
        $('#txt-name-modal').val('');
        $('#txt-description-modal').val('');
    }

    function loadRoles(isPageChanged) {
        var template = $('#table-template').html();
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txt-keyword').val(),
                page: tedu.configs.pageIndex,
                pageSize: tedu.configs.pageSize
            },
            url: '/Admin/Role/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var render = "";
                $.each(response.Results, function (_i, item) {
                    render += Mustache.render(template, {
                        Name: item.Name,
                        Description: item.Description,
                        Id: item.Id
                    });
                });
                $('#lbl-total-records').text(response.RowCount);
                $('#tbl-content').html(render);
                pagination.wrapPaging(response.RowCount, function () {
                    loadRoles();
                }, isPageChanged);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading role data.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function loadRole(_id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Role/GetById",
            data: { id: _id },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidden-id-modal').val(data.Id);
                $('#txt-name-modal').val(data.Name);
                $('#txt-description-modal').val(data.Description);
                $('#modal-add-edit').modal('show');
            },
            error: function (status) {
                console.log("Has an error in loading a role: ", status);
                tedu.notify('Has an error in loading a role', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveRole(id) {
        var name = $('#txt-name-modal').val();
        var description = $('#txt-description-modal').val();
        $.ajax({
            type: "POST",
            url: "/Admin/Role/SaveEntity",
            data: {
                Id: id,
                Name: name,
                Description: description
            },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                tedu.notify("Update role " + response.Name + " successful", 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                loadRoles(true);
            },
            error: function (status) {
                console.log("Has an error in loading roles: ", status);
                tedu.notify('Has an error in loading roles', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function deleteRole(_id, name) {
        tedu.confirm("Are you sure to delete role: " + name + "?", function () {
            $.ajax({
                type: "DELETE",
                url: "/Admin/Role/Delete",
                data: { id: _id },
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function () {
                    tedu.notify('Delete role ' + name + ' successful', 'success');
                    loadRoles(true);
                },
                error: function (status) {
                    console.log("Has an error in deleting a role: ", status);
                    tedu.notify('Has an error in deleting a role ' + name, 'error');
                },
                complete: function () {
                    tedu.stopLoading();
                }
            });
        });
    }

};