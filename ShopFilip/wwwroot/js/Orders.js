$('.status').on('change', function (e) {

    selectedValue = $(this).find('option:selected').text();
    orderId = $(this).closest('tr').find('td:eq(0)').find('#orderId').text();
    GetPageData(selectedValue, orderId)
});

function GetPageData(seletedValue, orderId) {
    $.ajax({
        type: "post",
        url: "/Admin/UpdateDataOrders?orderId=" + orderId + "&orderStatus=" + seletedValue + "",
        contentType: "html",
        success: function (result) {
            console.log("sucess")
        }
    });
}