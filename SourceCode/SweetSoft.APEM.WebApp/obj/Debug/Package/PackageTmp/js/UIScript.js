var activeTab = 0;
var actice2ndTab = 0;
$(function () {
    RestyleTopButton();
    addRequestHanlde(RestyleTopButton);

    RestyleCheckbox();
    addRequestHanlde(RestyleCheckbox);

    MaskTextBox();
    addRequestHanlde(MaskTextBox);


    RestyleDropdown();
    addRequestHanlde(RestyleDropdown);

    RestyleDatePicker();
    addRequestHanlde(RestyleDatePicker);

    RestyleMask();
    addRequestHanlde(RestyleMask);

    //RestyleTooltip();
    //addRequestHanlde(RestyleTooltip);

    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(SaveActiveTab);
    addRequestHanlde(SetActiveTab);

    TooltipButton();
    addRequestHanlde(TooltipButton);
});

function SaveActiveTab() {
    activeTab = $('.tabbable .nav-tabs li.active').index();
    actice2ndTab = $('.tabbable2nd .nav-tabs li.active').index();
}

function SetActiveTab() {
    if (activeTab)
        $('.tabbable .nav-tabs li:eq(' + activeTab + ') > a').click();
    if (actice2ndTab)
        $('.tabbable2nd .nav-tabs li:eq(' + actice2ndTab + ') > a').click();
}

function RestyleTopButton() {
    $('.button-control').affix({
        offset: {
            top: 0
        }
    });
}

function RestyleDatePicker() {
    $('.datepicker').datepicker({ format: 'dd/mm/yyyy' });
}

function RestyleTooltip() {
    $("[data-toggle='tooltip']").tooltip();
}

function RestyleDropdown() {
    $("[data-toggle='dropdown']").selectpicker();
}

function RestyleCheckbox() {
    $('.uniform > input[type=checkbox]').stop().uniform();
    $('.uniform > input[type=radio]').stop().uniform();
    $.fn.uniform && $(':radio.uniform, :checkbox.uniform').uniform();
}

function RestyleMask() {
    if (typeof $.fn.inputmask !== 'undefined') {
        $("input[data-inputmask],input[data-inputmask-regex]").each(function () {
            var type = undefined;
            if (typeof $(this).attr('data-inputmask-regex') !== 'undefined')
                type = 'Regex';
            $(this).inputmask(type, {});
        });
    }
}

//Thêm request
function addRequestHanlde(f) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(f);
}
//Xóa request
function removeRequestHanlde(f) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(clearRequestHandle);
}

function MaskTextBox() {
    $.mask;
    $('.mask-phone').mask('(00)0000-0000', { placeholder: "(00)0000-0000" });
    $('.mask-date').mask("00/00/0000", { placeholder: "__/__/_______" });
    $('.mask-percent').mask('AA.AAA', {
        reverse: true,
        'translation': {
            A: { pattern: /[0-9]/ }
        },
        placeholder: "00.000"
    });

    $('.mask-number').mask('###.AAA', {
        reverse: true,
        'translation': {
            A: { pattern: /[0-9]/ }
        },
        placeholder: "000.000"
    });
}

function TooltipButton() {
    $('[data-toggle="button_tooltip"]').bstooltip();
}