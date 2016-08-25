var options = {

    url: "Officers.json",

    getValue: "name",
    //getValue: function(element) {
    //    return element.name;
    //},

    list: {
        match: {
            enabled: true
        }
    },

    theme: "square"
};

$("#officers-json").easyAutocomplete(options);
