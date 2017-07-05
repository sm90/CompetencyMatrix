initEmployeeDetailSkillTree =
    function () {
        $("#employeeDetailSkillTree")
            .fancytree({
                extensions: ["dnd", "edit", "glyph", "table", "filter"],
                quicksearch: true,
                filter: {
                    autoApply: true, // Re-apply last filter if lazy data is loaded
                    counter: true, // Show a badge with number of matching child nodes near parent icons
                    fuzzy: false, // Match single characters in order, e.g. 'fb' will match 'FooBar'
                    hideExpandedCounter: true, // Hide counter badge, when parent is expanded
                    highlight: false, // Highlight matches by wrapping inside <mark> tags
                    mode: "hide" // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
                },

                checkbox: false,
                autoScroll: true,
                dnd:
                {
                    preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                    preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                    autoExpandMS: 400,
                    draggable: {
                        appendTo: "body",
                        scroll: false,
                        revert: "invalid"
                    },
                    dragStart: function (node, data) {
                        if (data.isFolder == true)
                            return false;
                        
                        if (data.originalEvent.shiftKey) {
                            console.log("dragStart with SHIFT");
                        }
                        // allow dragging `node`:
                        return true;
                    },
                    dragEnter: function (node, data) {
                        
                        return true;
                    },
                    dragDrop: function (node, data) {
                        return false;
                    }
                },

                glyph: glyph_opts,

                source:
                    {
                        url: "../api/Employees/employeeDetailSkillRoot"
                    },

                table:
                    {
                        checkboxColumnIdx: 0,
                        nodeColumnIdx: 1
                    },
                lazyLoad: lazyLoadCategory,

                postProcess: function (event, data) {
                    var orgResponse = data.response;
                    data.result = parseSkillCategoryList(orgResponse);
                },

                renderColumns: function (event, data) {
                    var node = data.node,
                        $tdList = $(node.tr).find(">td");

                    if (node.data.model == undefined) {
                        $tdList.eq(2).html("<span class='badge'>new</span>");
                    }
                    else {
                        $tdList.eq(2).html("");
                    }
                }
            });

        var tree = $("#employeeDetailSkillTree").fancytree("getTree");

        $("#skillTree_txtSearch")
            .keyup(function (e) {
                var n,
                    opts = {
                        autoExpand: true,
                        leavesOnly: false
                    },
                    match = $(this).val();

                if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
                    $("button#skillTree_clearSearch").click();
                    return;
                }
                // Pass a string to perform case insensitive matching
                n = tree.filterNodes(match, opts);
            })
            .focus();

        $("#skillTree_clearSearch")
            .click(function (e) {
                $("#skillTree_txtSearch").val("");
                tree.clearFilter();
            });
    };


function selectSkillNode(skillId) {
    $("#employeeDetailSkillTree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (!node.folder && node.data.model != undefined && node.data.model.id == skillId) {
		        node.setActive();
		    }
		});

};

function selectSkillCategoryNode(categoryId) {
    $("#employeeDetailSkillTree")
		.fancytree("getRootNode")
		.visit(function (node) {
		    if (node.folder && node.data.model != undefined && node.data.model.id == categoryId) {
		        node.setActive();
		    }
		});

};