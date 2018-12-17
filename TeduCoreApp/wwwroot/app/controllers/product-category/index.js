var productCategoryController = function () {
    this.initialize = function () {
        loadProductCategory();
    };

    function loadProductCategory() {
        $.ajax({
            type: 'GET',
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var treeArr = tedu.unflattern(data);
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onDrop: function (target, source, point) {
                        var targetNode
                    }
                });
            }
        });
    }
}