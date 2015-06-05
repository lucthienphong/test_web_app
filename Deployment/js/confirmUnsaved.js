/// <reference path="core/jquery.min.js" />
/*
window.onbeforeunload = function () {
    if (document.getElementById("hdIsChanged").value != "") {
        var message = "   Your changes have not been saved!\n" +
                          "   Data will be lost when navagate away.\n\n" +
                          "   Do you really want to navigate?\n";
        return message;
    }
}
*/

if (typeof SweetSoftScript === 'undefined')
    SweetSoftScript = {};
SweetSoftScript.ConfirmUnsaved = {
    message: "   Your changes have not been saved!\n" +
            "   Data will be lost when navagate away.\n\n" +
            "   Do you really want to navigate?\n",
    init: function () {
        SweetSoftScript.ConfirmUnsaved.DetectChangeState();

        var beforeUnloadTimeout = 0;
        $(window).bind('beforeunload', function () {
            var hdf = $('input[type="hidden"][id$="hdIsChanged"]');
            if (hdf.length > 0 && hdf.val() !== '') {
                beforeUnloadTimeout = setTimeout(function () {
                    SweetSoftScript.ConfirmUnsaved.ClearSessionKeyText();
                }, 100);

                return SweetSoftScript.ConfirmUnsaved.message;
            }
            //else
            //    ClearSessionKeyText();
        });
        $(window).bind('unload', function () {
            if (typeof beforeUnloadTimeout !== 'undefined' && beforeUnloadTimeout !== 0)
                clearTimeout(beforeUnloadTimeout);
        });
    },
    DetectChangeState: function () {
        var hdf = $('input[type="hidden"][id$="hdIsChanged"]');
        if (hdf.length > 0) {
            var formElems = $('input[type="text"],textarea,input[type="checkbox"],input[type="radio"],select')
            .not('.ignore,[data-ignore="1"]');
            if (formElems.length > 0) {
                formElems.change(function () {
                    hdf.val('1');
                });
            }
        }
    },
    ClearSessionKeyText: function () {
        if ($('div[id$="AjaxUpdateProgress"]').is(':visible'))
            $('div[id$="AjaxUpdateProgress"]').hide();
    },
    MarkAsChanged: function () {
        var hdf = $('input[type="hidden"][id$="hdIsChanged"]');
        if (hdf.length > 0) {
            hdf.val('1');
        }
    },
    MarkAsSaved: function () {
        var hdf = $('input[type="hidden"][id$="hdIsChanged"]');
        if (hdf.length > 0) {
            hdf.val('');
            setTimeout(function () {
                if (hdf.length > 0)
                    hdf.val('');
            }, 10);
        }
        return true;
    },
    CheckDataChange: function () {
        var hdf = $('input[type="hidden"][id$="hdIsChanged"]');
        if (hdf.length > 0) {
            return hdf.val() === '1' ? SweetSoftScript.ConfirmUnsaved.message : '';
        }
        return '';
    }
};

$(function () {
    SweetSoftScript.ConfirmUnsaved.init();
});
