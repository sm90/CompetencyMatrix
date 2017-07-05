initEmployeesPastProjectTree =
    function () {
        $("#employeePastProjectTree")
            .fancytree({
                extensions: ["glyph", "table", "filter"],
                selectMode: 2,
                quicksearch: true,

                filter: {
                    autoApply: true, // Re-apply last filter if lazy data is loaded
                    counter: true, // Show a badge with number of matching child nodes near parent icons
                    fuzzy: false, // Match single characters in order, e.g. 'fb' will match 'FooBar'
                    hideExpandedCounter: true, // Hide counter badge, when parent is expanded
                    highlight: false, // Highlight matches by wrapping inside <mark> tags
                    mode: "hide", // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
                    autoActivate: false
                },
                titlesTabbable: true,
                checkbox: true,
                glyph: glyph_opts,

                dblclick: function (event, data) {
                    var model = data.node.data.model;
                    if (model === null || model === 'undefined')
                        return;
                },

                table:
                    {
                        indentation: 20,      // indent 20px per node level
                        //nodeColumnIdx: 0,     // render the node title into the 2nd column
                        checkboxColumnIdx: 0  // render the checkboxes into the 1st column
                    },

                activate: function (event, data) {
                    var selected = data.tree.getSelectedNodes();
                    if (selected.length != 1)
                        $('#btnEditPastProject').html('Add past project');
                    else
                        $('#btnEditPastProject').html('Edit past project');
                },
                select: function (event, data) {
                    var selected = data.tree.getSelectedNodes();

                    if (selected.length != 1)
                        $('#btnEditPastProject').html('Add past project');
                    else {
                        $('#btnEditPastProject').html('Edit past project');
                    }

                },

                postProcess: function (event, data) {
                    var orgResponse = data.response;
                    data.result = parseEmployeePastProject(orgResponse);
                },

                renderColumns: function (event, data) {
                    var node = data.node,
                    $tdList = $(node.tr).find(">td");

                    var model = data.node.data;
                    $tdList.eq(1).html("<span > " + model.companyName + "</span>");
                    $tdList.eq(2).html("<span> " + model.workPeriod + "</span>");
                    $tdList.eq(3).html("<span style='width:200px;'> " + model.project + "</span>");
                    //$tdList.eq(4).html("<span class='navbar-link' style='width:200px;' data-toggle='tooltip' href='#' title='" + model.descriptionTooltip.replace(/\<br>/g, ' ') + "'> " + model.projectDescription + "</span>");
                    $tdList.eq(4).html("<span class='navbar-link' style='width:200px;' href='#'>" + model.projectDescription + "</span>");
                    $tdList.eq(5).html("<span style='vertical-align:middle; text-align: center'> " + model.role + "</span>");
                    $tdList.eq(6).html("<span> " + model.technologies + "</span>");
                    $tdList.eq(7).html("<span> " + model.tools + "</span>");
                    $tdList.eq(8).html("<span> " + model.team + "</span>");

                    $tdList.eq(0).css("vertical-align", "middle");
                    $tdList.eq(1).css("vertical-align", "middle");
                    $tdList.eq(2).css("vertical-align", "middle");
                    $tdList.eq(3).css("vertical-align", "middle");
                    $tdList.eq(4).css("vertical-align", "middle");
                    $tdList.eq(4).attr('data-toggle', 'tooltip').attr('title', model.descriptionTooltip==null?'': model.descriptionTooltip.replace(/\<br>/g, ' '));

                    $tdList.eq(5).css("vertical-align", "middle");
                    $tdList.eq(6).css("vertical-align", "middle");
                    $tdList.eq(7).css("vertical-align", "middle");
                    $tdList.eq(8).css("vertical-align", "middle");

                }
            });


        var tree = $("#employeePastProjectTree").fancytree("getTree");


        $("#employeePastProjectTree").tooltip({
            content: function () {
                return $(this).attr("title");
            }
        });
        $("#employeeSearch")
            .keyup(function (e) {
                var n,
                    opts = {
                        autoExpand: true,
                        leavesOnly: false
                    },
                    match = $(this).val();
                if (e && e.which === $.ui.keyCode.ENTER && $.trim(match) === "") {
                    return;
                }
                if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
                    return;
                }
                // Pass a string to perform case insensitive matching
                n = tree.filterNodes(match, opts);
            })
            .focus();

        $("#employeestree_clearSearch")
            .click(function (e) {
                $("#employeestree_txtSearch").val("");
                tree.clearFilter();
            });
    };



function parseEmployeePastProject(projects) {
    var children = [];
    $.each(projects, function () {
        children.push(this);
    });
    return children;
}

function parsePastProject(project) {
    var lazy = false;
    return {
        title: project.companyName,
        expanded: true,
        lazy: lazy,
        model: project
    }
}
function selectEmployeesNode(EmployeesId) {
    $("#employeePastProjectTree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (!node.folder && node.data.model != undefined && node.data.model.id == EmployeesId) {
		    }
		});
};

function expandAll() {
    $("#employeePastProjectTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(true);
        });
}

function collapseAll() {
    $("#employeePastProjectTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(false);
        });
}