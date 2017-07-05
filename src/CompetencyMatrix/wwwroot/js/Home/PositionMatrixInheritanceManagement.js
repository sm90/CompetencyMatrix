var PositionMatrixInheritanceManagement = {
    urlSetParentMatrixes: "", // must be setup in razor view
    initButtons: function() {
        var me = this;
        $("button.btn-management")
            .prop('disabled', true)
            .click(
                function() {
                    $(this).prop('disabled', true);
                });
        $("#addParentPositionMatrix")
            .click(function() {
                me.moveListItem('#matrixesCanBeAddedToParent', '#parentMatrixes');
            });
        $("#removeParentPositionMatrix")
            .click(function() {
                me.moveListItem('#parentMatrixes', '#matrixesCanBeAddedToParent');
            });
        $("#updateParentMatrixes")
            .click(function() {
                $.ajax({
                    url: me.urlSetParentMatrixes,
                    type: "POST",
                    data: {
                        CurrentMatrix: {
                            Id: $("#CurrentMatrix_Id").val(),
                            Name: $("#CurrentMatrix_Name").val()
                        },
                        ParentMatrixes: me.getMatrixList("#parentMatrixes > li"),
                        MatrixesCanBeAddedToParent: me.getMatrixList("#matrixesCanBeAddedToParent > li")
                    },
                    success: function (data, status, settings) {
                        if (data === "") location.reload();
                        else {
                            $("#positionMatrixInheritanceManagemenDialog").html(data);
                        }

                    }
                });
            });
    },

    moveListItem: function(srcListSelector, dstListSelector) {
        $(dstListSelector).append($(srcListSelector + ' .ui-selected.active').removeClass('ui-selected active'));
    },

    initListSelectable: function(listId, buttonId) {
        $(listId)
            .selectable({
                filter: "li",
                unselected: function() {
                    $(".active", this)
                        .each(function() {
                            $(this).removeClass('active');
                            $(this).find(".selectedHidden").val(false);
                        });
                },
                selected: function() {
                    $(".ui-selected", this)
                        .each(function() {
                            $(this).addClass('active');
                            $(this).find(".selectedHidden").val(true);
                            $(buttonId).prop('disabled', false);
                        });
                }
            });
    },

    init: function() {
        this.initButtons();
        this.initListSelectable("#matrixesCanBeAddedToParent", "#addParentPositionMatrix");
        this.initListSelectable("#parentMatrixes", "#removeParentPositionMatrix");
    },

    getMatrixList: function(selector) {
        var ids = [];
        $(selector)
            .each(function () {
                var parentMatrix = {
                    Id: $(this).find(".idHidden").val(),
                    Name: $(this).find(".nameHidden").val()
                };
                ids.push(parentMatrix);
            });
        return ids;
    }


};

$(function() {
    PositionMatrixInheritanceManagement.init();
});