initEmployeesChangeLogTree =
    function () {
        $("#employeeChangeLogTree")
            .fancytree({
                extensions: ["glyph", "table", "filter"],

                quicksearch: true,

                filter: {
                    autoApply: true, // Re-apply last filter if lazy data is loaded
                    counter: true, // Show a badge with number of matching child nodes near parent icons
                    fuzzy: false, // Match single characters in order, e.g. 'fb' will match 'FooBar'
                    hideExpandedCounter: true, // Hide counter badge, when parent is expanded
                    highlight: false, // Highlight matches by wrapping inside <mark> tags
                    mode: "hide" // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
                },
                titlesTabbable: true,
                checkbox: false,
                glyph: glyph_opts,
                dblclick: function (event, data) {
                    var model = data.node.data.model;
                    if (model === null || model === 'undefined')
                        return;
                },

                table:
                    {
                        indentation: 20,      // indent 20px per node level
                        nodeColumnIdx: 0,     // render the node title into the 2nd column
                        //checkboxColumnIdx: 0  // render the checkboxes into the 1st column
                    },

                activate: function (event, data) {
                },

                //lazyLoad: lazyLoadEmployeeSkills,

                postProcess: function (event, data) {
                    var orgResponse = data.response;
                    data.result = parseChangeLog(orgResponse);
                },

                renderColumns: function (event, data) {
                    var node = data.node,
                    $tdList = $(node.tr).find(">td");
                    var model = data.node.data.model;
                    $tdList.eq(0).html("<span> " + model.lastChangedString + "</span>");
                    $tdList.eq(1).html("<span> " + model.actionDescription + "</span>");
                    $tdList.eq(2).html("<span> " + model.status + "</span>");
                }
            });

        var tree = $("#employeeChangeLogTree").fancytree("getTree");

        $("#employeeChangeLogTree").tooltip({
            content: function () {
                return $(this).attr("title");
            }
        });


        $("#changeLogSearch")
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

        $("#changeLogTree_clearSearch")
            .click(function (e) {
                $("#changeLogTree_txtSearch").val("");
                tree.clearFilter();
            });
    };



function parseChangeLog(logs) {
    var children = [];
    $.each(logs, function () {
        children.push(parseLog(this));
    });
    return children;
}

function parseLog(log) {
    var lazy = false;
    return {
        title: log.lastChangedString,
        expanded: true,
        lazy: lazy,
        model: log
    }
}
function selectChangeLogNode(changeLogId) {
    $("#employeeChangeLogTree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (!node.folder && node.data.model != undefined && node.data.model.id == EmployeesId) {
		        node.setActive();
		    }
		});
};

function expandAll() {
    $("#employeeChangeLogTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(true);
        });
}

function collapseAll() {
    $("#employeeChangeLogTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(false);
        });
}