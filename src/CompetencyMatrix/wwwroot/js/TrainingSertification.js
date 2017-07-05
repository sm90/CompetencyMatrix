


$(document).ready(function () {

    $("#editSertificationDialog").on('show.bs.modal', function (evt) {

        var url = urlUpdateSertivfication,
            id = evt.relatedTarget && evt.relatedTarget.mode === 'edit' ? evt.relatedTarget.id : 0;

        var data = {
            employeeId: $('#EmployeeId').val(),
            id: id
        };

        $('#editSertificationDialogTitle').text(id > 0 ? 'Edit Item' : 'New Item')

        $.ajax({
            url: url,
            type: "GET",
            data: data,
            success: function (result, status) {
                if (status != "error") {
                    $("#editSertificationDlgArea").html(result);
                }
            }
        });
    });
});

var initSertificationEditorTree = function () {
    $("#sertificatesList")
        .fancytree({
            extensions: ["glyph", "table"],
            titlesTabbable: true,
            checkbox: true,
            glyph: {
                checkbox: "glyphicon glyphicon-unchecked",
                checkboxSelected: "glyphicon glyphicon-check",
                checkboxUnknown: "glyphicon glyphicon-share"
            },

            source:
            {
                url: urlGetSertifications
            },
            checkbox:function(node){
                return true;
            },
            dblclick: function (event, data) {
                editSertification({ mode: 'edit', id: data.node.data.model.id });
            },
            table:
                {
                    indentation: 20,
                    checkboxColumnIdx: 0,
                    nodeColumnIdx: 0
                },

            activate: function (event, data) {
                $('#deleteSertification').removeClass('disabled');
            },

            postProcess: function (event, data) {

                var orgResponse = data.response;
                data.result = parseSertificationList(orgResponse);
            },

            renderColumns: function (event, data) {
                var node = data.node,
                $tdList = $(node.tr).find(">td");
                var model = data.node.data.model;
                $tdList.eq(1).html("<span> " + model.type + "</span>");
                $tdList.eq(2).html("<span> " + htmlEncode(model.name) + "</span>");
                $tdList.eq(3).html("<span> " + model.whenYear + "</span>");
            }
        });

    var tree = $("#sertificatesList").fancytree("getTree");

    $("#sertificatesList").tooltip({
        content: function () {
            return $(this).attr("title");
        }
    });

};


function parseSertificationList(data) {
    var children = [];
    $.each(data, function () {
        children.push(parseSertification(this));
    });
    useReverseData = false;
    return children;
}

function parseSertification(sertification) {
    return {
        title: sertification.name,
        type: sertification.type,
        year: sertification.whenYear,
        tooltip: sertification.name,
        lazy: false,
        model: sertification,
        icon: "glyphicon glyphicon-unchecked cursorover"
    }
}


function closeDlg() {
    closeModalDlg('trainingDlgContent');
}

function addSertification() {
    $('#editSertificationDialog').modal('show');
}

function editSertification(ctx) {
    $('#editSertificationDialog').modal('show', ctx);
}

function deleteSertification() {

    var tree = $("#sertificatesList").fancytree("getTree");
    var model = tree.activeNode.data.model;

    showConfirm("Delete Training or Sertification confirmation",
                String.format("Are you sure you want to delete {0} {1}?", model.type, model.name),
                function () {

                    $.ajax({
                        url: urlDeleteSertification,
                        type: "POST",
                        data: { id: model.id },
                        success: function (result) {
                            location.reload();
                        }
                    });
                });

}
