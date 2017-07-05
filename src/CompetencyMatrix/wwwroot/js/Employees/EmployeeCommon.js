function approveEmployee(id) {

    var url = '/Employees/ApproveEmployee';

    postEmployeeAction(url, id, function (result, status) {
        openEmployeeDetails(id);
    });

}

function openEmployeeDetails(id) {
    location.href = '/Employees/GetEmployeeDetails/' + id;
}

function rejectEmployee(id, url) {
    var url = '/Employees/RejectEmployee';
    postEmployeeAction(url, id, function (result, status) {
        openEmployeeDetails(id);
    });
}

function postEmployeeAction(url, id, success){
    $.ajax({
        url: url,
        type: "POST",
        data: { id: id },
        success: success
    });
}