var billController = function () {
    var cachedObj = {
        products: [],
        colors: [],
        sizes: [],
        paymentMethods: [],
        billStatuses: []
    };
    this.initialize = function () {
        $.when(
            loadBillStatuses(),
            loadPaymentMethods(),
            loadColors(),
            loadSizes(),
            loadProducts()
        ).done(function () {
            loadOrders(true);
        });
        registerControls();
        registerEvents();
    };

    function registerControls() {
        // Make datetime picker
        $('#txt-from-date, #txt-to-date').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy'
        });
    }

    function registerEvents() {
        // Init validation
        $('#frm-order-details-modal').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtCustomerName: { required: true },
                txtCustomerAddress: { required: true },
                txtCustomerMobile: { required: true },
                txtCustomerMessage: { required: true },
                ddlBillStatus: { required: true }
            }
        });

        //Todo: binding events to controls
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadOrders();
            }
        });
        $("#btn-search").on('click', function () {
            loadOrders();
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-details').modal('show');
        });

        $("#ddl-show-page").on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            loadOrders(true);
        });

        $('#btn-add-detail').on('click', function (e) {
            e.preventDefault();
            var template = $('#template-table-order-details').html();
            var products = getProductOptions(null);
            var colors = getColorOptions(null);
            var sizes = getSizeOptions(null);
            var render = Mustache.render(template,
                {
                    Id: 0,
                    Products: products,
                    Colors: colors,
                    Sizes: sizes,
                    Quantity: 0,
                    Total: 0
                });
            $('#tbl-order-details tbody').append(render);
        });

        $('body').on('click', '.btn-delete-detail', function (e) {
            e.preventDefault();
            $(this).parent().parent().remove();
        });

        $('#btn-save').on('click', function (e) {
            if ($('#frm-order-details-modal').valid()) {
                e.preventDefault();         
                var id = $('#hidd-order-id').val();
                saveOrder(id);
            }
        });

        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadOrder(that);
        });
    }

    function resetFormMaintainance() {
        $('#hidd-order-id').val(0);
        $('#txt-customer-name').val('');
        $('#txt-customer-address').val('');
        $('#txt-customer-mobile').val('');
        $('#txt-customer-message').val('');
        $('#ddl-payment-method').val('');
        $('#ddl-customer-id').val('');
        $('#ddl-bill-status').val('');
        //$('#tbl-order-details').html('');
    }

    function loadBillStatuses() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetBillStatuses",
            dataType: "json",
            success: function (response) {
                cachedObj.billStatuses = response;
                var render = "";
                $.each(response, function (_i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddl-bill-status').html(render);
            },
            error: function (status) {
                console.log("There is an error in loading bill statuses progress", status);
                tedu.notify('There is an error in loading bill statuses progress', 'error');
            }
        });
    }

    function loadPaymentMethods() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetPaymentMethods",
            dataType: "json",
            success: function (response) {
                cachedObj.paymentMethods = response;
                var render = "";
                $.each(response, function (_i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddl-payment-method').html(render);
            },
            error: function (status) {
                console.log("There is an error in loading payment methods progress", status);
                tedu.notify('There is an error in loading payment methods progress', 'error');
            }
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

    function loadProducts() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAll",
            dataType: "json",
            success: function (response) {
                cachedObj.products = response;
            },
            error: function (status) {
                console.log("There is an error in loading products progress", status);
                tedu.notify('There is an error in loading products progress', 'error');
            }
        });
    }

    function getPaymentMethodName(paymentMethod) {
        var method = $.grep(cachedObj.paymentMethods, function (element, _index) {
            return element.Value === paymentMethod;
        });
        if (method.length > 0) {
            return method[0].Name;
        } else {
            return '';
        }
    }

    function getBillStatusName(billStatus) {
        var status = $.grep(cachedObj.billStatuses, function (element, _index) {
            return element.Value === billStatus;
        });
        if (status.length > 0) {
            return status[0].Name;
        } else {
            return '';
        }
    }

    function loadOrders(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetAllPaging",
            data: {
                startDate: $('#txt-from-date').val(),
                endDate: $('#txt-to-date').val(),
                keyword: $('#txt-search-keyword').val(),
                page: tedu.configs.pageIndex,
                pageSize: tedu.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                $.each(response.Results, function (_i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        CustomerName: item.CustomerName,
                        PaymentMethod: getPaymentMethodName(item.PaymentMethod),
                        OrderDate: tedu.dateTimeFormatJson(item.OrderDate),
                        BillStatus: getBillStatusName(item.BillStatus)
                    });
                });
                $("#lbl-total-records").text(response.RowCount);
                if (render !== undefined) {
                    $('#tbl-content').html(render);
                }
                pagination.wrapPaging(response.RowCount, function () {
                    loadOrders();
                }, isPageChanged);
            },
            error: function (status) {
                console.log("There is an error in loading orders progress", status);
                tedu.notify("There is an error in loading orders progress", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function loadOrder(_id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetById",
            data: { id: _id },
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidd-order-id').val(data.Id);
                $('#txt-customer-name').val(data.CustomerName);
                $('#txt-customer-address').val(data.CustomerAddress);
                $('#txt-customer-mobile').val(data.CustomerMobile);
                $('#txt-customer-message').val(data.CustomerMessage);
                $('#ddl-payment-method').val(data.PaymentMethod);
                $('#ddl-customer-id').val(data.CustomerId);
                $('#ddl-bill-status').val(data.BillStatus);

                var billDetails = data.BillDetails;
                if (data.BillDetails !== null && data.BillDetails.length > 0) {
                    var render = '';
                    var templateDetails = $('#template-table-order-details').html();

                    $.each(billDetails, function (_i, item) {
                        var products = getProductOptions(item.ProductId);
                        var colors = getColorOptions(item.ColorId);
                        var sizes = getSizeOptions(item.SizeId);

                        render += Mustache.render(templateDetails,
                            {
                                Id: item.Id,
                                Products: products,
                                Colors: colors,
                                Sizes: sizes,
                                Quantity: item.Quantity
                            });
                    });
                    $('#tbl-order-details tbody').html(render);
                }
                $('#modal-details').modal('show');   
            },
            error: function (status) {
                console.log("There is an error in loading an order progress", status);
                tedu.notify("There is an error in loading an order progress", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveOrder(orderId) {
        // Bill        
        var customerName = $('#txt-customer-name').val();
        var customerAddress = $('#txt-customer-address').val();
        var customerId = $('#ddlcustomerid').val();
        var customerMobile = $('#txt-customer-mobile').val();
        var customerMessage = $('#txt-customer-message').val();
        var paymentMethod = $('#ddl-payment-method').val();
        var billStatus = $('#ddl-bill-status').val();
        if (billStatus === undefined || billStatus === null) {
            billStatus = 1;
        }

        //bill details
        var billDetails = [];
        $.each($('#tbl-order-details>tbody>tr'), function (_i, item) {
            billDetails.push({
                Id: $(item).data('id'),
                ProductId: $(item).find('select.ddl-product-id').first().val(),
                Quantity: $(item).find('input.txt-quantity').first().val(),
                ColorId: $(item).find('select.ddl-color-id').first().val(),
                SizeId: $(item).find('select.ddl-size-id').first().val(),
                BillId: orderId
            });
        });
        $.ajax({
            type: "POST",
            url: "/Admin/Bill/SaveEntity",
            data: {
                Id: orderId,
                BillStatus: billStatus,
                CustomerAddress: customerAddress,
                CustomerId: customerId,
                CustomerMessage: customerMessage,
                CustomerMobile: customerMobile,
                CustomerName: customerName,
                PaymentMethod: paymentMethod,
                Status: billStatus,
                BillDetails: billDetails
            },
            dataType: "json",
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                tedu.notify('Save order successful', 'success');
                $('#modal-details').modal('hide');
                resetFormMaintainance();
                loadOrders(true);
            },
            error: function (status) {
                console.log("There is an error in saving order progress", status);
                tedu.notify("There is an error in saving order progress", "error");
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    // Function create order detail table
    function getProductOptions(selectedId) {
        var products = "<select class='form-control ddl-product-id'>";
        $.each(cachedObj.products, function (_i, product) {
            if (selectedId === product.Id)
                products += '<option value="' + product.Id + '" selected="select">' + product.Name + '</option>';
            else
                products += '<option value="' + product.Id + '">' + product.Name + '</option>';
        });
        products += "</select>";
        return products;
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

};