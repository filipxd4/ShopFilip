﻿var eventTimeout;
$(document).ready(function () {
    GetPageData("", "", 1);
    eventThrottler();
    let url = document.location.href;
    let lastElementUrl = url.split("/")[4]
    $("#title").html(lastElementUrl)
});

$("#search-btn").click(function () {
    GetPageData(1);
    eventThrottler();
});

function GetPageData(pageNum, pageSize) {
    $("html, body").animate({ scrollTop: 0 }, 500);
    setTimeout(function () {
        let searchValue = $("#search").val();
        var sizes = [];

        $("#products").empty();
        $("#paged").empty();

        $.each($("input[name='size']:checked"), function () {
            sizes.push($(this).val());
        });

        let url = document.location.href;
        let groupOfProducts = url.split("/")[4]
        $.ajax({
            url: '/Product/GetPaggedData',
            data: { SearchValue: searchValue, Sizes: sizes, GroupOfProducts: groupOfProducts, gender, pageNumber: pageNum, pageSize: pageSize },
            traditional: true,
            success: function (response) {
                console.log(response)
                let rowData = "";
                for (var i = 0; i < response.data.length; i++) {
                    rowData = rowData + '<div class="col-md-4"> <div class="card rounded"> <div class="card-image" > <img class="lazy-img img-fluid" src="data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" data-src="' + response.data[i].photos[0].photoPath + '" alt="" /> </div> <div class="card-image-overlay m-auto"> <span class="card-detail-badge">' + response.data[i].price + ' zł</span> </div> <div class="card-body text-center"> <div class="ad-title m-auto"> <h5>' + response.data[i].name + '</h5> </div> <a href="/product/productInfo/' + response.data[i].id+'" class="btn buy">KUP TERAZ</a> </div> </div></div>';
                    rowData = rowData.replace("-siemasiema", response.data[i].id);
                }
                $("#products").append(rowData).fadeIn(400);
                PaggingTemplate(response.totalPages, response.currentPage);
            }
        });
    }, 600);
    eventThrottler();
}

function PaggingTemplate(totalPage, currentPage) {
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();

    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }

    template = "<p style='text-align:center;'>" + CurrentPage + "  z  " + TotalPages + " stron</p>"
    template = template + '<ul class="pager" style="list-style:none;overflow:hidden;margin: auto;">' +
        '<li class="input-li previous"><input type="button"   onclick="GetPageData(' + FirstPage + ')" id="filip" class="btn btn-default" style="margin-right:40px"value="Pierwsza strona"  /></li>' +
        '<li class="input-li previous" style="margin-top: 7px">Liczba produktów </li>' +
        '<li class="input-li" ><select ng-model="pageSize" id="selectedId" class="form-control" style="width:70px;"><option value="2" selected>2</option><option value="50">50</option><option value="100">100</option><option value="150">150</option></select> </li>' +
        '<li class="input-li" style="margin-top:7px"><a onclick="GetPageData(' + BackwardOne + ')"><i class="glyphicon glyphicon-backward"></i></a>';

    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<a class="page-number active" onclick="GetPageData(' + PageNumberArray[i] + ')" >' + PageNumberArray[i] + ' &nbsp;&nbsp;</a>'
    }
    template = template + numberingLoop + '<a onclick="GetPageData(' + ForwardOne + ')" ><i class="glyphicon glyphicon-forward"></i></a></li>' +
        '<li  class="next input-li" ><input type="button"  class="btn btn-default" onclick="GetPageData(' + LastPage + ')" style="margin-left:40px" value="Ostatnia strona"  ></li></ul>';
    $("#paged").append(template);
    $('#selectedId').change(function () {
        GetPageData(1, $(this).val());
    });
}

function lazyLoad() {
    $('.lazy-img').each(function () {
        $(this).attr('src', $(this).data('src'));
        $(this).removeClass('lazy-img');
    })
};

var eventThrottler = function () {
    if (!eventTimeout) {
        eventTimeout = setTimeout(function () {
            eventTimeout = null;
            lazyLoad();
        }, 1000);
    }
};
$(document).on('scroll', function () {
    eventThrottler();
});

var input = document.getElementById("search");
input.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        document.getElementById("search-btn").click();
    }
});