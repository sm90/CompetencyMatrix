var useReverseData = false;
var useInnerFilter = true;

initEmployeesEditorTree =
    function () {
        $("#employeestree")
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

                source:
                {
                    url: "../api/Employees/root"
                },

                dblclick: function (event, data) {
                    var model = data.node.data.model;
                    if (model === null || model === 'undefined')
                        return;

                    $.ajax({
                        url: '../../Employees/CheckOnAccessPersonalInformation',
                        type: "POST",
                        data: { id: model.id },
                        success: function (result, status) {
                            if (result.accessDenied !== false) {
                                window.open('../../Employees/GetEmployeeDetails/' + model.id, '_self', false);
                            } else {
                                showAlert('Information', 'Access denied.');
                            }
                        }
                    });
                },
                table:
                    {
                        checkboxColumnIdx: 0,
                        nodeColumnIdx: 1
                    },

                activate: function (event, data) {
                },

                lazyLoad: lazyLoadEmployees,

                postProcess: function (event, data) {
                    var orgResponse = data.response;
                    data.result = parseEmployeesList(orgResponse);
                },

                renderColumns: function (event, data) {
                    var node = data.node,
                    $tdList = $(node.tr).find(">td");
                    var model = data.node.data.model;
                    $tdList.eq(2).html("<span> " + ((model.office==null)  ? '' : model.office) + "</span>");
                    $tdList.eq(3).html("<span> " + model.title + "</span>");
                    $tdList.eq(4).html("<span> " + model.profileStatus + "</span>");
                    var node = data.node;

                    var $span = $(node.span);
                    $span.find("> span.fancytree-custom-icon").on('click', function () {
                        $('#employeeTree_txtSearch').val('manager=@<' + model.name + '>');
                        filterEmployee();
                        return;
                    });
                }
            });

        var tree = $("#employeestree").fancytree("getTree");

        $("#employeestree").tooltip({
            content: function () {
                return $(this).attr("title");
            }
        });


        $("#employeeTree_txtSearch")
            .keyup(function (e) {
                useInnerFilter = true;
                var opts = {
                        autoExpand: true,
                        leavesOnly: false
                    },
                    match = $(this).val();

                // Pass a string to perform case insensitive matching
                if (e && e.which === $.ui.keyCode.ENTER){
                    filterEmployee();
                }                    
            })
            .focus();



        $("#employeestree_clearSearch")
            .click(function (e) {
                $("#employeeTree_txtSearch").val("");
                tree.clearFilter();
                useInnerFilter = true;
                tree.reload({ url: "../api/Employees/root" })
        .done(function (event) {
        });
            });
    };

function selectEmployeesNode(EmployeesId) {
    $("#employeestree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (!node.folder && node.data.model != undefined && node.data.model.id == EmployeesId) {
		        node.setActive();
		    }
		});
};

function parseEmployeesList(employees) {
    var children = [];
    $.each(employees, function () {
        children.push(parseEmployee(this));
    });
    useReverseData = false;
    return children;
}

function parseEmployee(employee) {
    var lazy = false;
    var inverseParent = employee.inverseManagerNavigation;
    return {
        title: employee.name,
        expanded: employee.parentId == undefined,
        tooltip: employee.description,
        folder: true,
        lazy: lazy,
        model: employee,
        children: null, //((inverseParent !== undefined && inverseParent != null && inverseParent.length && level <= 2) ? parseEmployeesList(inverseParent) : []),
        cstrender: true,
        icon: inverseParent != null ? "glyphicon glyphicon-user icon-skill cursorover" : ''
    }
}

function filterEmployee() {
    var tree = $("#employeestree").fancytree("getTree");
    var search = $('#employeeTree_txtSearch').val();

    var officePresenter = -1;
    var positionPresenter = -1;
    var managerPresenter = -1;
    if (search != '') {
        officePresenter = search.toLowerCase().indexOf('office=@'.toLowerCase());
        positionPresenter = search.toLowerCase().indexOf('position=@'.toLowerCase());
        managerPresenter = search.toLowerCase().indexOf('manager=@'.toLowerCase());
    }

    if (managerPresenter > -1)
        filterEmployeeByManager(search);
    if (officePresenter > -1)
        filterEmployeeBy($('#officeActionId').val() + "?id=" + getEmployeeFilterParam(search));
    if (positionPresenter > -1)
        filterEmployeeBy($('#positionActionId').val() + "?id=" + getEmployeeFilterParam(search));

    if (officePresenter === -1 && positionPresenter === -1 && managerPresenter === -1){

        //filterEmployeeBy
        //var 
        //    opts = ,
        //var    match = $(this).val();

        tree.filterNodes(search, { autoExpand: true, leavesOnly: false });

    }
}

function filterEmployeeBy(url) {
    var tree = $("#employeestree").fancytree("getTree");
    useInnerFilter = false;
    tree.clearFilter();
    tree.reload({ url: url })
        .done(function (event) {
        });
}

function getEmployeeFilterParam(filter) {
    var search = $('#employeeTree_txtSearch').val();

    search = search.replace('>', '~').replace('<', '~');
    var params = search.split('~');
    return params[1];
}

function filterEmployeeByManager(search) {
    var tree = $("#employeestree").fancytree("getTree");
    var id = 0;
    var match = null;
    if (search.trim() != '') {
        tree.visit(function (node) {
            if (search.toLowerCase().indexOf(node.data.model.name.trim().toLowerCase()) >= 0) {
                match = node;
                return false; // stop traversal (if we are only interested in first match)
            }
        });
    }

    if (match !== null) {
        id = match.data.model.id;
    }
    useReverseData = id > 0 ? true : false;
    //url: "../api/Employees/root"
    var pathforLoadEmployeeTree = $('#employeestreeContent').data('url') + '?id=' + id;
    tree.reload({ url: pathforLoadEmployeeTree })
        .done(function (event) {

        });
}

function showEmployeeInfo() {
    var isShow = $('#searchHints>blockquote>p').css("display");
    var displayValue = isShow === "block" ? 'none' : 'block'

    $('#searchHints>blockquote>p').css("display", displayValue);
    $('#searchHints').parent().css("display", displayValue);
}

function expandAll() {
    $("#employeestree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(true);
        });
}

function collapseAll() {
    $("#employeestree")
        .fancytree("getRootNode")
        .visit(function (node) {
            node.setExpanded(false);
        });
}