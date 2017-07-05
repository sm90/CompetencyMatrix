(function ($) {
    "use strict";

    var Employee = function (options) {
        this.init('employee', options, Employee.defaults);
    };

    //inherit from Abstract input
    $.fn.editableutils.inherit(Employee, $.fn.editabletypes.abstractinput);

    $.extend(Employee.prototype, {
        /**
        Renders input from tpl

        @method render() 
        **/
        render: function () {
            this.$input = this.$tpl.find('input');
        },

        /**
        Default method to show value in element. Can be overwritten by display option.
        
        @method value2html(value, element) 
        **/
        value2html: function (value, element) {
            if (!value) {
                $(element).empty();
                return;
            }
            var html = $('<div>').text(value.name).html() + ', ' + $('<div>').text(value.cell).html() + ', ' + $('<div>').text(value.office).html() 
            + $('<div>').text(value.profilestatus).html() + $('<div>').text(value.email).html() + $('<div>').text(value.skype).html() + $('<div>').text(value.title).html();
            $(element).html(html);
        },

        /**
        Gets value from element's html
        
        @method html2value(html) 
        **/
        html2value: function (html) {
            /*
              you may write parsing method to get value by element's html
              e.g. "Moscow, st. Lenina, bld. 15" => {city: "Moscow", street: "Lenina", building: "15"}
              but for complex structures it's not recommended.
              Better set value directly via javascript, e.g. 
              editable({
                  value: {
                      city: "Moscow", 
                      street: "Lenina", 
                      building: "15"
                  }
              });
            */
            return null;
        },

        /**
         Converts value to string. 
         It is used in internal comparing (not for sending to server).
         
         @method value2str(value)  
        **/
        value2str: function (value) {
            var str = '';
            if (value) {
                for (var k in value) {
                    str = str + k + ':' + value[k] + ';';
                }
            }
            return str;
        },

        /*
         Converts string to value. Used for reading value from 'data-value' attribute.
         
         @method str2value(str)  
        */
        str2value: function (str) {
            /*
            this is mainly for parsing value defined in data-value attribute. 
            If you will always set value by javascript, no need to overwrite it
            */
            return str;
        },

        /**
         Sets value of input.
         
         @method value2input(value) 
         @param {mixed} value
        **/
        value2input: function (value) {
            if (!value) {
                return;
            }
            this.$input.filter('[name="name"]').val(value.name);
            this.$input.filter('[name="cell"]').val(value.cell);
            this.$input.filter('[name="office"]').val(value.office);
            this.$input.filter('[name="profilestatus"]').val(value.profilestatus);
            this.$input.filter('[name="email"]').val(value.email);
            this.$input.filter('[name="skype"]').val(value.skype);
            this.$input.filter('[name="title"]').val(value.title);
        },

        /**
         Returns value of input.
         
         @method input2value() 
        **/
        input2value: function () {
            return {
                name: this.$input.filter('[name="name"]').val(),
                cell: this.$input.filter('[name="cell"]').val(),
                office: this.$input.filter('[name="office"]').val(),
                profilestatus: this.$input.filter('[name="profilestatus"]').val(),
                email: this.$input.filter('[name="email"]').val(),
                skype: this.$input.filter('[name="skype"]').val(),
                title: this.$input.filter('[name="title"]').val()
            };
        },

        /**
        Activates input: sets focus on the first field.
        
        @method activate() 
       **/
        activate: function () {
            this.$input.filter('[name="name"]').focus();
        },

        /**
         Attaches handler to submit form in case of 'showbuttons=false' mode
         
         @method autosubmit() 
        **/
        autosubmit: function () {
            this.$input.keydown(function (e) {
                if (e.which === 13) {
                    $(this).closest('form').submit();
                }
            });
        }
    });

    Employee.defaults = $.extend({}, $.fn.editabletypes.abstractinput.defaults, {
        tpl: '<div class="editable-employee"><label><span>Name: </span><input type="text" name="name" class="input-small"></label></div>' +
             '<div class="editable-employee"><label><span>Cell: </span><input type="text" name="cell" class="input-small"></label></div>' +
             '<div class="editable-employee"><label><span>Office: </span><input type="text" name="office" class="input-mini"></label></div>'+
             '<div class="editable-employee"><label><span>Profile status: </span><input type="text" name="profilestatus" class="input-small"></label></div>'+
             '<div class="editable-employee"><label><span>Email: </span><input type="text" name="email" class="input-small"></label></div>'+
             '<div class="editable-employee"><label><span>Skype: </span><input type="text" name="skype" class="input-small"></label></div>'+
             '<div class="editable-employee"><label><span>Title: </span><input type="text" name="title" class="input-small"></label></div>',

        inputclass: ''
    });

    $.fn.editabletypes.employee = Employee;

}(window.jQuery));