var ImageManagement = function () {
    var self = this;
    var parent = parent;
    var images = [];

    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        $('body').on('click', '.btn-images', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidd-product-id').val(that);
            clearFileInput($("#file-image"));
            loadImages(that);
            $('#modal-image-management').modal('show');
        });

        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            $(this).closest('div').remove();
            var that = $(this).data('path');
            var index = images.indexOf(that);
            if (index > -1) {
                images.splice(index, 1);
            }
        });

        $("#file-image").on('change', function (e) {
            e.preventDefault();
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            uploadImages(data);
        });

        $('#btn-save-image').on('click', function (e) {
            e.preventDefault();
            var productId = $('#hidd-product-id').val();
            //var images = [];
            //$.each($('#image-list').find('img'), function (_i, _item) {
            //    images.push($(this).data('path'));
            //});
            saveImages(productId);
        });
    }

    function clearFileInput(ctrl) {
        //try {
        //    ctrl.value = null;
        //} catch (ex) { }
        //if (ctrl.value) {
        //    ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
        //}
    }

    function loadImages(productId) {
        $.ajax({
            url: '/admin/Product/GetImages',
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
                images = [];
                $.each(response, function (_i, item) {
                    images.push(item.Path);
                    render += '<div class="col-md-3"><img width="100" src="' + item.Path + '"  data-path="' + item.Path + '"><br/><a href="#" class="btn-delete-image" data-path="' + item.Path + '">Remove</a></div>';
                });
                $('#image-list').html(render);
                clearFileInput();
            },
            error: function (status) {
                console.log(status);
                tedu.notify('There was errors in loading images!', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function uploadImages(data) {
        $.ajax({
            type: "POST",
            url: "/Admin/Upload/UploadImage",
            contentType: false,
            processData: false,
            data: data,
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (path) {
                clearFileInput($("#file-image"));
                images.push(path);
                $('#image-list').append('<div class="col-md-3"><img width="100"  data-path="' + path + '" src="' + path + '"><br/><a href="#" class="btn-delete-image" data-path="' + path + '">Remove</a></div>');
                tedu.notify('Upload images successfully!', 'success');
            },
            error: function (status) {
                console.log(status);
                tedu.notify('There was errors in uploading images!', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }

    function saveImages(productId) {
        $.ajax({
            url: '/admin/Product/SaveImages',
            data: {
                productId: productId,
                images: images
            },
            type: 'post',
            dataType: 'json',
            beforeSend: function () {
                tedu.startLoading();
            },
            success: function (response) {
                $('#modal-image-management').modal('hide');
                $('#image-list').html('');
                clearFileInput($("#file-image"));
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot save product images data.', 'error');
            },
            complete: function () {
                tedu.stopLoading();
            }
        });
    }
};