PositionMatricesViewModel = {};

function SelectSelectableElements(selectableContainer, elementsToSelect) {
    // add unselecting class to all elements in the styleboard canvas except the ones to select
    $(".ui-selected", selectableContainer).not(elementsToSelect).removeClass("ui-selected").addClass("ui-unselecting");

    // add ui-selecting class to the elements to select
    $(elementsToSelect).not(".ui-selected").addClass("ui-selecting");

    // trigger the mouse stop event (this will select all .ui-selecting elements, and deselect all .ui-unselecting elements)
    var instance = selectableContainer.selectable("instance");
    instance._mouseStop(null);
}

function selectPositionMatrix(matrixId) {
    var $container = $("#positionMatrixList");
    var $matrixItem = $(".idHidden[value=" + matrixId + "]", $container).closest("li");

    SelectSelectableElements($container, $matrixItem);

    PositionMatricesViewModel.selectedMatrixId = matrixId;
}

positionMatrixEditDialogInit = function() {
    $("#positionMatrixEditDialog")
        .on('show.bs.modal',
            function() {

                var url = _PositionMatrix_Edit;

                var data =
                    {
                        positionMatrixId: PositionMatricesViewModel.selectedMatrixId
                    };

                $.ajax({
                        url: url,
                        type: "POST",
                        data: data,
                        success: function(result, status) {
                            if (status != "error") {
                                $("#positionMatrixEditArea").html(result);
                                changeModel();
                            }
                        }
                    });
            });
};

positionMatrixInheritanceManagementDialogInit = function () {
    $("#positionMatrixInheritanceManagemenDialog")
        .on('show.bs.modal',
            function () {

                var url = _PositionMatrix_InheritanceManagement;

                var data =
                    {
                        positionMatrixId: PositionMatricesViewModel.selectedMatrixId
                    };

                $.ajax({
                    url: url,
                    type: "POST",
                    data: data,
                    success: function (result, status) {
                        if (status != "error") {
                            $("#positionMatrixInheritanceManagemenDialog").html(result);
                            changeModel();
                        }
                    }
                });
            });
};

positionMatrixListInitSelectable = function() {
    $("#positionMatrixList")
        .selectable({
            filter: "li",
            unselected: function() {
                $(".active", this).each(function() {
                    $(this).removeClass('active');
                    $(this).find(".selectedHidden").val(false);
                });
            },
            selected: function() {
                $(".ui-selected", this).each(function() {
                    $(this).addClass('active');
                    $(this).find(".selectedHidden").val(true);

                    var positionMatrixId = $(this).find(".idHidden").val();
                    PositionMatricesViewModel.selectedMatrixId = positionMatrixId;


                    var $positionMatrixSkillsView = $("#positionMatrixSkillsView");
                    $positionMatrixSkillsView.load(urlGetSkillsUi, { positionMatrixId: positionMatrixId });


                    var $positionMatrixDetailsView = $("#positionMatrixDetailsView");
                    $positionMatrixDetailsView.load(urlGetDetailsUi, { positionMatrixId: positionMatrixId });
                });
            }
        });
};

function editMatrix() {
    window.open(_PositionMatrix_Edit + '/' + PositionMatricesViewModel.selectedMatrixId, '_self', false);
}

function exportMatrix() {
    var matrixId = $('div#positionMatrixDetailsView #Id').val();
    var urlForExportMatrix = $('#exportMatrixId').data('url') + '/matrixPosition' + matrixId + '.csv' + '?matrixPositionId=' + matrixId;
    window.open(urlForExportMatrix);
}

function exportMatrixToPdf() {
    $('#positionMatrixListContent').parent().addClass('noprint');
    $('#positionMatrixDetailsView').parent().addClass('noprint');
    $('.navbar').addClass('noprint');
    $('#layoutFooter').addClass('noprint');

    $("#skillTreePositionMatrixSkills").fancytree("getRootNode").visit(function (node) {
        node.setExpanded(true);
    });

    window.print();

    $("#skillTreePositionMatrixSkills").fancytree("getRootNode").visit(function (node) {
        if (node.parent.isRootNode())
            node.setExpanded(true);
        else
            node.setExpanded(false);
    });
}

function deleteMatrix() {
    var isParent = $('div#positionMatrixDetailsView #HasChildren').val();
    var childrenList = $('div#positionMatrixDetailsView #ChildrenPlainList').val();

    if (isParent === 'True') {
        var matrixName = $('div#positionMatrixDetailsView #Name').val();
        showAlert("Can't Delete Position Matrix",
            "You can't delete matrix \""+matrixName+"\" because it is parent for the following matrix(es): " + childrenList);
    } else {
        var confirmationMessage = "Are you sure to delete position matrix?";

        showConfirm("Delete position matrix confirmation",
            confirmationMessage,
            function() {
                var matrixId = $('div#positionMatrixDetailsView #Id').val();
                $.ajax({
                    url: urlDelete,
                    type: "POST",
                    data: { positionMatrixId: matrixId },
                    success: function(result) {
                        location.href = '/Home/PositionMatrices/';
                    }
                });
            });
    }

}

function OnCreatePositioMatrixSuccess(data) {
    if (!data.createdSuccessfully) {
        $("#positionMatrixCreateModal").html(data);
    }
    if (data.createdSuccessfully && data.createdSuccessfully == true) {
        $('#positionMatrixCreateModal').modal('hide');
        location.href = '/Home/PositionMatrices/' + data.id;
    }
}

function OnCreatePositioMatrixFailure(xhr, ajaxOptions, thrownError) {
    $('#positionMatrixCreateModal').modal('hide');
}

