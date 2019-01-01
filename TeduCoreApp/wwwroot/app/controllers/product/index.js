var productController = function () {
    this.initialize = function () {
        loadCategories();
        loadProducts();
        registerEvents();
    };

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtNameM: { required: true },
                ddlCategoryIdM: { required: true },
                txtPriceM: {
                    required: true,
                    number: true
                }
            }
        });
        //Todo: binding events to controls
        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadProducts(true);
        });

        $('#btnSearch').on('click', function () {
            loadProducts(true);
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadProducts(true);
            }
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadProduct(that);
        });

        $('#btnSave').on('click', function (e) {
            e.preventDefault();
            if ($('#frmMaintainance').valid()) {
                var id = $('#hidIdM').val();
                saveProduct(id);
            }
        });
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
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            },
            error: function (status) {
                console.log("Has an error in loading product categories: ", status);
                tedu.notify('Has an error in loading product categories', 'error');
                tedu.stopLoading();
            }
        });
    }

    function initNumberSpinnerPrice() {
        var groupSeparator = '.';
        var decimalSeparator = ',';

        $('#txtPriceM').numberspinner({
            min: 0,
            precision: 0,
            groupSeparator: groupSeparator,
            decimalSeparator: decimalSeparator,
            editable: true
        });
        $('#txtOriginalPriceM').numberspinner({
            min: 0,
            precision: 0,
            groupSeparator: groupSeparator,
            decimalSeparator: decimalSeparator,
            editable: true
        });
        $('#txtPromotionPriceM').numberspinner({
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
                    $('#ddlCategorySearch').html(render);
                }
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading product category data.', 'error');
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtUnitM').val('');

        $('#txtPriceM').val('0');
        $('#txtOriginalPriceM').val('');
        $('#txtPromotionPriceM').val('');

        initNumberSpinnerPrice();

        //$('#txtImageM').val('');

        $('#txtTagM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        //CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);

    }

    function loadProducts(isPageChanged) {
        var template = $('#table-template').html();
        $.ajax({
            type: 'GET',
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: tedu.configs.pageIndex,
                pageSize: tedu.configs.pageSize
            },
            url: '/Admin/Product/GetAllPaging',
            dataType: 'json',
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
                $('#lblTotalRecords').text(response.RowCount);
                $('#tbl-content').html(render);
                //if (render !== "") {
                //    $('#tbl-content').html(render);
                //}
                wrapPaging(response.RowCount, function () {
                    loadProducts();
                }, isPageChanged);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading product data.', 'error');
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
                $('#hidIdM').val(data.Id);
                $('#txtNameM').val(data.Name);
                initTreeDropDownCategory(data.CategoryId);

                $('#txtDescM').val(data.Description);
                $('#txtUnitM').val(data.Unit);

                $('#txtPriceM').val(data.Price);
                $('#txtOriginalPriceM').val(data.OriginalPrice);
                $('#txtPromotionPriceM').val(data.PromotionPrice);
                initNumberSpinnerPrice();
                // $('#txtImageM').val(data.ThumbnailImage);

                $('#txtTagM').val(data.Tags);
                $('#txtMetakeywordM').val(data.SeoKeywords);
                $('#txtMetaDescriptionM').val(data.SeoDescription);
                $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                $('#txtSeoAliasM').val(data.SeoAlias);

                //CKEDITOR.instances.txtContentM.setData(data.Content);
                $('#ckStatusM').prop('checked', data.Status === 1);
                $('#ckHotM').prop('checked', data.HotFlag);
                $('#ckShowHomeM').prop('checked', data.HomeFlag);

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

        var name = $('#txtNameM').val();
        var categoryId = $('#ddlCategoryIdM').combotree('getValue');

        var description = $('#txtDescM').val();
        var unit = $('#txtUnitM').val();

        var price = $('#txtPriceM').val();
        var originalPrice = $('#txtOriginalPriceM').val();
        var promotionPrice = $('#txtPromotionPriceM').val();

        //var image = $('#txtImageM').val();

        var tags = $('#txtTagM').val();
        var seoKeyword = $('#txtMetakeywordM').val();
        var seoMetaDescription = $('#txtMetaDescriptionM').val();
        var seoPageTitle = $('#txtSeoPageTitleM').val();
        var seoAlias = $('#txtSeoAliasM').val();

        //var content = CKEDITOR.instances.txtContentM.getData();
        var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
        var hot = $('#ckHotM').prop('checked');
        var showHome = $('#ckShowHomeM').prop('checked');

        var dateCreated = $(this).data('created-date');
        $.ajax({
            type: "POST",
            url: "/Admin/Product/SaveEntity",
            data: {
                Id: id,
                Name: name,
                CategoryId: categoryId,
                Image: '',
                Price: price,
                OriginalPrice: originalPrice,
                PromotionPrice: promotionPrice,
                Description: description,
                Content: '',
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

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / tedu.configs.pageSize);
        // Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        // Bind pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (_event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
};