var BaseController = function () {
    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        $('body').on('click', '.add-to-cart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            addToCart(id);
        });

        $('body').on('click', '.remove-cart', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            removeFromCart(id);
        });
    }

    function addToCart(id) {
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'post',
            data: {
                productId: id,
                quantity: 1,
                color: 0,
                size: 0
            },
            success: function () {
                tedu.notify('The product was added to cart', 'success');
                tedu.loadHeaderCart();
            },
            error: function (status) {
                console.log("Has an error in adding product to cart: ", status);
                tedu.notify('Has an error in adding product to cart', 'error');
            }
        });
    }

    function removeFromCart(id) {
        $.ajax({
            url: '/Cart/RemoveFromCart',
            type: 'post',
            data: {
                productId: id
            },
            success: function (response) {
                tedu.notify('The product was removed', 'success');
                tedu.loadHeaderCart();
            },
            error: function (status) {
                console.log("Has an error in removing product to cart: ", status);
                tedu.notify('Has an error in removing product to cart', 'error');
            }
        });
    }
};