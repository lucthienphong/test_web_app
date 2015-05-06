/// <reference path="../js/core/jquery.min.js" />
/// <reference path="../js/core/bootstrap.min.js" />
/// <reference path="jobrecord-en.js" />
if (typeof SweetSoftScript === 'undefined')
    SweetSoftScript = {};
SweetSoftScript.Setting = {
    agreeBtnId: '',
    closeBtnId: '',
    saveBtnId: '',
    wdId: '',
    labelTitle: '',
    labelMessage: ''
};
SweetSoftScript.Data = {
    ajRequest: undefined,
    focusBtn: undefined
};
//  ResourceText: undefined,
SweetSoftScript.commonFunction = {
    initLoad: function () {
        var agreeBtn = document.getElementById(SweetSoftScript.Setting.agreeBtnId);
        if (agreeBtn !== null) {
            agreeBtn.value = SweetSoftScript.ResourceText.OK;
            agreeBtn.removeAttribute('onclick');
            $(agreeBtn).bind('click', function (evt) {
                evt.preventDefault();
                SweetSoftScript.mainFunction.CloseConfirmWindow();
                return false;
            });
        }

        var saveBtn = document.getElementById(SweetSoftScript.Setting.saveBtnId);
        if (saveBtn !== null) {
            saveBtn.value = SweetSoftScript.ResourceText.Save;
            saveBtn.removeAttribute('onclick');
            $(saveBtn).bind('click', function (evt) {
                evt.preventDefault();
                SweetSoftScript.mainFunction.CloseConfirmWindow();
                return false;
            });
        }

        var closeBtn = document.getElementById(SweetSoftScript.Setting.closeBtnId);
        if (closeBtn !== null) {
            closeBtn.value = SweetSoftScript.ResourceText.Cancel;
            closeBtn.removeAttribute('onclick');
            $(closeBtn).bind('click', function (evt) {
                evt.preventDefault();
                SweetSoftScript.mainFunction.CloseConfirmWindow();
                return false;
            });
        }

        var wd = $(document.getElementById(SweetSoftScript.Setting.wdId));
        if (wd.length > 0) {
            wd.on('shown.bs.modal', function () {
                if (typeof SweetSoftScript.Data.focusBtn !== 'undefined')
                    SweetSoftScript.Data.focusBtn.focus();
            });
        }
    },
    getIEVersion: function () {
        var rv = -1;
        if (navigator.appName === 'Microsoft Internet Explorer') {
            var ua = navigator.userAgent;
            var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) !== null)
                rv = parseFloat(RegExp.$1);
        }
        else if (navigator.appName === 'Netscape') {
            var ua = navigator.userAgent;
            var re = new RegExp("Trident/.*rv:([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) !== null)
                rv = parseFloat(RegExp.$1);
        }
        return rv;
    },
    includeToHead: function (url) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script'); script.type = 'text/javascript';
        script.src = url;
        head.appendChild(script);
    },
    preloadLoadImage: function (imgSrc, callback) {
        var imgPreload = new Image();
        imgPreload.src = imgSrc;

        if (imgPreload.complete) {
            callback(imgPreload);
        }
        else {
            imgPreload.onload = function () {
                callback(imgPreload);
            }
            imgPreload.onerror = function () {
                callback(undefined);
            }
        }
    },
    sortObjectProperties: function (obj) {
        var arr = [];
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                arr.push({
                    'key': prop,
                    'value': obj[prop]
                });
            }
        }
        arr.sort(function (a, b) {
            return (a.value.localeCompare(b.value));
        });
        return arr;
    },
    ajaxRequest: function (urlGet, dataValue, callback) {
        /// <summary>Get data from server by jQuery.</summary>
        //console.log('ajaxRequest : ', dataValue);
        if (!$('form').hasClass('progressClass')) {
            SweetSoftScript.Data.ajRequest = $.ajax({
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
                success: function (data) {
                    $('form').removeClass('progressClass');
                    if (typeof callback === 'function')
                        callback(data);
                },
                error: function () {
                    $('form').removeClass('progressClass');
                    if (typeof callback === 'function')
                        callback();
                }
            });
        }
        else {
            //console.log('Can\'t load data while process');
        }
    }

}
SweetSoftScript.mainFunction = {
    getWindowSize: function () {
        var w = 0; var h = 0;
        //IE
        if (!window.innerWidth) {
            if (!(document.documentElement.clientWidth === 0)) {
                //strict mode
                w = document.documentElement.clientWidth;
                h = document.documentElement.clientHeight;
            } else {
                //quirks mode
                w = document.body.clientWidth; h = document.body.clientHeight;
            }
        } else {
            //w3c
            w = window.innerWidth; h = window.innerHeight;
        }
        return { width: w, height: h };
    },
    OpenSimpleModalWindow: function (title, message, type, dataAction1, dataAction2, text1, text2) {
        var wd = $(document.getElementById(SweetSoftScript.Setting.wdId));
        if (wd.length > 0) {
            //var div = wd.find('.html');
            var div = wd.find('.modal-footer');
            if (div.length > 0) {
                //div.hide();

                var btn = div.find('.btnshowwindow');
                if (btn.length > 0)
                    btn.hide();
            }
        }
        SweetSoftScript.mainFunction.OpenModalWindow(title, message, type, dataAction1, dataAction2, text1, text2);
    },
    OpenModalWindow: function (title, message, type, dataAction1, dataAction2, text1, text2) {
        var wd = $(document.getElementById(SweetSoftScript.Setting.wdId));
        var btnCancel = null;
        if (wd.length > 0) {
            //show or hide process button
            var agreeBtn = document.getElementById(SweetSoftScript.Setting.agreeBtnId);
            var btn2 = document.getElementById(SweetSoftScript.Setting.saveBtnId);
            if (agreeBtn !== null) {
                if (typeof type !== 'undefined') {
                    switch (type) {
                        case 'alert': {
                            agreeBtn.style.display = 'none';
                            btn2.style.display = 'none';
                            btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                            if (btnCancel !== null)
                                btnCancel.value = text1 || SweetSoftScript.ResourceText.OK;
                            $(btnCancel).unbind();
                            if (typeof dataAction1 !== 'undefined') {
                                wd.off('hidden.bs.modal').on('hidden.bs.modal', function () {
                                    dataAction1();
                                });
                            }
                            $(btnCancel).bind('click', function (evt) {
                                evt.preventDefault();
                                SweetSoftScript.mainFunction.CloseConfirmWindow();
                            });
                        }
                            break;
                        case 'confirmDelete':
                        case 'confirmRetype':
                            {
                                agreeBtn.style.display = 'inline-block';
                                btn2.style.display = 'none';
                                $(agreeBtn).unbind();

                                agreeBtn.value = text1 || SweetSoftScript.ResourceText.OK;

                                if (typeof dataAction1 !== 'undefined') {
                                    $(agreeBtn).bind('click', function (evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }
                                else {
                                    $(agreeBtn).bind('click', function (evt) {
                                        evt.preventDefault();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null) {
                                    btnCancel.value = text2 || SweetSoftScript.ResourceText.Cancel;
                                    if (typeof dataAction2 !== 'undefined') {
                                        $(btnCancel).unbind();
                                        $(btnCancel).bind('click', function (evt) {
                                            evt.preventDefault();
                                            dataAction2();
                                            SweetSoftScript.mainFunction.CloseConfirmWindow();
                                        });
                                    } else {
                                        $(btnCancel).bind('click', function (evt) {
                                            evt.preventDefault();
                                            SweetSoftScript.mainFunction.CloseConfirmWindow();
                                        });
                                    }
                                }
                            }
                            break;
                        case 'confirmCancelSave':
                            {
                                agreeBtn.style.display = 'inline-block';
                                btn2.style.display = 'inline-block';
                                agreeBtn.value = text1 || SweetSoftScript.ResourceText.Yes;
                                btn2.value = text2 || SweetSoftScript.ResourceText.No;
                                $(agreeBtn).unbind();
                                if (typeof dataAction1 !== 'undefined') {
                                    $(agreeBtn).bind('click', function (evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                } else {
                                    $(agreeBtn).bind('click', function (evt) {
                                        evt.preventDefault();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                $(btn2).unbind();
                                if (typeof dataAction2 !== 'undefined') {
                                    $(btn2).bind('click', function (evt) {
                                        evt.preventDefault();
                                        dataAction2();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                } else {
                                    $(btn2).bind('click', function (evt) {
                                        evt.preventDefault();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null) {
                                    btnCancel.value = SweetSoftScript.ResourceText.Cancel;
                                    $(btnCancel).unbind();
                                    $(btnCancel).bind('click', function (evt) {
                                        evt.preventDefault();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (message && message.length > 0) {
                $(document.getElementById(SweetSoftScript.Setting.labelMessage)).html(message).parent().show();
            }
            else {
                $(document.getElementById(SweetSoftScript.Setting.labelMessage)).html('').parent().hide();
            }

            wd.modal('show');

            //title of window
            if (typeof title !== 'undefined' && title.length > 0)
                $(document.getElementById(SweetSoftScript.Setting.labelTitle)).text(title);
            else
                $(document.getElementById(SweetSoftScript.Setting.labelTitle)).text(SweetSoftScript.ResourceText.notice);

            //console.log(agreeBtn, btn2, btnCancel);
            if (agreeBtn !== null && agreeBtn.style.display !== 'none') {
                agreeBtn.className = 'focus btn btn-default';
                SweetSoftScript.Data.focusBtn = agreeBtn;
                if (btnCancel !== null)
                    btnCancel.className = 'btn btn-default';
            }
            else if (btnCancel !== null && btnCancel.style.display !== 'none') {
                btnCancel.className = 'focus btn btn-default';
                SweetSoftScript.Data.focusBtn = btnCancel;
                if (agreeBtn !== null)
                    agreeBtn.className = 'btn btn-default';
            }
        }
    },
    CloseConfirmWindow: function () {
        var radWindowConfirm = $(document.getElementById(SweetSoftScript.Setting.wdId));
        if (radWindowConfirm.length > 0) {
            radWindowConfirm.modal('hide');
        }
        return false;
    }
}

$(function () {
    SweetSoftScript.commonFunction.initLoad();
    //SweetSoftScript.jobbarcode.init();
});