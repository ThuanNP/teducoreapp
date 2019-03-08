var CartController = function () {
    var cachedObj = {
        colors: [],
        sizes: []
    };
    this.initialize = function () {
        $.when(loadColors(), loadSizes()).then(function () {
            loadCart();
        });
        registerEvents();
    };

    function registerEvents() {
        $('body').on('change', '.txt-quantity', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var qty = $(this).val();
            var size = $(this).closest('tr').find('.ddl-sizes').first().val();
            var color = $(this).closest('tr').find('.ddl-colors').first().val();
            if (qty > 0) {
                updateCart(id, qty, color, size);
            } else {
                qty = $(this).data('quantity');
                $(this).val(qty);
                tedu.notify('Your quantity is invalid', 'warn');
            }
        });

        $('body').on('change', '.ddl-colors', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var color = $(this).val();
            var size = $(this).closest('tr').find('.ddl-sizes').first().val();
            var qty = $(this).closest('tr').find('.txt-quantity').first().val();
            if (qty > 0) {
                updateCart(id, qty, color, size);
            } else {
                tedu.notify('Your color is invalid', 'warn');
            }
        });

        $('body').on('change', '.ddl-sizes', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var color = $(this).closest('tr').find('.ddl-colors').first().val();
            var size = $(this).val();
            var qty = $(this).closest('tr').find('.txt-quantity').first().val();
            if (qty > 0) {
                updateCart(id, qty, color, size);
            } else {
                tedu.notify('Your color is invalid', 'warn');
            }
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            removeCartItem(id);
        });

        $('#btn-clear-all').on('click', function (e) {
            e.preventDefault();
            clearCart();
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                tedu.notify('Has an error in colours loading progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                tedu.notify('Has an error in sizes loading progress', 'error');
            }
        });
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control input-sm ddl-colors'>";
        $.each(cachedObj.colors, function (_i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control input-sm ddl-sizes'>";
        $.each(cachedObj.sizes, function (_i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function getQuantityOptions(id, qty) {
        var quantity = "<input class='form-control input-sm txt-quantity' type='number' data-quantity='" + qty + "' value='" + qty + "'></td>";
        return quantity;
    }

    function loadCart() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (_i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: tedu.formatNumber(item.Price, 0),
                            Quantity: getQuantityOptions(item.Product.Id, item.Quantity),
                            Colors: getColorOptions(item.Color === null ? 0 : item.Color.Id),
                            Sizes: getSizeOptions(item.Size === null ? "" : item.Size.Id),
                            Amount: tedu.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lbl-total-amount').text(tedu.formatNumber(totalAmount, 0));
                if (render !== "") 
                    $('#table-cart-content').html(render);                
                else
                    $('#table-cart-content').html('You have no product in cart');
            }
        });
        return false;
    }

    function updateCart(id, qty, colorId, sizeId) {
        $.ajax({
            url: '/Cart/UpdateCart',
            type: 'post',
            data: {
                productId: id,
                quantity: qty,
                color: colorId,
                size: sizeId
            },
            success: function () {
                tedu.notify('Update quantity is successful', 'success');
                //loadHeaderCart();
                loadCart();
            }
        });
    }

    function removeCartItem(id) {
        $.ajax({
            url: '/Cart/RemoveFromCart',
            type: 'post',
            data: {
                productId: id
            },
            success: function () {
                tedu.notify('Removing product is successful.', 'success');
                //loadHeaderCart();
                loadCart();
            }
        });
    }

    function clearCart() {
        $.ajax({
            url: '/Cart/ClearCart',
            type: 'post',
            success: function () {
                tedu.notify('Clear cart is successful', 'success');
                // loadHeaderCart();
                loadCart();
            }
        });
    }
};