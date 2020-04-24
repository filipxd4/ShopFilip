$("#add-table").click(function () {
    GetTables()
});

$("#button-add-table").click(function () {
    var a = $('input[name="test"]:checked').val();
    document.getElementById('table-photo-add').defaultValue = a;
    $("#table-added").html("");
    $("#table-added").append("<p>Dodano tabele: </p>" + a)
});

$('body').on('click', '#x', function () {
    $(this).closest('div').remove();
    let removePhoto = $(this).closest('div').children().eq(1).attr("src")
    $.ajax({
        url: "/Admin/RemoveFromDatabase",
        type: "POST",
        data: { 'path': removePhoto },
        dataType: 'text',
        success: function () {
            if (response.success) {
                $("#getCode").html(resp);
                $("#getCodeModal").modal('show');
            }
        },
    });
});

$('.size-container').change(function () {
    if (this.checked) {
        $(this).nextAll('input').first().show();
    }
    else {
        $(this).nextAll('input').first().hide();
    }
});

$("#gallery-photo-add").change(function () {
    let files = document.getElementById('gallery-photo-add').files;
    let data = new FormData();

    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    $.ajax({
        url: "/Admin/AddNewPhotos",
        type: "POST",
        data: data,
        processData: false,
        contentType: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                ImagesPreview(response[i], 'div.gallery');
            }
        },
    });
});

$("#add-new-table").change(function () {
    let fileUpload = $("#add-new-table").get(0);
    let files = fileUpload.files;
    let data = new FormData();

    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }

    $.ajax({
        url: "/Admin/AddNewTable",
        type: "POST",
        data: data,
        processData: false,
        contentType: false,
        success: function () {

        },
    });
    GetTables();
});

function ImagesPreview(input, placeToInsertImagePreview) {
    $(placeToInsertImagePreview).append("<div id='photo'><input id='x' type='button' value='X'><img name='photo-of-product' width='100px' id='photo-min-product' src='" + input + "'></div>");
}

$(function () {
    var imagesPreview = function (input, placeToInsertImagePreview) {
        if (input.files) {
            let filesAmount = input.files.length;
            for (i = 0; i < filesAmount; i++) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    $(placeToInsertImagePreview).append("<div id='photo'><input id='x' type='button' value='X'><img name='zdj' width='100px' id='photo-min-product' src='" + event.target.result + "'></div>");
                }
                reader.readAsDataURL(input.files[i]);
            }
        }
    };

    $('#table-photo-add').on('change', function () {
        imagesPreview(this, 'div.table');
    });
});

function GetTables() {
    $('div#photos').empty();
    $.getJSON("/Admin/GetAllTables", function (response) {
        for (var i = 0; i < response.length; i++) {
            var res = response[i].replace("C:\\Users\\Filip\\source\\repos\\Shop\\OnlineShop\\ShopFilip\\wwwroot", "");
            $('div#photos').append('<label> <input type="radio" id="radio" name="test" value="' + res + '"  > <img  height="100px" src="' + res + '"></label>')
        }
    })
}