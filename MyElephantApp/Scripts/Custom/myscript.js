
var uri = '/api/products';

function formatItem(item) {
    return item.Name + ': $' + item.Price;
}

function find() {
    var id = $('#prodId').val();
    $.getJSON(uri + '/' + id)
        .done(function (data) {
            $('#product').text(formatItem(data));
        })
        .fail(function (jqXHR, textStatus, err) {
            $('#product').text('Error: ' + err);
        });
}

$(document).ready(function () {
    $(document).ready(function () {

        $.getJSON(uri, function () {
            console.log("success");
        })
            .done(function (data) {
                $.each(data, function (key, item) {
                    // Add a list item for the product.
                    $('<li>', { text: formatItem(item) }).appendTo($('#products'));
                });
            })
            .fail(function (data) {
                console.log("error");
            })
            .always(function () {
                console.log("complete");
            });
    });
});

