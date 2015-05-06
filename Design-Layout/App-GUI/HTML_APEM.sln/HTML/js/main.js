/// <reference path="plugins/bootstrap-plugins/bootstrap-datepicker.js" />
$(document).ready(function () {
    $('.button-control').affix({
        offset: {
            top: 0
        }
    });

    // Style checkboxes and radios
    $.fn.uniform && $(':radio.uniform, :checkbox.uniform').uniform();

    $('.selectpicker').selectpicker();
    $('.datepicker').datepicker();
    //checkable
    $('.table-checkable thead th.checkbox-column :checkbox').on('change', function () {
        var checked = $(this).prop('checked');
        $(this).parents('table').children('tbody').each(function (i, tbody) {
            $(tbody).find('.checkbox-column').each(function (j, cb) {
                $(':checkbox', $(cb)).prop("checked", checked).trigger('change');
                $(cb).closest('tr').toggleClass('checked', checked);
            });
        });
    });
    $('.table-checkable tbody tr td.checkbox-column :checkbox').on('change', function () {
        var checked = $(this).prop('checked');
        $(this).closest('tr').toggleClass('checked', checked);
    });

    //input mask
    //exam: http://igorescobar.github.io/jQuery-Mask-Plugin/
    $.mask;
    $('.mask-phone').mask('(00)0000-0000');
    $('.mask-date').mask("00/00/0000", { placeholder: "__/__/_______" });

    
})
//prepare for windowloading
$(window).load(function() {
    setTimeout(function() {
        $('#prepare_window_loading').fadeOut();
    },500)
})