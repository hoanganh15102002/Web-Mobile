var load = function (keyWord, pageIndex, pageSize) {
    $.ajax({
        url: "/Order/GetPaging",
        data: {
            keyWord: keyWord,
            pageIndex: pageIndex,
            pageSize: pageSize
        },
        type: "GET",
        success: function (response) {
            var pageCurrent = response.pageCurrent;
            var totalPage = response.totalPage;

            var str = "";
            var info = `Trang ${pageCurrent} / ${totalPage}`;
            $("#selection-datatable_info").text(info);
            $.each(response.data, function (index, value) {
                str += "<tr>";
                str += "<td>" + value.OrderID + "</td>";
                str += "<td>" + value.Ngay + "</td>";
                str += "<td>" + value.TongTien + "</td>";
                if (value.Status == 1) {
                    str += "<td>Đặt hàng thàng công</td>";
                }
                else if (value.Status == 2) {
                    str += "<td>Đang duyệt</td>";
                }
                else if (value.Status == 3) {
                    str += "<td>Đã duyệt</td>";
                }
                else if (value.Status == 4) {
                    str += "<td>Đang giao </td>";
                }
                else if (value.Status == 5) {
                    str += "<td>Đã giao</td>";
                }
                else if (value.Status == 6) {
                    str += "<td>Hoàn tất</td>";
                }
                else {
                    str += "<td>Đã hủy</td>";
                }
                str += '<td class="d-flex justify-content-around"><a class="btn btn-info text-white" href="/Order/Details/' + value.OrderID + '">Xem chi tiết</a>';
                if (value.Status != 7 && value.Status != 6) {
                    str += '<a class="btn btn-warning text-white" href="#" data-user=' + value.OrderID + '>Sửa đơn hàng</a></td>';
                }
                str += "</tr>";

                //create pagination
                var pagination_string = "";

                if (pageCurrent > 1) {
                    var pagePrevious = pageCurrent - 1;
                    pagination_string += '<li class="paginate_button page-item previous"><a href="#" class="page-link" data-page="' + pagePrevious + '">‹</a></li>';
                }
                for (var i = 1; i <= totalPage; i++) {
                    if (i == pageCurrent) {
                        pagination_string += '<li class="paginate_button page-item active"><a class="page-link" href="#" data-page=' + i + '>' + i + '</a></li>';
                    } else {
                        pagination_string += '<li class="paginate_button page-item"><a href="#" class="page-link" data-page=' + i + '>' + i + '</a></li>';
                    }
                }
                //create button next
                if (pageCurrent > 0 && pageCurrent < totalPage) {
                    var pageNext = pageCurrent + 1;
                    pagination_string += '<li class="paginate_button page-item next"><a href="#" class="page-link" data-page=' + pageNext + '>›</a></li>';
                }
                $("#load-pagination").html(pagination_string);
            });
            //load str to class="load-list"
            $("#datatablesSimple > tbody").html(str);
        }
    });
}

$("body").on("click", "#datatablesSimple a.btn.btn-warning", function (event) {
    event.preventDefault();
    var cate_edit = $(this).attr('data-user');
    if (confirm("Bạn có muốn hủy đơn hàng có id = " + cate_edit + " này không?")) {
        $.ajax({
            url: "/Order/Edit",
            type: "POST",
            data: { id: cate_edit },
            dataType: "json",
            success: (result) => {
                location.reload();
            }
        });
    }
});