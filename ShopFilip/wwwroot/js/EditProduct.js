$("#submit-form").click(function () {
    let group = $("input[name='group']:checked").val();
    let gender = $("input[name='gender']:checked").val();
    let size = $("input[name='size']:checked").map(function () { return $(this).val(); }).get();
    let table = document.getElementById("table-photo-add").value;
    let name = $("#name").val()
    let price = $("#price").val()
    let description = $("#description").val().replace(/\n/g, "<br />")
    let quantity = $("input[name='quantity']").map(function () { return $(this).val(); }).get()
    let photos = $("img[name='photo-of-product']").map(function () { return $(this).attr("src"); }).get()
    let id = $("#idOfProoduct").val();

    var myData = {
        Id: id,
        Name: name,
        Price: price,
        Group: group,
        Size: size,
        Quantity: quantity,
        Description: description,
        Gender: gender,
        Table: table,
        Photos: photos,
    }

    $.ajax({
        url: '/Admin/Edit',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(myData),
        success: function (response) {
            if (response.success) {
                console.log("siemano");
                $('#modal-set-center').modal('show');
            }
        },
    });
});