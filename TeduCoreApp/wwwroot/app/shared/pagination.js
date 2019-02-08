var pagination = {
    wrapPaging: function (recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / tedu.configs.pageSize);
        // Unbind pagination if it existed or click change pagesize
        if ($('#pagination-url a').length === 0 || changePageSize === true) {
            $('#pagination-url').empty();
            $('#pagination-url').removeData("twbs-pagination");
            $('#pagination-url').unbind("page");
        }
        // Bind pagination Event
        if (totalSize > 0) {
            $('#pagination-url').twbsPagination({
                totalPages: totalSize,
                visiblePages: 7,
                first: 'Đầu',
                prev: 'Trước',
                next: 'Tiếp',
                last: 'Cuối',
                onPageClick: function (_event, p) {
                    tedu.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }        
    }
};