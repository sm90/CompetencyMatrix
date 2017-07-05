initEmployeesSkillTree =
    function () {
        $("#employeeSkillTree")
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
                        checkboxColumnIdx: 0  // render the checkboxes into the 1st column
                    },

                activate: function (event, data) {
                },

                lazyLoad: lazyLoadEmployeeSkills,

                postProcess: function (event, data) {
                    var orgResponse = data.response;
                    data.result = parseEmployeeSkills(orgResponse);
                },

                renderColumns: function (event, data) {
                    var node = data.node,
                    $tdList = $(node.tr).find(">td");
                    var model = data.node.data;
                    $tdList.eq(0).html("<span class='col-lg-4'> " + htmlEncode(model.skillName) + "</span>");
                    $tdList.eq(1).html("<span class='col-lg-4'> " + htmlEncode(model.levelName) + "</span>");
                    $tdList.eq(2).html("<span class='col-lg-4'> " + model.lastUsedYear + "</span>");
                }
            });

        var tree = $("#employeeSkillTree").fancytree("getTree");

        $("#employeeSkillTree").tooltip({
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



function parseEmployeeSkills(skills) {
    var children = [];
    $.each(skills, function () {
        children.push(this);
    });
    return children;
}

function parseSkills(employee) {
    var lazy = false;
    return {
        title: htmlEncode(employee.skillName),
        expanded: true,
        lazy: lazy,
        model: employee
    }
}
function selectEmployeesNode(EmployeesId) {
    $("#employeeSkillTree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (!node.folder && node.data.model != undefined && node.data.model.id == EmployeesId) {
		        node.setActive();
		    }
		});
};

function expandAll() {
    $("#employeeSkillTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(true);
        });
}

function collapseAll() {
    $("#employeeSkillTree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(false);
        });
}