
var productCategoryController = function () {
    this.initialize = function () {
        registerEvents();
        loadProductCategories();
    };

    function registerEvents() {

        $('#btn-create').off('click').on('click', function (e) {
            e.preventDefault();           
            s();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            loadProductCategory();
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            tedu.confirm("Are you sure to delete?", function () {
                var that = parseInt($('#hidden-id-modal').val());
                deleteProductCategory(that);
            });
        });

        $('#btn-save').off('click').on('click', function (e) {
            e.preventDefault();
            var that = parseInt($('#hidden-id-modal').val());
            saveProductCategory(that);
        });

        $('#btn-select-imgage').on('click', function () {
            $('#txt-input-image-file').click();
        });

        $("#txt-input-image-file").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txt-image-modal').val(path);
                    tedu.notify('Upload image succesful!', 'success');
                },
                error: function (status) {
                    console.log("There was error uploading files: ", status);
                    tedu.notify('There was error uploading files!', 'error');
                }
            });
        });
    }

    function deleteProductCategory(id) {
        $.ajax({
            type: "DELETE",
            url: "/Admin/Productcategory/Delete",
            data: { id: id },
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function () {
                tedu.notify('Deleting success', 'success');
                loadProductCategories();
            },
            error: function (status) {
                console.log("Has an error in deleting progress: ", status);
                tedu.notify('Has an error in deleting progress', 'error');
                tedu.stopLoading();
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveProductCategory(id) {
        var name = $('#txt-name-modal').val();
        var parentId = $('#ddl-category-id-modal').combotree('getValue');
        var description = $('#txt-description-modal').val();

        var image = $('#txt-image-modal').val();
        var order = parseInt($('#txtOrderM').val());
        var homeOrder = $('#txtHomeOrderM').val();

        var seoKeyword = $('#txtSeoKeywordM').val();
        var seoMetaDescription = $('#txtSeoDescriptionM').val();
        var seoPageTitle = $('#txt-seo-page-title-modal').val();
        var seoAlias = $('#txt-seo-alias-modal').val();
        var status = $('#ck-status-modal').prop('checked') === true ? 1 : 0;
        var showHome = $('#ck-show-homepage-modal').prop('checked');

        $.ajax({
            type: "POST",
            url: "/Admin/ProductCategory/SaveEntity",
            data: {
                Id: id,
                Name: name,
                Description: description,
                ParentId: parentId,
                HomeOrder: homeOrder,
                SortOrder: order,
                HomeFlag: showHome,
                Image: image,
                Status: status,
                SeoPageTitle: seoPageTitle,
                SeoAlias: seoAlias,
                SeoKeywords: seoKeyword,
                SeoDescription: seoMetaDescription
            },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                tedu.notify('Update success ' + response.Name, 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                loadProductCategories();
            },
            error: function (status) {
                console.log("Has an error in updating progress: ", status);
                tedu.notify('Has an error in updating progress', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidden-id-modal').val(0);
        $('#txt-name-modal').val('');
        initTreeDropDownCategory('');

        $('#txt-description-modal').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txt-image-modal').val('');

        $('#txt-meta-keyword-modal').val('');
        $('#txt-meta-description-modal').val('');
        $('#txt-seo-page-title-modal').val('');
        $('#txt-seo-alias-modal').val('');

        $('#ck-status-modal').prop('checked', true);
        $('#ck-show-homepage-modal').prop('checked', false);

        initNumberSpinnerOrderDisplay();
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                var categoryId = $('#hidden-id-modal').val();
                $.each(response, function (_i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = tedu.unflattern(data);
                arr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                $('#ddl-category-id-modal').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddl-category-id-modal').combotree("clear", categoryId);
                    $('#ddl-category-id-modal').combotree("setValue", selectedId);
                }
            },
            error: function (status) {
                console.log("Has an error in loading product categories: ", status);
                tedu.notify('Has an error in loading product categories', 'error');
                tedu.stopLoading();
            }
        });
    }

    function initNumberSpinnerOrderDisplay() {
        $('#txtOrderM').numberspinner({
            min: 0,
            precision: 0,
            editable: true
        });
        $('#txtHomeOrderM').numberspinner({
            min: 0,
            precision: 0,
            editable: true
        });
    }
    

    function loadProductCategory() {
        var that = $('#hidden-id-modal').val();
        $.ajax({
            type: "GET",
            url: "/Admin/ProductCategory/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidden-id-modal').val(data.Id);
                $('#txt-name-modal').val(data.Name);
                initTreeDropDownCategory(data.ParentId);

                $('#txt-description-modal').val(data.Description);

                $('#txt-image-modal').val(data.Image);

                $('#txtSeoKeywordM').val(data.SeoKeywords);
                $('#txtSeoDescriptionM').val(data.SeoDescription);
                $('#txt-seo-page-title-modal').val(data.SeoPageTitle);
                $('#txt-seo-alias-modal').val(data.SeoAlias);

                $('#ck-status-modal').prop('checked', data.Status === 1);
                $('#ck-show-homepage-modal').prop('checked', data.HomeFlag);
                $('#txtOrderM').val(data.SortOrder);
                $('#txtHomeOrderM').val(data.HomeOrder);
                initNumberSpinnerOrderDisplay();
                $('#modal-add-edit').modal('show');
            },
            error: function (status) {
                console.log("Has an error in loading a product category: ", status);
                tedu.notify('Has an error in loading a product category', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function loadProductCategories() {
        $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = [];
                $.each(response, function (_i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var treeArr = tedu.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        $(this).tree('select', node.target);
                        $('#hidden-id-modal').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {

                        console.log(target);
                        console.log(source);
                        console.log(point);


                        var targetNode = $(this).tree("getNode", target);
                        if (point === "append") {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });
                            // Update to database                           
                            $.ajax({
                                url: '/Admin/ProductCategory/UpdateParentId',
                                type: 'POST',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function () {
                                    loadProductCategories();
                                }
                            });

                        } else if (point === "top" || point === "bottom") {
                            var parentNode = $(this).tree("getParent", target);
                            var items = [];
                            if (parentNode) {
                                $.each(parentNode.children, function (i, item) {
                                    items.push({
                                        key: item.id,
                                        value: i
                                    });
                                });
                            } else {
                                var rootNodes = $(this).tree("getRoots");
                                $.each(rootNodes, function (i, item) {
                                    items.push({
                                        key: item.id,
                                        value: i
                                    });
                                });
                            }

                            //Reorder the target product category
                            $.ajax({
                                url: '/Admin/ProductCategory/Reorder',
                                type: 'POST',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: items
                                },
                                success: function () {
                                    loadProductCategories();
                                }
                            });
                        }
                    }
                });
            },
            error: function (status) {
                console.log("Has an error in loading product categories: ", status);
                tedu.notify('Has an error in loading product categories', 'error');
                tedu.stopLoading();
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }
};