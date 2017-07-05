var SkillType = {
    Category: 0,
    Skill: 1
};

var positionMatrixEditorIsTreeMode = false;

initSkillTree = function () {
    $("#skilltreePositionMatrixEditor")
    .fancytree({
        extensions: ["dnd", "glyph", "table", "filter"],

        filter: {
            autoApply: true, // Re-apply last filter if lazy data is loaded
            counter: true, // Show a badge with number of matching child nodes near parent icons
            fuzzy: false, // Match single characters in order, e.g. 'fb' will match 'FooBar'
            hideExpandedCounter: true, // Hide counter badge, when parent is expanded
            highlight: false, // Highlight matches by wrapping inside <mark> tags
            mode: "hide" // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
        },

        dnd:
            {
                focusOnClick: true,
                dragStart: function (node, data) {
                    return true;
                },
                dragEnter: function (node, data) {
                    if (!node.folder) {
                        return false;
                    }

                    return true;
                },
                dragDrop: function (node, data) {
                    node.setExpanded(true)
                        .always(function () {
                            // Wait until expand finished, then post the change and move node
                            var otherNode = data.otherNode;
                            var entityId = otherNode.data.model.id;
                            var newParentId = node.data.model.id;
                        });
                }
            },

        glyph: glyph_opts,

        source:
            {
                url: "../../api/SkillCategory/root"
            },

        table:
            {
                checkboxColumnIdx: 0,
                nodeColumnIdx: 1
            },

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

    var tree = $("#skilltreePositionMatrixEditor").fancytree("getTree");

    $("#editPositionMatrix_txtSearch")
        .keyup(function (e) {
            var n,
                opts = {
                    autoExpand: true,
                    leavesOnly: false
                },
                match = $(this).val();

            if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
                $("button#editPositionMatrix_clearSearch").click();
                return;
            }
            // Pass a string to perform case insensitive matching
            n = tree.filterNodes(match, opts);
        })
        .focus();

    $("#editPositionMatrix_clearSearch")
        .click(function (e) {
            $("#editPositionMatrix_txtSearch").val("");
            tree.clearFilter();
        });

}

function onUpdateGroup(data, result) {
    var group = positionMatrixManager.findGroup(data.json.value.id);
    if (!group && data.json.value.state === CompetencyMatrix.EntityState.Added) {
        //New group
        positionMatrixManager.addGroup(data.json.value, data.view);
        
        if (positionMatrixEditorIsTreeMode) {
            var targetNode = $("#skillTreePositionMatrixSkillsEditor").fancytree("getActiveNode");
            if (!targetNode)
                targetNode = $("#skillTreePositionMatrixSkillsEditor").fancytree("getRootNode");

            targetNode.addChildren({
                title: data.json.value.name,
                id: data.json.value.id,
                name: data.json.value.name,
                groupTypeId: data.json.value.groupTypeId,
                children: [],
                folder: true,
                    model: data.json.value,
            });
            targetNode.setExpanded();
        }
        else {
            scrollToElement("group_" + data.json.value.id);
        }
    }
    else {
        //edit existing group
        positionMatrixManager.updateGroup(data.json.value);
    }

    closeModalDlg("positionMatrixEditGroupArea");
}

function closeModalDlg(containerId) {
    $('#' + containerId).closest('.modal.fade').modal('hide');
}


function onDrop(evt, ui) {

    var sourceNode = $(ui.helper).data("ftSourceNode"),
        data =
                {
                    id: sourceNode.data.model.id,
                    type: sourceNode.data.isLeaf ? SkillType.Skill : SkillType.Category,
                    matrixId: positionMartix.matrixId
                },
        parentId = null; //null means root group, below this value can be recalculated

    if (positionMatrixManager.findSkillBySkillId(sourceNode.data.model.id)) {
        showAlert('Error', 'Sorry, this matrix already has such skill. You cannot add the same skill twice.');
        return;
    }

    var targetId = $(evt.target).attr('id');

    if (targetId && targetId !== 'group_leaf' && targetId.indexOf('group_') === 0) {
        parentId = targetId.split('_')[1];
    }

    data.parentId = parentId;

    $.ajax({
        url: getGroupUIUrl,
        type: "POST",
        data: data,
        success: function (result, status) {

            if (status != "error") {

                if (!sourceNode.data.isLeaf) {
                    //Add list of skills to group
                    $(result.view).insertBefore($(String.format('#{0} #childGroup', targetId)).parent());
                }
                else {
                    //Add Skill to root
                    if ($(evt.target).attr('id') == 'detailsView') {
                        $(result.view).insertBefore($('#group_leaf #childGroup').parent());
                        $('#emptyPlaceholder').addClass('hidden');
                    }
                    else if (targetId && targetId.indexOf('group_') === 0) {
                        //Add skill to some group
                        $(result.view).insertBefore($(evt.target).find('td#childGroup').first().parent());
                    }
                }

                positionMatrixManager.addItem(targetId, result.json.value, sourceNode.data.isLeaf);
            }
        }
    });
}

function removeSkillNodeSelection(skillLevel) {
    var node = $("#skillTreePositionMatrixSkillsEditor").fancytree("getActiveNode");

    $("#skillTreePositionMatrixSkillsEditor").fancytree("getTree").activateKey(false);

    if (node) {
        //set new skill level before node rendering
        node.data.model.skillLevelId = skillLevel;
        node.render(true);
    }
}

function onSkillLevelChanged(obj) {
    var modelId = $(obj).attr('modelid'),
        skill = positionMatrixManager.findSkill(modelId);

    skill.skillLevelId = $(obj).val();
    if (skill.state !== CompetencyMatrix.EntityState.Added) {
        skill.state = CompetencyMatrix.EntityState.Modified;
    }

    positionMatrixManager.onModify();

    removeSkillNodeSelection(skill.skillLevelId);
}

function onSkillGroupChanged(obj, type) {
    var skill,
        group,
        modelId = $(obj).attr('modelid');
    
    if (type === SkillType.Category) {
        positionMatrixManager.updateGroupType(modelId, $(obj).val());
    }
    else {
        skill = positionMatrixManager.findSkill(modelId);
        skill.skillGroupTypeId = $(obj).val();
        if (skill.state !== CompetencyMatrix.EntityState.Added) {
            skill.state = CompetencyMatrix.EntityState.Modified;
        }
    }
    positionMatrixManager.onModify();
}

function goBack() {
    location.href = '/Home/PositionMatrices/' + positionMartix.matrixId;
}

function scrollToElement(id) {
    $("#detailsView").animate({ scrollTop: $("#" + id).offset().top });
}

function recalcButtonAvailability() {
    var selectedItems = $('.panel-body.ui-selected'),
        selectedCheckBoxes = $('input:checkbox:checked');

    $('#btnRenameGroup').prop('disabled', selectedItems.length !== 1);
    $('#btnRemove').prop('disabled', selectedItems.length === 0 && selectedCheckBoxes.length === 0);
    $('#btnAddGroup').prop('disabled', selectedItems.length > 1);
}

function selectSkillsGroup(modelId) {
    var div = $("#group_" + modelId).children("#group_" + modelId).children(".panel-body");
    var checkbox = $('#groupCheckbox_' + modelId);

    if ($(div).hasClass('selectedfilter')) {
        $(div).removeClass('selectedfilter').removeClass('ui-selected');
        checkbox.each(function(){ this.checked = false; });
        // do unselected stuff
    } else {
        $(div).addClass('selectedfilter').addClass('ui-selected');
        checkbox.each(function () { this.checked = true; });
        // do selected stuff
    }
    $('.ui-selecting').removeClass('ui-selecting');

    $('#groupName_' + modelId).removeClass('ui-selected');

    recalcButtonAvailability();
}

$(function () {

    $(".selectable").selectable({
        //autoRefresh: tru,
        filter: "label",
        selected: function (event, ui) {

            selectSkillsGroup($(ui.selected).attr('modelId'));
        },
    });

    function PositionMatrixManager(positionMatrix) {
        var generatedId = 0;
        var dirty = false;

        this.updateStarted = false;

        this.matrix = positionMatrix;

        this.isModified = function () {
            return dirty && !this.updateStarted;
        };

        this.getNewId = function () {
            generatedId = ++generatedId;
            return -generatedId;
        }

        this.addItem = function (targetId, data, isLeaf) {
            var skill;

            if (targetId === null || targetId == 'group_leaf' || targetId == 'detailsView') {


                if (!positionMartix.skills) {
                    positionMartix.skills = [];
                }

                for (var j = 0; j < data.skills.length; j++) {
                    skill = data.skills[j];
                    skill.state = CompetencyMatrix.EntityState.Added;
                    positionMartix.skills.push(skill);
                }
            }
            else if (targetId) {
                var parentId = targetId.split('_')[1];

                var group = this.findGroup(parentId);

                if (!group.childGroups) {
                    group.childGroups = [];
                }

                for (var j = 0; j < data.skills.length; j++) {
                    group.skills.push(data.skills[j]);
                }
            }
            $('#emptyPlaceholder').addClass('hidden');
            this.onModify();
        };

        this.addGroup = function (group, view) {
            //leaf group
            if (group.parentGroupId === null) {
                positionMartix.groups.push(group);

                if (!positionMatrixEditorIsTreeMode)
                    $('#matrixEditorContainer #detailsView').append(view);
            }
            else {
                //Add child group
                var parentGroup = this.findGroup(group.parentGroupId);

                if (!parentGroup.childGroups) {
                    parentGroup.childGroups = [];
                }

                parentGroup.childGroups.push(group);

                if (!positionMatrixEditorIsTreeMode)
                    $(String.format('#group_{0} td#childGroup', group.parentGroupId)).append(view);
            }

            var obj = $('#' + this.getCtrlGroupId(group.id));//.parent();

            $(obj).find(".droppable").droppable({
                greedy: true,
                revert: true,
                hoverClass: 'droppable-area',
                connectToFancytree: true,
                drop: function (event, ui) {
                    onDrop(event, ui);
                }
            });

            obj.find(".selectable").selectable({
                //autoRefresh: false,
                filter: "label",
                selected: function (event, ui) {

                    selectSkillsGroup($(ui.selected).attr('modelId'));
                    
                },
            });
            this.onModify();
        };

        this.updateGroup = function (newGroup) {

            var group = this.findGroup(newGroup.id);

            group.name = newGroup.name;

            if (group.state != CompetencyMatrix.EntityState.Added) {
                group.state = CompetencyMatrix.EntityState.Modified;
            }            

            this.renameNode(group.id, group.name);

            this.onModify();
        };

        this.renameNode = function(id, newName) {
            $("#skillTreePositionMatrixSkillsEditor")
                .fancytree("getRootNode")
                .visit(function (node) {
                    if (node.folder && node.data.model != undefined && node.data.model.id == id) {
                        node.title = htmlEncode(newName);
                        node.render(true);
                    }
                });
        };

        this.updateGroupType = function (groupId, groupTypeId) {

            var group = this.findGroup(groupId);

            group.groupTypeId = groupTypeId;

            if (group.state !== CompetencyMatrix.EntityState.Added) {
                group.state = CompetencyMatrix.EntityState.Modified;
            }
            
            this.onModify();
        };


        this.findSkill = function (value) {
            return this.findSkillByProperty(function (skill) {
                return skill.id == value;
            });
        };

        this.skillExists = function (id) {
            return !!this.findSkillBySkillId(id);
        };

        this.findSkillBySkillId = function (value) {
            return this.findSkillByProperty(function (skill) {
                return skill.skillId == value && !skill.hidden;
            });
        };

        this.findSkillByProperty = function (func) {
            var group,
                skill;

            for (var i = 0; i < this.matrix.skills.length; i++) {
                skill = this.matrix.skills[i];

                if (func(skill)) {
                    return skill;
                }
            }

            for (var i = 0; i < this.matrix.groups.length; i++) {
                skill = this.findSkillInGroup(this.matrix.groups[i], func)

                if (skill) {
                    return skill;
                }
            }

            return null;
        };

        this.findSkillInGroup = function (group, func) {
            var skill;

            for (var i = 0; i < group.skills.length; i++) {
                skill = group.skills[i];

                if (func(skill)) {
                    return skill;
                }
            }

            if (group.childGroups) {
                for (var i = 0; i < group.childGroups.length; i++) {
                    skill = this.findSkillInGroup(group.childGroups[i], func)

                    if (skill) {
                        return skill;
                    }
                }
            }

            return null;
        };

        this.findGroup = function (id) {
            var group;

            for (var i = 0; i < this.matrix.groups.length; i++) {
                group = this.findGroupInternal(this.matrix.groups[i], id);

                if (group) {
                    return group;
                }
            }
            return null;
        };

        this.findGroupByName = function (name) {
            var group;

            for (var i = 0; i < this.matrix.groups.length; i++) {
                group = this.findGroupInternalByName(this.matrix.groups[i], name.trim());

                if (group) {
                    return group;
                }
            }
        };

        this.findGroupInternalByName = function (group, name) {
            if (group.name == name) {
                return group;
            }

            if (group.childGroups) {
                for (var j = 0; j < group.childGroups.length; j++) {
                    var tmpGroup = this.findGroupInternal(group.childGroups[j], name);

                    if (tmpGroup) {
                        return tmpGroup;
                    }
                }
            }
            return null;
        };

        this.findGroupInternal = function (group, id) {
            if (group.id == id) {
                return group;
            }

            if (group.childGroups) {
                for (var j = 0; j < group.childGroups.length; j++) {
                    var tmpGroup = this.findGroupInternal(group.childGroups[j], id);

                    if (tmpGroup) {
                        return tmpGroup;
                    }
                }
            }
            return null;
        };

        this.getModelIdFromControl = function (ctrl) {

            var id = -1,
                modelId = $(ctrl).attr('modelid');

            if (modelId) {
                id = parseInt(modelId);
            }

            return id;
        };

        this.getCtrlGroupId = function (id) {
            return 'group_' + id;
        },

        this.removeSkill = function (id) {

            var skill = this.findSkill(id);

            if (skill.state != CompetencyMatrix.EntityState.Added) {
                skill.state = CompetencyMatrix.EntityState.Deleted;
            }
            else {
                //completely remove skill on the client because it was just added
                if (skill.skillGroupId !== null) {
                    var group = this.findGroup(skill.skillGroupId);

                    group.skills = jQuery.grep(group.skills, function (groupSkill) {
                        return groupSkill.id != skill.id;
                    });
                }
                else {
                    this.matrix.skills = jQuery.grep(this.matrix.skills, function (groupSkill) {
                        return groupSkill.id != skill.id;
                    });
                }
            }            

            var ctrl = $(String.format("#skillCheckbox[modelid*=\"{0}\"]", skill.id)).parents('tr:first');

            if (ctrl) {
                ctrl.remove();
            }
            this.onModify();
        };

        this.removeGroup = function (id) {
            var group = this.findGroup(id);
            group.state = CompetencyMatrix.EntityState.Deleted;

            var ctrl = $(String.format("#{0}", this.getCtrlGroupId(group.id)));

            if (ctrl) {
                ctrl.remove();
            }
            this.onModify();
        };

        this.onModify = function () {
            dirty = true;
        }

        this.updateMatrixName = function(name){
            this.matrix.matrixName = name;
            this.onModify();
        };

        this.onBeforeUpdate = function () {
            this.updateStarted = true;
        };

        this.onAfterUpdate = function () {
            this.updateStarted = false;
        }
    };

    positionMatrixManager = new PositionMatrixManager(positionMartix);

    $("#positionMatrixEditGroupDialog")
        .on('show.bs.modal',
            function (evt) {
                var data;

                if (evt.relatedTarget.tagName !== 'BUTTON') {
                    data = {
                        id: evt.relatedTarget.id,
                        name: evt.relatedTarget.name,
                        state: CompetencyMatrix.EntityState.Modified
                    };
                }
                else {

                    data = {
                        id: positionMatrixManager.getNewId(),
                        name: 'New group',
                        parentGroupId: getSelectedGroupId() || null,
                        state: CompetencyMatrix.EntityState.Added,
                        matrixId: positionMatrixManager.matrix.matrixId
                    };
                }

                $.ajax({
                    url: getUpdateGroupUrl,
                    data: data,
                    type: "get",
                    success: function (result, status) {
                        if (status != "error") {
                            $("#positionMatrixEditGroupArea").html(result);
                            $('#emptyPlaceholder').addClass('hidden');
                        }
                    }
                });
            });

    $('#btnCancel').click(function () {
        goBack();
    });

    function getSelectedGroupId() {

        var id = $('.panel-body.ui-selected').parent().attr('id');

        if (id) {
            var idPart = id.split('_')[1];

            if (idPart == 'leaf') {
                return 0;
            }
            return idPart;
        }

        return null;
    };

    $('#btnRenameGroup').click(function () {

        var panelId = $('.panel-body.ui-selected').parent().attr('id');

        if (panelId !== 'group_leaf') {
            var id = panelId.split('_')[1];

            var group = positionMatrixManager.findGroup(id);

            if (group) {
                $("#positionMatrixEditGroupDialog").modal('show', group);
            }
        }
    });

    $('input:radio[name="IsPublic"]').change(function () {
        positionMatrixManager.matrix.isPublic = this.value;
    });

    if (!positionMatrixManager.matrix.isPublicEditable) {

        $('input:radio[name="IsPublic"]').each(function(i) {
            $(this).attr('disabled', 'disabled');
        });
    }

    $('#btnSave').click(function () {

        positionMatrixManager.onBeforeUpdate();

        $.ajax({
            url: getUpdateUrl,
            type: "POST",
            data: { positionMatrix: JSON.stringify(positionMartix) },
            success: function (result, status) {
                
                if (status != "error") {
                    goBack();
                }
            }
        });
    });

    $('#btnRemove').click(function () {
        showConfirm('Confirm', 'Are you sure you want to delete selected items?', function () {
            var modelId;
            $('input:checkbox:checked').each(function () {
                modelId = positionMatrixManager.getModelIdFromControl(this);
                positionMatrixManager.removeSkill(modelId);
            });
        });

        var groupsToDelete = [];
        $('.panel-body.ui-selected').each(function () {
            var id = $(this).parent().attr('id').split('_')[1];
            if (id !== 'leaf') {
                var group = positionMatrixManager.findGroup(id);
                groupsToDelete.push(group);
            }
        });

        var groupsToDeleteNames = groupsToDelete.map(function (group) {
            return '"' + group.name + '"';
        });

        if (groupsToDelete.length > 0) {
            showConfirm('Confirm', 'You are about to delete groups: <br>' + groupsToDeleteNames.join(', <br>') + '.<br>Proceed?', function () {
                groupsToDelete.forEach(function (group) {
                    positionMatrixManager.removeGroup(group.id);
                });
            });
        }

        recalcButtonAvailability();
    });

    $(window).bind('beforeunload', function (e) {
        if (positionMatrixManager.isModified()) {
            return 'You have unsaved changes. Do you want to proceed?';
        }
    });

    $(".draggable").draggable({
        revert: true, //"invalid",
        cursorAt: { top: -5, left: -5 },
        connectToFancytree: true   // let Fancytree accept drag events
    });

    $(".droppable").droppable({
        //accept: '.valid',
        greedy: true,
        revert: true,
        hoverClass: 'droppable-area',
        connectToFancytree: true,
        drop: function (event, ui) {
            onDrop(event, ui);
        },
        click: function (event) {
            //prevent container and body from triggering click event
            event.stopPropagation();
        }
    });

    function makeScrollable() {
        var bottom = getLayoutFooter().position();
        var top = $('#skilltreePositionMatrixEditor').offset();
        var diff = bottom.top - top.top;
        var detailsViewContainer = $('#detailsViewContainer').offset();;
        var detailsView = $('#detailsView').offset();
        $('#editMatrixSkillTree').css('max-height', diff.toString() + 'px').css('height', diff.toString() + 'px');
        diff = diff - (detailsView.top - detailsViewContainer.top) + 2;
        var val = diff.toString() + 'px';
        $('#detailsView').css('max-height', val).css('height', val);
    }

    $(window).resize(function () {
        makeScrollable();
    }).resize();

    $("#lblMatrixName")
        .editable(
		    {
		        title: "Change name",
		        success: function (response, newValue) {
		            if (newValue) {
		                positionMatrixManager.updateMatrixName(newValue);
		            }
		            else {
		                showAlert('Error', 'Please enter name for matrix.');
		            }
		        }
		    });

})