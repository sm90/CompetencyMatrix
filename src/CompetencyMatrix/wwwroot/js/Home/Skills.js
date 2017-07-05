

initSkillEditorTree =
    function() {
        $("#skilltree")
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
                            focusOnClick: true,
                            dragStart: function(node, data) {
                                return true;
                            },
                            dragEnter: function(node, data) {
                                if (!node.folder) {
                                    return false;
                                }

                                return true;
                            },
                            dragDrop: function(node, data) {
                                node.setExpanded(true)
                                    .always(function() {
                                        // Wait until expand finished, then post the change and move node
                                        var otherNode = data.otherNode;
                                        var entityId = otherNode.data.model.id;
                                        var newParentId = node.data.model.id;

                                        var url;
                                        var postBody;

                                        if (otherNode.folder) {
                                            if (data.originalEvent.ctrlKey) {
                                                url = "../api/SkillCategory/clone";
                                            }
                                            else {
                                                url = "../api/SkillCategory/move";
                                            }

                                            postBody = { Id: entityId, ParentId: newParentId };
                                        }
                                        else {
                                            if (data.originalEvent.ctrlKey) {
                                                url = "../api/Skill/clone";
                                            }
                                            else {
                                                url = "../api/Skill/move";
                                            }

                                            postBody = { Id: entityId, CategoryId: newParentId };
                                        }

                                        $.ajax({
                                                url: url,
                                                type: "POST",
                                                data: JSON.stringify(postBody),
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function(result) {
                                                    if (data.originalEvent.ctrlKey) {
                                                        var newNodeData = {};

                                                        if (otherNode.folder) {
                                                            newNodeData = parseSkillCategory(result);
                                                        }
                                                        else {
                                                            newNodeData = parseSkill(result);
                                                        }

                                                        node.addNode(newNodeData, data.hitMode);
                                                    }
                                                    else {
                                                        otherNode.moveTo(node, data.hitMode);
                                                    }
                                                }
                                            });
                                    });
                            }
                        },

                    edit:
                        {
                            adjustWidthOfs: null, // null: don't adjust input size to content
                            inputCss: { width: "auto", background: "#aabbcc" },

                            save: function(event, data) {
                                // Only called when the text was modified and the user pressed enter or
                                // the <input> lost focus.
                                // Additional information is available (see `beforeClose`).
                                // Return false to keep editor open, for example when validations fail.
                                // Otherwise the user input is accepted as `node.title` and the <input> 
                                // is removed.
                                // Typically we would also issue an Ajax request here to send the new data 
                                // to the server (and handle potential errors when the asynchronous request 
                                // returns). 
                                var node = data.node;
                                var model = node.data.model;
                                var entityId = model ? model.id : 0;

                                var url;
                                var postBody;

                                if (data.input.val() == "") return false;

                                if (node.folder) {
                                    if (entityId === 0) {
                                        url = "../api/SkillCategory";
                                        postBody =
                                            {
                                                Id: entityId,
                                                ParentId: node.parent.data.model.id,
                                                Name: data.input.val()
                                            };
                                    }
                                    else {
                                        url = "../api/SkillCategory/rename";
                                        postBody = { id: entityId, name: data.input.val() };
                                    }
                                }
                                else {
                                    if (entityId === 0) {
                                        var matchNode = $("#skilltree").fancytree("getTree").findFirst(data.input.val());
                                        if (matchNode) {
                                            showAlert('Warning', 'Skill with name "' + matchNode.title + '" already exist.', null, null);
                                            node.remove();
                                            return;
                                        }
                                        url = "../api/Skill";
                                        postBody =
                                            {
                                                'Id': entityId,
                                                'CategoryId': node.parent.data.model.id,
                                                'Name': data.input.val()
                                            };
                                    }
                                    else {
                                        url = "../api/Skill/rename";
                                        postBody = { 'Id': entityId, 'Name': data.input.val() };
                                    }
                                }

                                $.ajax({
                                        url: url,
                                        type: "POST",
                                        data: postBody,
                                        success: function(result) {
                                            node.data.model = result;
                                            node.render(true);

                                            //Reflect changes in the selected skill/category name 
                                            var nameInput = $("#GeneralArea input[id=Name]");
                                            nameInput.val(result.name);

                                            renameActiveBreadCrumb(result.name);
                                        }
                                    });

                            },
                            close: function(event, data) {
                                // Editor was removed. If we started an async request, mark the node as pending
                                if (data.save) {
                                    $(data.node.span).addClass("pending");
                                }
                            }
                        },

                    glyph: glyph_opts,

                    source:
                        {
                            url: "../api/SkillCategory/root"
                        },

                    table:
                        {
                            checkboxColumnIdx: 0,
                            nodeColumnIdx: 1
                        },

                    activate: function(event, data) {
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

        var tree = $("#skilltree").fancytree("getTree");

        $("#skillTree_txtSearch")
            .keyup(function(e) {
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
            .click(function(e) {
                $("#skillTree_txtSearch").val("");
                tree.clearFilter();
            });
    };


function renameSkillNode(skillId, newName) {
	$("#skilltree")
	.fancytree("getRootNode")
	.visit(function(node) {
		if (!node.folder && node.data.model != undefined && node.data.model.id == skillId) {
		    node.title = htmlEncode(newName);
			node.render(true);
		}
	});

	renameActiveBreadCrumb(newName);
};

function renameSkillCategoryNode(categoryId, newName) {
	$("#skilltree")
	.fancytree("getRootNode")
	.visit(function(node) {
		if (node.folder && node.data.model != undefined && node.data.model.id == categoryId) {
		    node.title = htmlEncode(newName);
			node.render(true);
		}
	});

	renameActiveBreadCrumb(newName);
};

function renameActiveBreadCrumb(newName) {
	var breadCrumb = $("ol.breadcrumb li.active");
	breadCrumb.html(newName);
};

function selectSkillNode(skillId) {
	$("#skilltree")
		.fancytree("getRootNode")
		.visit(function(node) {
			if (!node.folder && node.data.model != undefined && node.data.model.id == skillId) {
				node.setActive();
			}
		});

};

function selectSkillCategoryNode(categoryId) {
	$("#skilltree")
		.fancytree("getRootNode")
		.visit(function(node) {
			if (node.folder && node.data.model != undefined && node.data.model.id == categoryId) {
				node.setActive();
			}
		});

};