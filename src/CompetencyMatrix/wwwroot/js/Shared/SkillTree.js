glyph_opts = {
    map: {
        doc: "glyphicon glyphicon-file icon-skill",
        docOpen: "glyphicon glyphicon-file icon-skill",
        checkbox: "glyphicon glyphicon-unchecked",
        checkboxSelected: "glyphicon glyphicon-check",
        checkboxUnknown: "glyphicon glyphicon-share",
        dragHelper: "glyphicon glyphicon-play",
        dropMarker: "glyphicon glyphicon-arrow-right",
        error: "glyphicon glyphicon-warning-sign",
        expanderClosed: "glyphicon glyphicon-menu-right",
        expanderLazy: "glyphicon glyphicon-menu-right",  // glyphicon-plus-sign
        expanderOpen: "glyphicon glyphicon-menu-down",  // glyphicon-collapse-down
        folder: "glyphicon glyphicon-folder-close icon-skillcategory",
        folderOpen: "glyphicon glyphicon-folder-open icon-skillcategory",
        loading: "glyphicon glyphicon-refresh glyphicon-spin"
    }
};

lazyLoadCategory = function(event, data) {
    var node = data.node;
    var id = node.data.id;

    data.result =
    {
        url: "../api/SkillCategory/fancytree/children/" + id,
        debugDelay: 1000
    };
}

function parseSkillCategory(category) {
    var lazy = false;
    var subCategories = category.inverseParent;
    var skills = category.skill;
    return {
        title: htmlEncode(category.name),
        expanded: category.parentId == undefined,
        folder: true,
        tooltip: category.description,
        lazy: lazy,
        model: category,
        children: ((subCategories !== undefined && subCategories != null && subCategories.length) ? parseSkillCategoryList(subCategories) : []).concat(((skills !== undefined && skills != null && skills.length) ? parseSkillList(skills) : []))
    }
}

function parseSkill(skill) {
    return {
        title: htmlEncode(skill.name),
        folder: false,
        tooltip: skill.description,
        model: skill,
        isLeaf: true,
        children: null
    }
}

function parseSkillCategoryList(categories) {
    var children = [];
    $.each(categories, function() {
        children.push(parseSkillCategory(this));
    });
    return children;
}

function parseSkillList(skills) {
    var children = [];
    $.each(skills, function() {
        children.push(parseSkill(this));
    });
    return children;
}
