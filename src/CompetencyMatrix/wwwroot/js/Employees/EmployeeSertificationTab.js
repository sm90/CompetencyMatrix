function initSertificationTab() {


    var data = {
        employeeId: $("#employeeStoredId").val()
    };

    $.ajax({
        url: listSertificatesUrl,
        type: "GET",
        data: data,
        success: function (result, status) {

            if (status != "error") {

                var table = $("#trainingSertificationTbl").find('tbody');

                for (var i = 0; i < result.length; i++) {
                    var item = result[i];

                    table.append($('<tr>')
                                            .append($('<td>').append(item.type))
                                            .append($('<td>').append(htmlEncode(item.name)))
                                            .append($('<td>').append((new Date(item.when)).getFullYear())));
                }

            }
        }
    });

}