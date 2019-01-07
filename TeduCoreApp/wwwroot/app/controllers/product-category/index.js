
var productCategoryController = function () {
    this.initialize = function () {
        registerEvents();
        loadProductCategories();
    };

    function registerEvents() {

        $('#btnCreate').off('click').on('click', function (e) {
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
                var that = parseInt($('#hidIdM').val());
                deleteProductCategory(that);
            });
        });

        $('#btnSave').off('click').on('click', function (e) {
            e.preventDefault();
            var that = parseInt($('#hidIdM').val());
            saveProductCategory(that);
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
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
                    $('#txtImageM').val(path);
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
        var name = $('#txtNameM').val();
        var parentId = $('#ddlCategoryIdM').combotree('getValue');
        var description = $('#txtDescM').val();

        var image = $('#txtImageM').val();
        var order = parseInt($('#txtOrderM').val());
        var homeOrder = $('#txtHomeOrderM').val();

        var seoKeyword = $('#txtSeoKeywordM').val();
        var seoMetaDescription = $('#txtSeoDescriptionM').val();
        var seoPageTitle = $('#txtSeoPageTitleM').val();
        var seoAlias = $('#txtSeoAliasM').val();
        var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
        var showHome = $('#ckShowHomeM').prop('checked');

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
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');

        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);

        initNumberSpinnerOrderDisplay();
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                var categoryId = $('#hidIdM').val();
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
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree("clear", categoryId);
                    $('#ddlCategoryIdM').combotree("setValue", selectedId);
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
        var that = $('#hidIdM').val();
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
                $('#hidIdM').val(data.Id);
                $('#txtNameM').val(data.Name);
                initTreeDropDownCategory(data.ParentId);

                $('#txtDescM').val(data.Description);

                $('#txtImageM').val(data.Image);

                $('#txtSeoKeywordM').val(data.SeoKeywords);
                $('#txtSeoDescriptionM').val(data.SeoDescription);
                $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                $('#txtSeoAliasM').val(data.SeoAlias);

                $('#ckStatusM').prop('checked', data.Status === 1);
                $('#ckShowHomeM').prop('checked', data.HomeFlag);
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
                        $('#hidIdM').val(node.id);
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