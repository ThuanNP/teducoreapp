﻿var productController = function () {
    this.initialize = function () {
        loadCategories();
        loadProducts();       
        registerEvents();
    };

    function registerEvents() {
        //todo: Binding events to controls
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
    }

    function loadCategories() {
        $.ajax({
            type: 'GET',            
            url: '/Admin/Product/GetAllCategories',
            dataType: 'json',
            success: function (response) {
                var render = '<option>-- All categories --</option>';
                $.each(response, function (i, item) {
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
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Image: item.Image === null ? '<img src="/admin-side/images/no-image-icon.png"' : '<img src="' + item.Image + '" width="25" />',
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
            onPageClick: function (event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
};