/// <reference path="core/jquery.min.js" />
/// <reference path="../barcode/jobrecord-vi.js" />
/// <reference path="../barcode/jobrecord.js" />
/// <reference path="confirmUnsaved.js" />
//http://fstoke.me/jquery/window/
var SweetSoftScriptMain = {
    beforeUnloadTimeout: 0,
    replaceAll: function (strtoReplace, strTarget, strSubString) {
        /// <summary>Replaces all instances of the given substring.</summary>
        /// <param name="strtoReplace" type="string">The main string.</param>
        /// <param name="strTarget" type="string">The substring you want to replace.</param>
        /// <param name="strSubString" type="string">The string you want to replace in.</param>
        /// <returns type="string">The string replaced.</returns>
        var intIndexOfMatch = strtoReplace.indexOf(strTarget);

        // Keep looping while an instance of the target string
        // still exists in the string.
        while (intIndexOfMatch != -1) {
            // Relace out the current instance.
            strtoReplace = strtoReplace.replace(strTarget, strSubString)

            // Get the index of any next matching substring.
            intIndexOfMatch = strtoReplace.indexOf(strTarget);
        }

        // Return the updated string with ALL the target strings
        // replaced out with the new substring.
        return (strtoReplace);
    },
    init: function () {
        setTimeout(function () {
            $.window.prepare({
                dock: 'bottom',
                dockArea: $('#dockwindowpanel'),
                dock: 'bottom',       // change the dock direction: 'left', 'right', 'top', 'bottom'
                animationSpeed: 200,  // set animation speed
                minWinLong: 180
            });
        }, 500)

        $(window).on('blur', function (e) {
            $('.dropdown-toggle + .dropdown-menu:visible').parent().removeClass('open');
            //console.log(e.target);
            if ($("#menu").hasClass("in")) {
                //console.log('hide menu');
                $('#menu').collapse('hide');
            }
        });

        jQuery(document).bind('touchstart mousedown', function (event) {
            //var elementClicked = $(event.target).attr('class');
            var btn = $('button[data-toggle="collapse"]');
            if (btn.is(':visible')) {
                var div = $(event.target).closest('#menu');
                var el = $(event.target);
                //console.log(div, el.hasClass('window'));
                if ($("#menu").hasClass("in") && (div.length === 0 || el.hasClass('window')) && el.is(btn) === false)
                    //if ($("#menu").hasClass("in") && elementClicked != 'menu-item')
                    $('#menu').collapse('hide');
            }
        });

        $('.window').each(function () {
            $(this).click(function () {
                var _this = $(this);
                var title = $(this).data('title');
                var url = $(this).data('href');
                var show = $(this).attr('data-show');
                //if (show === "false") {
                openWindow(_this, title, url);
                //}


                /*
                var _this = $(this);
                var title = _this.data('title');
                var url = _this.data('href');
                var show = _this.attr('data-show');
                if (show === "true") {
                    var all = $.window.getAll();
                    if (typeof all !== 'undefined' && all.length > 0) {
                        $.each(all, function (i, o) {
                            var s = o.getUrl();
                            if (s.toLowerCase() === url.toLowerCase()) {
                                if (o.isMinimized())
                                    //o.maximize();
                                    o.restore();
                                //return false;
                            }
                            else
                                o.minimize();
                        });
                    }
                    //else
                    //    openWindow(_this, title, url);
                }
                else
                    openWindow(_this, title, url);
                */
            });
        });

        //SweetSoftScriptMain.callCheckUnsaved();
    },
    hideOther: function () {
        var all = $.window.getAll();
        if (typeof all !== 'undefined' && all.length > 0) {
            var cur = $.Window.getSelectedWindow().getUrl();
            $.each(all, function (i, o) {
                var s = o.getUrl();
                console.log(s, ' vs ', cur);

                if (s.toLowerCase() !== cur.toLowerCase()) {
                    if (o.isMinimized() === false)
                        o.minimize();
                    return false;
                }
            });
        }
    },
    callopenWindow: function (linkElem, title, url) {
        $('#relative').window({
            title: title,
            url: url,
            hideOther: true,
            bookmarkable: false,
            checkBoundary: true,
            width: 900,
            height: 400,
            custBtns: customButton,
            iframeRedirectCheckMsg: "the window is going to redirect to " + url
                + "!!\r\nPlease select 'cancel' to stay here.",
            onOpen: function (api) {
                if (typeof linkElem !== 'undefined')
                    linkElem.attr('data-show', 'true');
            },
            onIframeEnd: function (api) {
                //console.log('open');
                api.maximize();
            },
            onClose: function (api) {
                if (typeof linkElem !== 'undefined')
                    linkElem.attr('data-show', 'false');
                var ct = $.window.getSelectedWindow().getFrame();
                try {
                    if (ct.length > 0 && ct[0].contentWindow.SweetSoftScript) {
                        var msg = ct[0].contentWindow.SweetSoftScript.ConfirmUnsaved.CheckDataChange();
                        //console.log('msg : ', msg);

                        if (msg.length > 0) {
                            SweetSoftScriptMain.showConfirmNavigate(api, msg);
                            return false;
                        }
                    }
                }
                catch (ex) {

                }
            },
            /*
            afterMaximize: function (wnd) {
                var id = wnd.getWindowId();
                var toolbar = $('#' + id + ' .window_function_bar');
                if (toolbar.length > 0 && toolbar.is(':hidden'))
                    toolbar.show();
                var allWindow = $.window.getAll();
                $.each(allWindow, function (i, o) {
                    if (o.isMinimized() === false && o.isSelected() === false) {
                        o.minimize();
                    }
                })
            },
            afterCascade: function (wnd) {
                var allWindow = $.window.getAll();
                if (wnd.isMaximized()) {
                    $.each(allWindow, function (i, o) {
                        if (o.isSelected() === false && o.isMinimized() === false) {
                            o.minimize();
                        }
                    })
                } else {
                    $.each(allWindow, function (i, o) {
                        if (o.isSelected() === false && o.isMinimized() === false && o.isMaximized() === true) {
                            o.minimize();
                        }
                    })
                }
            },*/
            onCascade: function (api) {
                //console.log('onCascade ', api);
                //api.maximize();
            },
            afterCascade: function (api) {
                //SweetSoftScriptMain.hideOther();
            }
        });
    },
    callCheckUnsaved: function () {
        $(window).bind('beforeunload', function () {
            //console.log('beforeunload');
            var all = $.window.getAll();
            var isBreak = false;
            if (typeof all !== 'undefined' && all.length > 0) {
                $.each(all, function (i, o) {
                    if (o.getFrame()[0].contentWindow.SweetSoftScript.ConfirmUnsaved.CheckDataChange().length > 0) {
                        isBreak = true;
                        return 'Can\'t close';
                    }
                    if (isBreak === true)
                        return false;
                });
            }
        });
        $(window).bind('unload', function () {
            //console.log('unload');
            if (typeof SweetSoftScriptMain.beforeUnloadTimeout !== 'undefined'
                && SweetSoftScriptMain.beforeUnloadTimeout !== 0)
                clearTimeout(SweetSoftScriptMain.beforeUnloadTimeout);
        });
    },
    showConfirmNavigate: function (api, msg) {
        SweetSoftScript.mainFunction.OpenSimpleModalWindow('Notice',
            SweetSoftScriptMain.replaceAll(msg, '\n', '<br/>'), 'confirmDelete',
            function () {
                if (api && typeof api === 'object') {
                    //console.log('api : ', api);
                    api.getContainer().remove();
                    var alw = $.window.getAll();
                    if (typeof alw !== 'undefined' && alw.length > 0) {
                        for (var i = 0; i < alw.length; i++) {
                            if (alw[i].getWindowId() === api.getWindowId()) {
                                alw.splice(i, 1);
                                break;
                            }
                        }
                    }
                }
                else {
                    //console.log(api);
                    if (typeof api === 'function')
                        api();
                }
            });
    }
};
$(document).ready(function () {
    SweetSoftScriptMain.init();
})

var customButton = [
{
    id: "btn_refresh",
    title: "refresh page",
    clazz: "refreshImg",
    image: "/img/refreshImg.png",
    callback: function (btn, wnd) {
        //old
        //var f = wnd.getContainer().find('iframe');
        //f[0].contentWindow.location.reload(true);

        //new
        var f = wnd.getContainer().find('iframe');
        if (typeof f[0].contentWindow.SweetSoftScript !== 'undefined' &&
            typeof f[0].contentWindow.SweetSoftScript.ConfirmUnsaved !== 'undefined') {
            var msg = f[0].contentWindow.SweetSoftScript.ConfirmUnsaved.CheckDataChange();
            //console.log('msg : ', msg);
            if (msg.length > 0) {
                SweetSoftScript.mainFunction.OpenSimpleModalWindow('Notice',
                   SweetSoftScriptMain.replaceAll(msg, '\n', '<br/>'), 'confirmDelete',
                   function () {
                       f[0].contentWindow.SweetSoftScript.ConfirmUnsaved.MarkAsSaved();
                       f[0].contentWindow.location.reload(true);
                   });
                return false;
            }
            else
                f[0].contentWindow.location.reload(true);
        }
        else
            f[0].contentWindow.location.reload(true);
    }
}
];

function openWindow(linkElem, title, url) {
    //console.log(linkElem, title, url);
    var all = $.window.getAll();
    if (typeof all !== 'undefined' && all.length > 0) {
        var isfound = false;
        $.each(all, function (i, o) {
            var s = o.getUrl();
            if (s.toLowerCase() === url.toLowerCase()) {
                //console.log('found ', s);
                if (o.isMinimized() === true)
                    o.getHeader().click();
                isfound = true;
            }
            else if (o.isMinimized() === false)
                o.minimize();
        });
        if (isfound === false)
            SweetSoftScriptMain.callopenWindow(linkElem, title, url);
    }
    else
        SweetSoftScriptMain.callopenWindow(linkElem, title, url);
}
