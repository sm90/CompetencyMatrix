glyph_opts = {
    map: {
        checkbox: "glyphicon glyphicon-unchecked",
        checkboxSelected: "glyphicon glyphicon-check",
        checkboxUnknown: "glyphicon glyphicon-share",
        dragHelper: "glyphicon glyphicon-play",
        dropMarker: "glyphicon glyphicon-arrow-right",
        error: "glyphicon glyphicon-warning-sign",
        expanderClosed: "glyphicon glyphicon-menu-right",
        expanderLazy: "glyphicon glyphicon-menu-right",  // glyphicon-plus-sign
        expanderOpen: "glyphicon glyphicon-menu-down",  // glyphicon-collapse-down
    }
};

lazyLoadEmployeeSkills = function(event, data) {
    var node = data.node;
    var id = node.data.id;

    data.result =
    {
        url: "../api/Employees/fancytree/children/" + id,
        debugDelay: 1000
    };
}