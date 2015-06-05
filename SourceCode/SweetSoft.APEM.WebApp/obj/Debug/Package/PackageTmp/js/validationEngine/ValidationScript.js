var SW_Validation = {
    check_validation_on_postback: false,
    check_validation_control: "#aspnetForm",
    check_validation_type: "validate"
}

$(function() {
    $("#aspnetForm").validationEngine();

});

function ChangeValidationMode(allow) {
    console.log("allow ", allow);
    SW_Validation.check_validation_on_postback = allow;
}

function CheckValidItem(itemid) {
    return $("#" + itemid).validationEngine("validate");
}

function CheckValid(id, type) {

    return $(id).validationEngine(type);
}

function CheckPageIsValid() {
    console.log("CheckPageIsValid");
    var Page_IsValid = true;
    if (SW_Validation.check_validation_on_postback) {
        var item = $(SW_Validation.check_validation_control);

        Page_IsValid = jQuery("#aspnetForm").validationEngine("validate", {
            addFailureCssClassToField: "error",
            promptPosition: "topLeft",
            onFieldFailure: function(x) {

            }
        });

        SW_Validation.check_validation_on_postback = false;
        SW_Validation.check_validation_control = "#aspnetForm";
        SW_Validation.check_validation_type = "validate";
        console.log("Page_IsValid", Page_IsValid);
        return Page_IsValid;
    }
    else {
        CheckValid(SW_Validation.check_validation_control, "detach");
    }
    return true;
}

function CheckValidWithButtonAndType(allow, groupid, type) {
    SW_Validation.check_validation_on_postback = allow;
    SW_Validation.check_validation_control = "[validation-group='" + groupid + "']";
    SW_Validation.check_validation_type = type;
}

function CheckValidWithButton(allow, groupid) {
    CheckValidWithButtonAndType(allow, groupid, "showPrompt");
}

function ShowPromtMessage(control, message) {
    $('#' + control).addClass('error').attr('data-prompt-position', 'inline').validationEngine('showPrompt', message, 'error', 'inline', false);
    //    setTimeout(function() {
    //        var divTip = $('#' + control).next();
    //        var pos = $('#' + control).offset();
    //        divTip.append('<img src="../images/error.png" title="' + $.trim(divTip.find('.formErrorContent').text())
    //          + '" style="left:' + (pos.left + $('#' + control).width()) + 'px;top:' + (pos.top - 15) + 'px;" />');
    //    }, 100);
}