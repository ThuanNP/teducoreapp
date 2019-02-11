var productController = function () {
    var quantityManagement = new QuantityManagement();

    this.initialize = function () {
        loadCategories();
        loadProducts();
        registerEvents();
        registerControls();
        quantityManagement.initialize();
    };

    function registerEvents() {
        //Init validation
        $('#form-maintainance-modal').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                'txt-name-modal': { required: true },
                'ddl-category-id-modal': { required: true },
                'txt-price-modal': {
                    required: true,
                    number: true
                }
            }
        });

        //Todo: binding events to controls
        $('#ddl-show-page').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadProducts(true);
        });

        $('#btn-search').on('click', function () {
            loadProducts(true);
        });

        $('#txt-keyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadProducts(true);
            }
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadProduct(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();           
            var name = $(this).data('name');
            var that = $(this).data('id');
            deleteProduct(that, name);          
        });

        $('#btn-save').on('click', function (e) {
            e.preventDefault();
            if ($('#form-maintainance-modal').valid()) {
                var id = $('#hidd-product-id').val();
                saveProduct(id);
            }
        });

        $('#btn-select-image').on('click', function (e) {
            e.preventDefault();            
            $('#txt-input-image-file').click();
        });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal("show");
        });

        $("#txt-input-image-file").on('change', function (e) {
            e.preventDefault();
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

        $('#ddl-category-search').on('change', function (e) {
            e.preventDefault();
            loadProducts(true);
        });

        $('#btn-import').on('click', function (e) {
            e.preventDefault();
            initTreeDropDownCategory();
            $('#modal-import-excel').modal("show");
        });

        $('#btn-import-excel').on('click', function (e) {
            e.preventDefault();
            var fileUpload = $('#file-input-excel').get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();
            // Looping overakk files and add it ti FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object
            fileData.append("categoryId", $('#ddl-category-id-import-excel').combotree('getValue'));
            importExcel(fileData);
        });

        $("#btn-export").on('click', function (e) {
            e.preventDefault();           
            exportExcel();
        });


    }

    function registerControls() {
        CKEDITOR.replace('txtContentModal', {});
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };

        //Set tooltip for button
        tedu.tooltip("btn-template", "Download template");
        tedu.tooltip("btn-import", "Import products by file. You can download template by clicking button on the the left");
        tedu.tooltip("btn-export", "Export all products");
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
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
                var arr = tedu.unflattern(data);
              
                $('#ddl-category-id-modal').combotree({
                    data: arr,
                    required: true,
                    editable: false
                });
               
                $('#ddl-category-id-import-excel').combotree({
                    data: arr,
                    required: true,
                    editable: false
                });
                if (selectedId !== undefined || selectedId !== null) {
                    $('#ddl-category-id-modal').combotree('setValue', selectedId);
                }
            },
            error: function (status) {
                console.log("Has an error in loading product categories: ", status);
                tedu.notify('Has an error in loading product categories', 'error');               
            }
        });
    }

    function initNumberSpinnerPrice() {
        var groupSeparator = '.';
        var decimalSeparator = ',';

        $('#txt-price-modal').numberspinner({
            min: 0,
            precision: 0,
            groupSeparator: groupSeparator,
            decimalSeparator: decimalSeparator,
            editable: true
        });
        $('#txt-original-price-modal').numberspinner({
            min: 0,
            precision: 0,
            groupSeparator: groupSeparator,
            decimalSeparator: decimalSeparator,
            editable: true
        });
        $('#txt-promotion-price-modal').numberspinner({
            min: 0,
            precision: 0,
            groupSeparator: groupSeparator,
            decimalSeparator: decimalSeparator,
            editable: true
        });
    }

    function loadCategories() {
        $.ajax({
            type: 'GET',
            url: '/Admin/Product/GetAllCategories',
            dataType: 'json',
            success: function (response) {
                var render = '<option>-- All categories --</option>';
                $.each(response, function (_i, item) {
                    render += '<option value="' + item.Id + '">' + item.Name + '</option>';
                });
                if (render !== "") {
                    $('#ddl-category-search').html(render);
                }
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading product category data.', 'error');
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidd-product-id').val(0);
        $('#txt-name-modal').val('');
        initTreeDropDownCategory('');

        $('#txt-description-modal').val('');
        $('#txt-unit-modal').val('');

        $('#txt-price-modal').val('0');
        $('#txt-original-price-modal').val('');
        $('#txt-promotion-price-modal').val('');

        initNumberSpinnerPrice();

        $('#txt-image-modal').val('');

        $('#txt-tag-modal').val('');
        $('#txt-meta-keyword-modal').val('');
        $('#txt-meta-description-modal').val('');
        $('#txt-seo-page-title-modal').val('');
        $('#txt-seo-alias-modal').val('');

        CKEDITOR.instances.txtContentModal.setData('');

        $('#ck-status-modal').prop('checked', true);
        $('#ck-hot-modal').prop('checked', false);
        $('#ck-show-homepage-modal').prop('checked', false);

    }

    function loadProducts(isPageChanged) {       
        var template = $('#table-template').html();
        $.ajax({
            type: 'GET',
            data: {
                categoryId: $('#ddl-category-search').val(),
                keyword: $('#txt-keyword').val(),
                page: tedu.configs.pageIndex,
                pageSize: tedu.configs.pageSize
            },
            url: '/Admin/Product/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var render = "";
                $.each(response.Results, function (_i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: (item.Image === null ? '<img src="/admin-side/images/no-image-icon.png"' : '<img src="') + item.Image + '" width="25" />',
                        CategoryName: item.ProductCategory.Name,
                        Price: tedu.formatNumber(item.Price, 0),
                        CreatedDate: tedu.dateTimeFormatJson(item.DateCreated),
                        Status: tedu.getStatus(item.Status)
                    });
                });
                $('#lbl-total-records').text(response.RowCount);
                $('#tbl-content').html(render);
                //if (render !== "") {
                //    $('#tbl-content').html(render);
                //}
                pagination.wrapPaging(response.RowCount, function () {
                    loadProducts();
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

    function loadProduct(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidd-product-id').val(data.Id);
                $('#txt-name-modal').val(data.Name);
                initTreeDropDownCategory(data.CategoryId);

                $('#txt-description-modal').val(data.Description);
                $('#txt-unit-modal').val(data.Unit);

                $('#txt-price-modal').val(data.Price);
                $('#txt-original-price-modal').val(data.OriginalPrice);
                $('#txt-promotion-price-modal').val(data.PromotionPrice);
                initNumberSpinnerPrice();
                // $('#txt-image-modal').val(data.ThumbnailImage);

                $('#txt-tag-modal').val(data.Tags);
                $('#txt-meta-keyword-modal').val(data.SeoKeywords);
                $('#txt-meta-description-modal').val(data.SeoDescription);
                $('#txt-seo-page-title-modal').val(data.SeoPageTitle);
                $('#txt-seo-alias-modal').val(data.SeoAlias);

                CKEDITOR.instances.txtContentModal.setData(data.Content);
                $('#ck-status-modal').prop('checked', data.Status === 1);
                $('#ck-hot-modal').prop('checked', data.HotFlag);
                $('#ck-show-homepage-modal').prop('checked', data.HomeFlag);

                $('#modal-add-edit').modal('show');
            },
            error: function (status) {
                console.log("Has an error in loading a product: ", status);
                tedu.notify('Has an error in loading a product', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveProduct(id) {

        var name = $('#txt-name-modal').val();
        var categoryId = $('#ddl-category-id-modal').combotree('getValue');

        var description = $('#txt-description-modal').val();
        var unit = $('#txt-unit-modal').val();

        var price = $('#txt-price-modal').val();
        var originalPrice = $('#txt-original-price-modal').val();
        var promotionPrice = $('#txt-promotion-price-modal').val();

        var image = $('#txt-image-modal').val();

        var tags = $('#txt-tag-modal').val();
        var seoKeyword = $('#txt-meta-keyword-modal').val();
        var seoMetaDescription = $('#txt-meta-description-modal').val();
        var seoPageTitle = $('#txt-seo-page-title-modal').val();
        var seoAlias = $('#txt-seo-alias-modal').val();

        var content = CKEDITOR.instances.txtContentModal.getData();
        var status = $('#ck-status-modal').prop('checked') === true ? 1 : 0;
        var hot = $('#ck-hot-modal').prop('checked');
        var showHome = $('#ck-show-homepage-modal').prop('checked');

        var dateCreated = $(this).data('created-date');

        $.ajax({
            type: "POST",
            url: "/Admin/Product/SaveEntity",
            data: {
                Id: id,
                Name: name,
                CategoryId: categoryId,
                Image: image,
                Price: price,
                OriginalPrice: originalPrice,
                PromotionPrice: promotionPrice,
                Description: description,
                Content: content,
                HomeFlag: showHome,
                HotFlag: hot,
                Tags: tags,
                Unit: unit,
                Status: status,
                SeoPageTitle: seoPageTitle,
                SeoAlias: seoAlias,
                SeoKeywords: seoKeyword,
                SeoDescription: seoMetaDescription,
                DateCreated: dateCreated
            },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                tedu.notify("Update product successful " + response.Name, "success");
                $('#modal-add-edit').modal("hide");
                resetFormMaintainance();
                loadProducts(true);
            },
            error: function (status) {
                console.log("Has an error in save product progress", status);
                tedu.notify("Has an error in save product progress", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function deleteProduct(id, name) {
        tedu.confirm("Are you sure to delete product: " + name + "?", function () {
            $.ajax({
                type: "DELETE",
                url: "/Admin/Product/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function (response) {
                    tedu.notify("Delete product " + name, "success");
                    loadProducts(true);
                },
                error: function (status) {
                    console.log("Has an error in delete product progress", status);
                    tedu.notify("Has an error in delete product progress", "error");
                },
                complete: function () {
                    tedu.stopLoading();
                }
            });
        });        
    }

    function importExcel(fileData) {
        $.ajax({
            url: '/Admin/Product/ImportExcel',
            type: 'POST',
            data: fileData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (data) {
                $('#modal-import-excel').modal("hide");
                loadProducts();
                tedu.notify("you've imported products file: " + data, "success");
            },
            error: function (status) {
                console.log("Has an error in importing products file progress", status);
                tedu.notify("Has an error in importing products file progress", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function exportExcel() {
        $.ajax({
            type: "POST",
            data: {
                categoryId: $('#ddl-category-search').val(),
                keyword: $('#txt-keyword').val()
            },
            url: "/Admin/Product/ExportExcel",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                window.location.href = response;
            },
            error: function (status) {
                console.log("Has an error in exporting products file progress", status);
                tedu.notify("Has an error in exporting products file progress", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }
};