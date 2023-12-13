var load = function (keyWord, pageIndex, pageSize) {
    $.ajax({
        url: "/Employee/Customer/GetPaging",
        data: {
            keyWord: keyWord,
            pageIndex: pageIndex,
            pageSize: pageSize
        },
        type: "GET",
        success: function (response) {
            var pageCurrent = response.pageCurrent;
            var toalPage = response.toalPage;

            var str = "";
            var info = `Trang ${pageCurrent} / ${toalPage}`;
            $("#selection-datatable_info").text(info);
            $.each(response.data, function (index, value) {
                str += "<tr>";
                str += "<td>" + value.MaKH + "</td>";
                str += "<td>" + value.Ten + "</td>";
                str += "<td class='banner'><img class='img-banner' src=" + value.Hinh + " /</td>";
                str += "<td>" + value.DiaChi + "</td>";
                str += "<td>" + value.SoDienThoai + "</td>";
                if (value.GioiTinh == 1) {
                    str += "<td>Nữ" + "</td>";
                }
                else {
                    str += "<td>Nam" + "</td>";
                }
                str += '<td class="text-center"><a class="btn btn-warning" href="/Employee/Customer/Edit/' + value.MaKH + '">Cập nhật</a>';                        
                str += "</tr>";

                //create pagination
                var pagination_string = "";

                if (pageCurrent > 1) {
                    var pagePrevious = pageCurrent - 1;
                    pagination_string += '<li class="paginate_button page-item previous"><a href="#" class="page-link" data-page="' + pagePrevious + '">‹</a></li>';
                }
                for (var i = 1; i <= toalPage; i++) {
                    if (i == pageCurrent) {
                        pagination_string += '<li class="paginate_button page-item active"><a class="page-link" href="#" data-page=' + i + '>' + i + '</a></li>';
                    } else {
                        pagination_string += '<li class="paginate_button page-item"><a href="#" class="page-link" data-page=' + i + '>' + i + '</a></li>';
                    }
                }
                //create button next
                if (pageCurrent > 0 && pageCurrent < toalPage) {
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
