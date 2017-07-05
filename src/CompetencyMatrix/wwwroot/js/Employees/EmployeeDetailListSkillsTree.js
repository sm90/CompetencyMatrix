initEmployeesDetailListSkillTree =
    function () {
        $("#employeeDetailListSkillTree")
            .fancytree({
                extensions: ["dnd", "glyph", "table", "filter"],
                quicksearch: true,
                filter: {
                    autoApply: true, // Re-apply last filter if lazy data is loaded
                    counter: true, // Show a badge with number of matching child nodes near parent icons
                    fuzzy: false, // Match single characters in order, e.g. 'fb' will match 'FooBar'
                    hideExpandedCounter: true, // Hide counter badge, when parent is expanded
                    highlight: false, // Highlight matches by wrapping inside <mark> tags
                    mode: "hide" // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
                },
                dblclick: function (event, data) {
                    if (!allowEditEmployee) {
                        return;
                    }

                    if (data.node.data.skillId > 0)
                        employeeSkillEdit(null, true, data.node);
                },
                titlesTabbable: true,
                checkbox: true,
                glyph: glyph_opts,
                select: function (event, data) {

                    if (!allowEditEmployee) {
                        return;
                    }

                    var selected = data.tree.getSelectedNodes();
                    if (selected.length > 0) {
                        $('#deleteEmployeeSkillId').removeClass('disabled');
                    } else {
                        $('#deleteEmployeeSkillId').addClass('disabled');
                    }

                    if (selected.length != 1) {
                        $('#btnEditEmployeeSkillId').addClass('disabled');
                    }
                    else {
                        $('#btnEditEmployeeSkillId').removeClass('disabled');
                    }
                },
                activate: function (event, data) {
                    if (!allowEditEmployee) {
                        return;
                    }

                    if (data.node.data.skillId > 0) {
                        $("#detailsView")
                        .load("../Skill/GetSkillDetailsView",
                            { id: data.node.data.skillId, isEditable: false },
                            function (response, status, xhr) {
                                $('#subContainerDetailSkillId').height($("#containerDetailSkillId").height() - $('#headerContainerDetailSkillId').height());
                            });
                    } else {
                        $('#detailsView').empty();
                    }
                },
                table:
                    {
                        indentation: 20,      // indent 20px per node level
                        //nodeColumnIdx: 1,     // render the node title into the 2nd column
                        checkboxColumnIdx: 0  // render the checkboxes into the 1st column
                    },
                dnd: {
                    preventVoidMoves: false, // Prevent dropping nodes 'before self', etc.
                    preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                    autoExpandMS: 400,
                    draggable: {
                        scroll: false,
                        revert: "invalid"
                    },

                    dragStart: function (node, data) {
                        return false;
                    },
                    dragEnter: function (node, data) {
                        if (data.otherNode.folder == true)
                            return false;
                        return true;
                    },
                    dragDrop: function (node, data) {
                        if (!allowEditEmployee) {
                            return;
                        }
                        var allKeys = $.map($('#employeeDetailListSkillTree').fancytree('getRootNode').getChildren(), function (node) {
                            return node;
                        });
                        var alreadyExist = false;
                        var params = [ data.otherNode.data.model.id ];
                        $.each(allKeys, function (event, data, otherNode) {
                            if (data.data.skillId == params[0])
                                alreadyExist = true;
                        }, params);

                        if (alreadyExist)
                            showAlert("Alert", "Skill '" + data.otherNode.data.model.name + "' already exist. Please choose another one or edit existing");
                        else
                            employeeSkillEdit(data.otherNode.data.model.id, false);
                    }
                },

                postProcess: function (event, data) {
                    var orgResponse = data.response;
                    showEmptyLabel(data.response.length === 0);
                    data.result = parseEmployeeSkills(orgResponse);
                },

                renderColumns: function (event, data) {
                    var node = data.node,
                    $tdList = $(node.tr).find(">td");
                    var model = data.node.data;
                    $tdList.eq(1).html("<span class='col-lg-5'> " + htmlEncode(model.skillName) + "</span>");
                    $tdList.eq(2).html("<span class='col-lg-5'> " + htmlEncode(model.levelName) + "</span>");
                    $tdList.eq(3).html("<span class='col-lg-2'> " + model.lastUsedYear + "</span>");

                    if (data.node.data !== null && data.node.data.skillName === "Drop Here") {
                        $tdList.eq(0).find('span').removeClass("glyphicon");
                        $tdList.eq(0).find('span').removeClass("fancytree-checkbox");
                        $tdList.eq(0).find('span').removeClass("glyphicon-unchecked");
                    }
                }
            });
    };


function showEmptyLabel(show)
{
    if (show) {
        $('#lblNoSkills').show();
    }
    else {
        $('#lblNoSkills').hide();
    }
    
}

function employeeSkillEdit(skillId, isedit, node) {
    // skillId whene add

    $("#employeeAddSkillModalId").draggable({
        handle: ".modal-header"
    });

    var tree = $("#employeeDetailListSkillTree").fancytree("getTree");
    var nodes = tree.getSelectedNodes();
    if (isedit && nodes.length !== 1)
        return;

    if (isedit === false) {
        //add new skill
        var urlForLoadEditSkillForm = $('#employeeDetailListSkillTree').data('url') + '?employeeId=11&skillId=' + skillId + '&isEdit=false';
        $.get(urlForLoadEditSkillForm,
            function (data) {
                $('#modalContent').html(data);
                $('#modalTitle').text('Add skill');
                $('#employeeAddSkillModalId').modal('show');
            });
    } else {
        //var urlForLoadEditSkillForm = $('#employeeDetailListSkillTree').data('url') + '?employeeId=11&skillId=' + skillId + '&isEdit=true' + '&changeLogId=' + node.data.changeLogId;
        skillId = nodes[0].data.skillId;
        var urlForLoadEditSkillForm = $('#employeeDetailListSkillTree').data('url') + '?employeeId=11&skillId=' + skillId + '&isEdit=true';
        $.get(urlForLoadEditSkillForm,
            function (data) {
                $('#modalContent').html(data);
                $('#modalTitle').text('Edit skill');
                $('#employeeAddSkillModalId').modal('show');
            });
    }
}

function parseEmployeeSkills(skills) {
    var children = [];
    $.each(skills, function () {
        children.push(this);
    });
    return children;
}

function parseSkills(employee) {
    return {
        title: htmlEncode(employee.skillName),
        expanded: true,
        lazy: false,
        model: employee
    }
}
function selectEmployeesNode(employeesId) {
    $("#employeeDetailListSkillTree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (!node.folder && node.data.model !== undefined && node.data.model.id === employeesId) {
		        node.setActive();
		    }
		});
};

function expandAll() {
    $("#employeeDetailListSkillTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(true);
        });
}

function collapseAll() {
    $("#employeeDetailListSkillTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(false);
        });
}