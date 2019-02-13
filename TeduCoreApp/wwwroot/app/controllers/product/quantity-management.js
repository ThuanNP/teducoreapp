var QuantityManagement = function () {
    var self = this;
    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.initialize = function () {
        loadColors();
        loadSizes();
        registerEvents();
    };

    function registerEvents() {
        $('body').on('click', '.btn-quantity', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadQuantities(that);      
            $('#modal-quantity-management').modal('show');
        });

        $('#btn-add-quantity').on('click', function (e) {
            e.preventDefault();
            var template = $('#template-table-quantity').html();
            var render = Mustache.render(template, {
                Id: 0,
                Colors: getColorOptions(null),
                Sizes: getSizeOptions(null),
                Quantity: 1
            });            
            $('#table-quantity-content').append(render);
        });

        $("#btn-save-quantity").on('click', function (e) {
            e.preventDefault();
            var productId = $("#hidd-product-id").val();
            var quantities = [];
            $.each($('#table-quantity-content').find('tr'), function (_i, item) {
                quantities.push({
                    Id: $(item).data('id'),
                    ProductId: productId,
                    Quantity: $(item).find('input.txt-quantity').first().val(),
                    SizeId: $(item).find('select.ddl-size-id').first().val(),
                    ColorId: $(item).find('select.ddl-color-id').first().val()
                });
            });
            saveQuantities(productId, quantities);
        });

        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function (status) {
                console.log("There is an error in loading colors progress", status);
                tedu.notify('There is an error in loading colors progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function (status) {
                console.log("There is an error in loading sizes progress", status);
                tedu.notify('There is an error in loading sizes progress', 'error');
            }
        });
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddl-color-id'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option style="color:' + color.Code + '" value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option style="color:' + color.Code + '" value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddl-size-id'>";
        $.each(cachedObj.sizes, function (_i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function loadQuantities(productId) {
        $.ajax({
            url: '/admin/Product/GetQuantities',
            data: {
                productId: productId
            },
            type: 'get',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var render = '';
                var template = $('#template-table-quantity').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Colors: getColorOptions(item.ColorId),
                        Sizes: getSizeOptions(item.SizeId),
                        Quantity: item.Quantity
                    });
                });
                $('#table-quantity-content').html(render);
                $("#hidd-product-id").val(productId);           
                // resetFile();
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading product quantities data.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveQuantities(productId, quantities) {
        $.ajax({
            url: '/admin/Product/SaveQuantities',
            data: {
                productId: productId,
                quantityViewModels: quantities
            },
            type: 'post',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                $('#modal-quantity-management').modal('hide');
                $('#table-quantity-content').html('');
                tedu.notify("Save product quantities data successfully.", "success");
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot save product quantities data.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }
};