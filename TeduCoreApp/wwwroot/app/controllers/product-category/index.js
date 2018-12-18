
var productCategoryController = function () {
    this.initialize = function () {
        loadProductCategories();
    };

    function loadProductCategories() {
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
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onDrop: function (target, source, point) {       

                        console.log(target);
                        console.log(source);
                        console.log(point);

                        var targetNode = $(this).tree("getNode", target);
                        var children = [];
                        $.each(targetNode.children, function (_i, item) {
                            children.push({
                                key: item.id,
                                value: _i
                            });
                        });

                        if (point === "append") {   
                            // Update to database
                            $.ajax({
                                url: '/Admin/ProductCategory/UpdateParentId',
                                type: 'PUT',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function () {
                                    loadProductCategories();
                                }
                            });

                        } else if (point === "top" || point === "bottom") {
                            //Reorder the target product category
                            $.ajax({
                                url: '/Admin/ProductCategory/Reorder',
                                type: 'PUT',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function () {
                                    loadProductCategories();
                                }
                            });

                        }
                    }
                });
            }
        });
    }
}