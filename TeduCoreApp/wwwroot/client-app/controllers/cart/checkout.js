var CheckoutController = function () {
    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        //$('body').on('change', '#shipping-methods-ddl', function (e) {
        //    e.preventDefault();
        //    var shippmentCost = parseFloat($(this).val());
        //    var total = parseFloat($('#lbl-total-amount').data('total'));
        //    $('#lbl-shipping-cost').html(tedu.formatNumber(shippmentCost, 0));
        //    $('#lbl-total-amount').html(tedu.formatNumber(shippmentCost + total, 0));
        //    //tedu.notify('Your quantity is invalid', 'warn');
        //});
    }
};