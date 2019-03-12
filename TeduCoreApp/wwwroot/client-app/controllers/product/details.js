var ProductDetailsController = function () {
    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        $('#btn-add-products-to-cart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            var colorId = parseInt($('#ddl-color-id').val());
            var sizeId = parseInt($('#ddl-size-id').val());
            var quantity = parseInt($('#txt-quantity').val());
            addCartItem(id, sizeId, colorId, quantity);
        });
    }

    function addCartItem(id, sizeId, colorId, quantity) {
        $.ajax({
            type: "POST",
            url: "/Cart/AddToCart",
            data: {
                productId: id,
                quantity: quantity,
                color: colorId,
                size: sizeId
            },
            dataType: "json",
            success: function (response) {
                tedu.loadHeaderCart();
                tedu.notify('Add cart item successfully', 'success');
            },
            error: function (status) {
                console.log("Has an error in add cart item progress", status);
                tedu.notify("Has an error in add cart item progress", "error");
            }
        });
    }
};