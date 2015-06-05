/// <reference path="../js/core/bootstrap.min.js" />
/// <reference path="../barcode/jobrecord.js" />
/// <reference path="notification-vi.js" />
/// <reference path="../js/core/jquery.min.js" />
var SweetSoftMessageManager = {
    data: {
        timeout: undefined,
        msgCol: [],
        timeCall: 15000,
        isLoadMessage: true,
        ajRequest: undefined,
        container: undefined,
        badge: undefined,
        ul: undefined,
        errorCount: 0
    },
    commonFunction: {
        ajaxRequest: function (urlGet, dataValue, callback) {
            /// <summary>Get data from server by jQuery.</summary>
            if (!$('form').hasClass('progressClass')) {
                SweetSoftMessageManager.data.ajRequest = $.ajax({
                    type: "post",
                    async: true,
                    url: urlGet,
                    //traditional: false,
                    //dataType: "json",
                    data: dataValue,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    beforeSend: function () {
                        $('form').addClass('progressClass');
                    },
                    success: function (data, x, y) {
                        SweetSoftMessageManager.data.errorCount = 0;
                        $('form').removeClass('progressClass');
                        if (typeof callback === 'function')
                            callback(data);
                    },
                    error: function () {
                        SweetSoftMessageManager.data.errorCount += 1;
                        $('form').removeClass('progressClass');
                        if (typeof callback === 'function')
                            callback();
                    }
                });
            }
            else {
            }
        }
    },
    mainFunction: {
        init: function () {
            SweetSoftMessageManager.data.container = $('#notification');
            SweetSoftMessageManager.data.badge = SweetSoftMessageManager.data.container.find('.badge');
            SweetSoftMessageManager.data.ul = SweetSoftMessageManager.data.container.find('.dropdown:eq(0) .scrollmsg ul');
        },
        GetItemMessage: function (id) {
            var item;
            var obj = SweetSoftMessageManager.data.msgCol;
            if (typeof obj !== 'undefined') {
                $.each(obj, function (i, o) {
                    if (decodeURIComponent(o.id) === id.toString()) {
                        item = o;
                        return false;
                    }
                });
            }

            return item;
        },
        RemoveMessage: function (id) {
            var ul = SweetSoftMessageManager.data.ul;
            var badge = SweetSoftMessageManager.data.badge;
            if (ul && badge) {
                SweetSoftMessageManager.commonFunction.ajaxRequest('/notification/default.aspx',
                'nid=' + encodeURIComponent(id), function (dataRes) {
                    if (dataRes && dataRes.length > 0) {
                        if ($.trim(dataRes) === '1') {
                            SweetSoftMessageManager.mainFunction.CheckOpenSettingPage(id);
                            var li = ul.find('li[data-id="' + id + '"]');
                            if (li.length > 0) {
                                li.remove();

                                var num = parseInt(badge.text());
                                if (isNaN(num))
                                    badge.text('');
                                else {
                                    num = num - 1;
                                    if (num < 1) {
                                        badge.text('');
                                        ul.append('<li data-id=""><a href="javascript:void(0);">' + SweetSoftScript.ResourceText.NoNotification + '</a></li>');
                                    }
                                    else
                                        badge.text(num.toString());
                                }
                            }
                        }
                    }
                });
            }
        },
        OpenMessage: function (id) {
            var obj = SweetSoftMessageManager.data.msgCol;
            if (typeof obj !== 'undefined') {
                var item = SweetSoftMessageManager.mainFunction.GetItemMessage(id);
                if (typeof item !== 'undefined') {
                    var mess = item.content;
                    if (mess && mess.length > 0) {
                        var wd = $(document.getElementById(SweetSoftScript.Setting.wdId));
                        if (wd.length > 0) {
                            //var div = wd.find('.html');
                            var div = wd.find('.modal-footer');
                            if (item.action && item.action.length > 0) {
                                if (div.length > 0) {
                                    var isExist = false;
                                    var btn = div.find('.btnshowwindow');
                                    if (btn.length > 0)
                                        isExist = true;
                                    else
                                        btn = $('<input type="button" class="btnshowwindow btn btn-default" value="' + SweetSoftScript.ResourceText.ViewDetail + '" />');

                                    btn.unbind('click').off('click').click(function () {
                                        if (item.dismissevent && item.dismissevent.length > 0) { }
                                        else
                                            SweetSoftMessageManager.mainFunction.RemoveMessage(id);
                                        eval(item.action);
                                        return false;
                                    });

                                    //div.empty().show().append(btn);
                                    if (isExist === false)
                                        div.prepend(btn);
                                    else
                                        btn.show();
                                }
                            }
                            else {
                                SweetSoftMessageManager.mainFunction.RemoveMessage(id);
                                //if (div.length > 0)
                                //    div.hide();
                                var btn = div.find('.btnshowwindow');
                                if (btn.length > 0)
                                    btn.hide();
                            }
                        }
                        SweetSoftScript.mainFunction.OpenModalWindow(undefined, mess, 'alert');
                    }
                }
            }
        },
        GetMessage: function () {
            var linotifi = SweetSoftMessageManager.data.container.find('.dropdown:eq(0)');
            if (linotifi.length > 0 && linotifi.hasClass('open') && linotifi.find('li').length > 0) {
                if (SweetSoftMessageManager.data.isLoadMessage === true
                                   && SweetSoftMessageManager.data.timeCall > 0) {
                    SweetSoftMessageManager.data.timeout = setTimeout(function () {
                        SweetSoftMessageManager.mainFunction.GetMessage();
                    }, SweetSoftMessageManager.data.timeCall);
                }
            }
            else {
                SweetSoftMessageManager.commonFunction.ajaxRequest("/notification/default.aspx", '',
                    function (data) {
                        if (data && data.length > 0) {
                            var obj;
                            try {
                                obj = JSON.parse(data);
                                var ul = SweetSoftMessageManager.data.ul;
                                ul.empty();
                                if (obj.length > 0) {
                                    SweetSoftMessageManager.data.msgCol = obj;
                                    //var co = SweetSoftMessageManager.data.container;
                                    SweetSoftMessageManager.data.badge.text(obj.length);
                                    $.each(obj, function (i, o) {
                                        ul.append('<li data-id="' + decodeURIComponent(o.id) + '"><a href="javascript:SweetSoftMessageManager.mainFunction.OpenMessage(\'' + o.id + '\');">' + o.title + '</a></li>');
                                    });
                                }
                                else {
                                    SweetSoftMessageManager.data.badge.text('');
                                    ul.append('<li data-id=""><a href="javascript:void(0);">' + SweetSoftScript.ResourceText.NoNotification + '</a></li>');
                                }
                            }
                            catch (ex) {
                                if (SweetSoftMessageManager.data.isLoadMessage === true
                                && SweetSoftMessageManager.data.timeCall > 0) {
                                    SweetSoftMessageManager.data.timeout = setTimeout(function () {
                                        SweetSoftMessageManager.mainFunction.GetMessage();
                                    }, SweetSoftMessageManager.data.timeCall);
                                }
                            }

                            if (SweetSoftMessageManager.data.isLoadMessage === true
                                && SweetSoftMessageManager.data.timeCall > 0) {
                                SweetSoftMessageManager.data.timeout = setTimeout(function () {
                                    SweetSoftMessageManager.mainFunction.GetMessage();
                                }, SweetSoftMessageManager.data.timeCall);
                            }
                        }
                        else {
                            if (SweetSoftMessageManager.data.errorCount > 5)
                                SweetSoftMessageManager.mainFunction.ClearMessage();
                            else {
                                if (SweetSoftMessageManager.data.isLoadMessage === true
                                && SweetSoftMessageManager.data.timeCall > 0) {
                                    SweetSoftMessageManager.data.badge.text('');
                                    SweetSoftMessageManager.data.timeout = setTimeout(function () {
                                        SweetSoftMessageManager.mainFunction.GetMessage();
                                    }, SweetSoftMessageManager.data.timeCall);
                                }
                            }
                        }
                    });
            }
        },
        LoadAndShowMessage: function (id, callback) {
            SweetSoftMessageManager.commonFunction.ajaxRequest("/notification/default.aspx", 'gid=' + encodeURIComponent(id),
                function (data) {
                    if (data && data.length > 0) {
                        var item;
                        try {
                            item = JSON.parse(data);
                            var mess = item.content;
                            if (mess && mess.length > 0) {
                                var wd = $(document.getElementById(SweetSoftScript.Setting.wdId));
                                if (wd.length > 0) {

                                    var div = wd.find('.modal-footer');
                                    if (item.action && item.action.length > 0) {
                                        if (div.length > 0) {
                                            var isExist = false;
                                            var btn = div.find('.btnshowwindow');
                                            if (btn.length > 0)
                                                isExist = true;
                                            else
                                                btn = $('<input type="button" class="btnshowwindow btn btn-default" value="' + SweetSoftScript.ResourceText.ViewDetail + '" />');

                                            btn.unbind('click').off('click').click(function () {
                                                if (item.dismissevent && item.dismissevent.length > 0) { }
                                                else
                                                    SweetSoftMessageManager.mainFunction.RemoveMessage(id);
                                                eval(item.action);

                                                if (typeof callback === 'function')
                                                    callback();
                                                return false;
                                            });

                                            //div.empty().show().append(btn);
                                            if (isExist === false)
                                                div.prepend(btn);
                                            else
                                                btn.show();

                                            SweetSoftScript.mainFunction.OpenModalWindow(undefined, mess, 'alert');
                                        }
                                    }
                                    else {
                                        SweetSoftScript.mainFunction.OpenSimpleModalWindow(undefined, mess, 'alert');
                                        SweetSoftMessageManager.mainFunction.RemoveMessage(id);
                                        if (div.length > 0) {
                                            var btn = div.find('.btnshowwindow');
                                            if (btn.length > 0)
                                                btn.hide();
                                        }
                                        if (typeof callback === 'function')
                                            callback();
                                    }
                                }
                            }
                        }
                        catch (ex) { }
                    }
                    else if (typeof callback === 'function')
                        callback();
                });
        },
        ClearMessage: function () {
            clearTimeout(SweetSoftMessageManager.data.timeout);
            SweetSoftMessageManager.data.timeout = null;
        },
        ForceGetMessage: function () {
            SweetSoftMessageManager.mainFunction.ClearMessage();
            SweetSoftMessageManager.mainFunction.GetMessage();
        },
        OpenPage: function (title, pageUrl) {
            SweetSoftScript.mainFunction.CloseConfirmWindow();
            var all = $.window.getAll();
            if (typeof all !== 'undefined' && all.length > 0) {
                var isfound = false;
                $.each(all, function (i, o) {
                    var s = o.getUrl();
                    if (s.toLowerCase() === pageUrl.toLowerCase()) {
                        if (o.isMinimized())
                            o.maximize();
                        // o.restore();
                        isfound = true;
                        return false;
                    }
                });
                if (isfound === false)
                    openWindow(undefined, title, pageUrl);
            }
            else
                openWindow(undefined, title, pageUrl);
        },
        CheckOpenSettingPage: function (id) {
            var all = $.window.getAll();
            if (typeof all !== 'undefined' && all.length > 0) {
                var cur = $.Window.getSelectedWindow().getUrl();
                $.each(all, function (i, o) {
                    var s = o.getUrl();
                    if (s.toLowerCase().indexOf('notificationhistories.aspx') >= 0) {
                        var link = o.getFrame().contents().find('div[id$="grvPageList"] a.datamsg[data-id="' + id + '"]');
                        if (link.length > 0)
                            link.text(link.text());
                        return false;
                    }
                });
            }
        }
    }
};

$(function () {
    SweetSoftMessageManager.mainFunction.init();
    SweetSoftMessageManager.mainFunction.GetMessage();
});