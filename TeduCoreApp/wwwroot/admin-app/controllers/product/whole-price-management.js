var WholePriceManagement = function (parent) {
    var self = this;
    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        $('body').on('click', '.btn-whole-price', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidd-product-id').val(that);
            loadWholePrices(that);
            $('#modal-whole-price').modal('show');
        });

        $('#btn-add-whole-price').on('click', function (e) {
            e.preventDefault();
            var template = $('#template-table-whole-price').html();
            var render = Mustache.render(template, {
                Id: 0,
                FromQuantity: 10,
                ToQuantity: 100,
                Price: 0
            });
            $('#table-content-whole-price').append(render);
        });

        $('#btn-save-whole-price').on('click', function (e) {
            e.preventDefault();
            var productId = $('#hidd-product-id').val();
            var prices = [];
            $.each($('#table-content-whole-price').find('tr'), function (i, item) {
                prices.push({
                    Id: $(item).data('id'),
                    ProductId: $('#hidId').val(),
                    FromQuantity: $(item).find('input.txt-quantity-from').first().val(),
                    ToQuantity: $(item).find('input.txt-quantity-to').first().val(),
                    Price: $(item).find('input.txt-whole-price').first().val(),
                });
            });
            saveWholePrice(productId, prices);
        });

        $('body').on('click', '.btn-delete-whole-price', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
    }

    function loadWholePrices(productId) {
        $.ajax({
            url: '/admin/Product/GetWholePrices',
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
                var template = $('#template-table-whole-price').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        FromQuantity: item.FromQuantity,
                        ToQuantity: item.ToQuantity,
                        Price: item.Price
                    });
                });
                $('#table-content-whole-price').html(render);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot load product whole prices.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveWholePrice(productId, prices) {
        $.ajax({
            url: '/admin/Product/SaveWholePrice',
            data: {
                productId: productId,
                wholePrices: prices
            },
            type: 'post',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                $('#modal-whole-price').modal('hide');
                $('#table-content-whole-price').html('');
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot save product whole prices.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }
};