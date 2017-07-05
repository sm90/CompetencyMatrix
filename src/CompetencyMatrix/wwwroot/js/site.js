$(document).on({
    ajaxStart: function () {
        $("#LOADING").css("display", "block");
    },
    ajaxStop: function () {
        $("#LOADING").css("display", "none");
    },
    ajaxError: function (event, jqxhr, settings, thrownError) {
        if (jqxhr.status == 440) {
            $('#loginDialog').modal('show');
            return;
        }

        var alertDiv = $("#ajaxErrorAlert");
        var alert = $("#errorAlertTemplate").clone();
        alert.attr("id", "ajaxErrorAlertInstance");

        alertDiv.html(alert);

        $("#ajaxErrorAlertInstance > #ajaxErrorAlertUrl").html(settings.url);
        $("#ajaxErrorAlertInstance > #ajaxErrorAlertStatusText").html(jqxhr.statusText);

        alert.css("display", "block");
        alertDiv.css("display", "block");
    }

});
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover();
    $(".datefield").datepicker();

});

if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

function showAlert(caption, message, error, onbtnOk) {
    $("#alertModal").modal("show");
    $("#alertModal .modal-dialog .modal-content .modal-body>p").html(message);
    $("#alertModal .modal-dialog .modal-content .modal-header>h4").html(caption);
    if (onbtnOk)
        $("#btnOk").off("click").click({ param1: null, param2: null }, onbtnOk);
}

function hideAlert() {
    $("#alertModal").modal("hide");
    $("#alertModal .modal-dialog .modal-content .modal-body>p").html('');
    $("#alertModal .modal-dialog .modal-content .modal-header>h4").html('');
}

function showConfirm(caption, message, onBtnPressYes) {
    $("#alertConfirm").modal("show");
    if (onBtnPressYes !== 'undefined' && onBtnPressYes !== null)
        $("#yesButtonId").off("click").click({ param1: null, param2: null }, onBtnPressYes);
    $("#alertConfirm .modal-dialog .modal-content .modal-body>p").html(message);
    $("#alertConfirm .modal-dialog .modal-content .modal-header>h4").html(caption);

}

function hideConfirm() {
    $("#alertConfirm").modal("hide");
    $("#alertConfirm .modal-dialog .modal-content .modal-body>p").html('');
    $("#alertConfirm .modal-dialog .modal-content .modal-header>h4").html('');
}

$.fn.editable.defaults.mode = 'inline';

jQuery.fn.selectorExists = function () { return this.length > 0; }



$("*").ajaxComplete(function (e, xhr, settings) {
    IsLogonRequired(xhr.responseText);
});

function IsLogonRequired(response_data) {
    var data = null;
    try {
        var data = $.parseJSON(response_data);
    }
    catch (ex) {
        //content is not json so we dont care that jsonParse failed
    };

    if (data != null && data.LogonRequired != undefined && data.LogonRequired == true)
        windowlocation.href = "/Account/Login";

};

function getLayoutFooter() {
    return $('#layoutFooter');
}

function onCreateTicket() {
    window.open('https://jira.intetics.com/browse/AR0111/?selectedTab=com.atlassian.jira.jira-projects-plugin:issues-panel', '_blank');
    return false;
}

function closeModalDlg(containerId) {
    $('#' + containerId).closest('.modal.fade').modal('hide');
}

var CompetencyMatrix = {};

CompetencyMatrix.EntityState =
{
    Unchanged: 1,
    Deleted: 2,
    Modified: 3,
    Added: 4
}


CompetencyMatrix.EmployeeProfileStatus =
{
    NoTransaction: 0,
    Open: 1,
    Submitted: 2,
    Approved: 3,
    Cancelled: 4
}


function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}