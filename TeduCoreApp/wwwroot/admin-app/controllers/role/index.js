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
                var that = $('#hidden-id-modal').val();
                saveRole(that);
            }
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var name = $(this).data('name');
            var that = $(this).data('id');
            deleteRole(that, name);
        });

        //Grant permission
        $('body').on('click', '.btn-grant', function (e) {
            e.preventDefault();
            var guid = $(this).data('id');
            $('#hidden-role-id-modal').val(guid);
            $.when(loadFunctionList()).done(fillPermission(guid));
            $('#modal-grant-permission').modal('show');
        });

        $("#btn-save-permission").off('click').on('click', function (e) {
            e.preventDefault();
            var permmissions = [];
            $.each($('#tbl-function tbody tr'), function (_i, item) {
                permmissions.push({
                    RoleId: $('#hidden-role-id-modal').val(),
                    FunctionId: $(item).data('id'),
                    CanRead: $(item).find('.ck-view').first().prop('checked'),
                    CanCreate: $(item).find('.ck-add').first().prop('checked'),
                    CanUpdate: $(item).find('.ck-edit').first().prop('checked'),
                    CanDelete: $(item).find('.ck-delete').first().prop('checked')
                });
            });
            savePermissions(permmissions);
        });
    }

    function loadFunctionList(callback) {
        var strUrl = "/Admin/Function/GetAll";
        return $.ajax({
            type: "GET",
            url: strUrl,
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var template = $('#result-data-function').html();
                var render = "";
                $.each(response, function (_i, item) {
                    render += Mustache.render(template, {
                        Name: item.Name,
                        isParent: item.ParentId === null,
                        treegridparent: item.ParentId !== null ? "treegrid-parent-" + item.ParentId : "",
                        Id: item.Id,
                        AllowCreate: item.AllowCreate ? "checked" : "",
                        AllowEdit: item.AllowEdit ? "checked" : "",
                        AllowView: item.AllowView ? "checked" : "",
                        AllowDelete: item.AllowDelete ? "checked" : "",
                        Status: tedu.getStatus(item.Status)
                    });
                });

                if (render !== undefined) {
                    $('#lst-data-function').html(render);
                }
                $('.tree').treegrid({
                    'initialState': 'collapsed'
                    //'saveState': true,
                });


                $('#ck-check-all-view').on('click', function () {
                    $('.ck-view').prop('checked', $(this).prop('checked'));
                });

                $('#ck-check-all-create').on('click', function () {
                    $('.ck-add').prop('checked', $(this).prop('checked'));
                });
                $('#ck-check-all-edit').on('click', function () {
                    $('.ck-edit').prop('checked', $(this).prop('checked'));
                });
                $('#ck-check-all-delete').on('click', function () {
                    $('.ck-delete').prop('checked', $(this).prop('checked'));
                });

                $('.ck-view').on('click', function () {
                    if ($('.ck-view:checked').length === response.length) {
                        $('#ck-check-all-View').prop('checked', true);
                    } else {
                        $('#ck-check-all-View').prop('checked', false);
                    }
                });
                $('.ck-add').on('click', function () {
                    if ($('.ck-add:checked').length === response.length) {
                        $('#ck-check-all-Create').prop('checked', true);
                    } else {
                        $('#ck-check-all-Create').prop('checked', false);
                    }
                });
                $('.ck-edit').on('click', function () {
                    if ($('.ck-edit:checked').length === response.length) {
                        $('#ck-check-all-Edit').prop('checked', true);
                    } else {
                        $('#ck-check-all-Edit').prop('checked', false);
                    }
                });
                $('.ck-delete').on('click', function () {
                    if ($('.ck-delete:checked').length === response.length) {
                        $('#ck-check-all-Delete').prop('checked', true);
                    } else {
                        $('#ck-check-all-Delete').prop('checked', false);
                    }
                });

                

                if (callback !== undefined) {
                    callback();
                }
            },
            error: function (status) {
                console.log("Has an error in loading functions: ", status);
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function fillPermission(roleId) {
        var strUrl = "/Admin/Role/ListAllFunction";
        return $.ajax({
            type: "POST",
            url: strUrl,
            data: {
                roleId: roleId
            },
            dataType: "json",
            beforeSend: function () {
                tedu.stopLoading();
            },
            success: function (response) {
                var litsPermission = response;
                $.each($('#tbl-function tbody tr'), function (_i, item) {
                    $.each(litsPermission, function (_j, jitem) {
                        if (jitem.FunctionId === $(item).data('id')) {
                            $(item).find('.ck-view').first().prop('checked', jitem.CanRead);
                            $(item).find('.ck-add').first().prop('checked', jitem.CanCreate);
                            $(item).find('.ck-edit').first().prop('checked', jitem.CanUpdate);
                            $(item).find('.ck-delete').first().prop('checked', jitem.CanDelete);
                        }
                    });
                });

                if ($('.ck-view:checked').length === $('#tbl-function tbody tr .ck-view').length) {
                    $('#ck-check-all-view').prop('checked', true);
                } else {
                    $('#ck-check-all-view').prop('checked', false);
                }
                if ($('.ck-add:checked').length === $('#tbl-function tbody tr .ck-add').length) {
                    $('#ck-check-all-create').prop('checked', true);
                } else {
                    $('#ck-check-all-create').prop('checked', false);
                }
                if ($('.ck-edit:checked').length === $('#tbl-function tbody tr .ck-edit').length) {
                    $('#ck-check-all-edit').prop('checked', true);
                } else {
                    $('#ck-check-all-edit').prop('checked', false);
                }
                if ($('.ck-delete:checked').length === $('#tbl-function tbody tr .ck-delete').length) {
                    $('#ck-check-all-delete').prop('checked', true);
                } else {
                    $('#ck-check-all-delete').prop('checked', false);
                }
                tedu.stopLoading();
            },
            error: function (status) {
                console.log("Has an error in filling permissions: ", status);
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidden-id-modal').val('');
        $('#txt-name-modal').val('');
        $('#txt-description-modal').val('');
    }

    function loadRoles(isPageSizeChanged) {
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
                }, isPageSizeChanged);
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
                loadRoles();
            },
            error: function (status) {
                console.log("Has an error in saving role: ", status);
                tedu.notify('Has an error in saving role', 'error');
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

    function savePermissions(permmissions) {
        $.ajax({
            type: "POST",
            url: "/Admin/Role/SavePermission",
            data: {
                permissionViewModels: permmissions,
                roleId: $('#hidden-role-id-modal').val()
            },
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                tedu.notify("Save permissions successfully", 'success');
                $('#modal-grant-permission').modal('hide');
            },
            error: function () {
                console.log("Has an error in save permission progress: ", status);
                tedu.notify('Has an error in save permission progress ', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

};