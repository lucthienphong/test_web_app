/// <reference name="MicrosoftAjax.js" />
/// <reference name="MicrosoftAjaxWebForms.js" />
/// <reference name="BaseScripts.js" assembly="AjaxControlToolkit" />
/// <reference name="Common.js" assembly="AjaxControlToolkit" />
/// <reference path="jquery-1.11.1.min.js" />


function CheckPageWhenClose() {
    return true;
}

window.onload = function () {
    if (window.parent != null) {
        if (typeof window.parent.ClosedfLoading === "function")
            window.parent.ClosedfLoading();
    }
}

var SweetSoftScript = {
    Setting: {
        progressClass: 'tl_inprogress',
        sectionRange: 90,
        title: '',
        message: {}
    },
    Data: {
        ajRequest: undefined,
        arrayId: [],
        arraySectionId: [],
        arrayInfoId: [],
        arrayMainId: [],
        process: {
            isWaitProcess: false,
            num: 0,
            dataTime: undefined
        },
        cache: {},
        isCylinderTimeline: false
    },
    ResourceText: {
        pleaseSelectJob: '',
        notice: '',
        noData: ''
    },
    commonFunction: {
        showLoading: function (callback) {
            var divloader = $('#page_spinner');
            if (divloader.length > 0) {
                divloader.fadeIn('fast', function () {
                    if (typeof callback === 'function')
                        callback();
                });
            }
            else {
                if (typeof callback === 'function')
                    callback();
            }
        },
        hideLoading: function (callback) {
            var divloader = $('#page_spinner');
            if (divloader.length > 0) {
                divloader.fadeOut('fast', function () {
                    if (typeof callback === 'function')
                        callback();
                });
            }
            else {
                if (typeof callback === 'function')
                    callback();
            }
        },
        ajaxGet: function (urlGet, dataValue, callback) {
            /// <summary>Get data from server by jQuery.</summary>
            if (!$('form').hasClass(SweetSoftScript.Setting.progressClass)) {
                SweetSoftScript.Data.ajRequest = $.ajax({
                    type: "post",
                    async: true,
                    url: urlGet,
                    //traditional: false,
                    //dataType: "json",
                    data: dataValue,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    beforeSend: function () {
                        $('form').addClass(SweetSoftScript.Setting.progressClass);
                    },
                    success: function (data) {
                        $('form').removeClass(SweetSoftScript.Setting.progressClass);
                        if (typeof callback === 'function')
                            callback(data);
                    },
                    error: function () {
                        $('form').removeClass(SweetSoftScript.Setting.progressClass);
                        if (typeof callback === 'function')
                            callback();
                    }
                });
            }
            else {
                if (typeof callback === 'function')
                    callback();
            }
        },
        hexcolorFromRGB: function (colorval) {
            var parts = colorval.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
            delete (parts[0]);
            for (var i = 1; i <= 3; ++i) {
                parts[i] = parseInt(parts[i]).toString(16);
                if (parts[i].length === 1) parts[i] = '0' + parts[i];
            }
            return '#' + parts.join('');
        }
    },
    mainFunction: {
        caculateTime: function (dateTo, dateFrom) {
            var diff = dateFrom - dateTo, sign = diff < 0 ? -1 : 1, milliseconds, seconds, minutes, hours, days;
            diff /= sign; // or diff=Math.abs(diff);
            diff = (diff - (milliseconds = diff % 1000)) / 1000;
            diff = (diff - (seconds = diff % 60)) / 60;
            diff = (diff - (minutes = diff % 60)) / 60;
            days = (diff - (hours = diff % 24)) / 24;
            return {
                'isPassed': sign === 1 ? false : true,
                'inDays': days,
                'inHours': hours,
                'inMinutes': minutes,
                'inSeconds': seconds,
                'inMilliseconds': milliseconds
            };
        },
        notify: function (title, content) {
            SweetSoftScript.Setting.message.Notify(title, content);
        },
        loadInfoProgress: function (id, callback) {
            var curorder = $('#gvdonhang tr.active');
            if (curorder.length > 0) {
                var hdf = curorder.find('input[type="hidden"]');
                if (hdf.length > 0 && hdf.val().length > 0) {
                    SweetSoftScript.commonFunction.showLoading(function () {
                        SweetSoftScript.Data.ajRequest = SweetSoftScript.commonFunction.ajaxGet(
                            SweetSoftScript.Data.isCylinderTimeline ? '/Timeline/info-cylinder.aspx' : '/Timeline/info-job.aspx',
                            'Id=' + hdf.val(), function (response) {
                                if (typeof response !== 'undefined' && response.length > 0) {
                                    if (SweetSoftScript.Data.isCylinderTimeline) {
                                        if (response && response.length > 0) {
                                            $('#gvdetail > tbody tr:gt(0)').remove();
                                            var tbody = $('#gvdetail > tbody');
                                            $.each(response, function (i, o) {
                                                tbody.append('<tr>' +
                                                    '<td>' + o.DepartmentName + '</td>' +
                                                    '<td><span class="btn btn-' + (o.Status.toLowerCase() === 'finish' ? 'success' : (o.Status.toLowerCase() === 'inprogress' ? 'warning' : 'notprocess')) + '"></span></td>' +
                                                    '<td>' + (o.StartedOn || '') + '</td>' +
                                                    '<td>' + (o.FinishedOn || '') + '</td>' +
                                                    '<td>' + (o.MachineName || '') + '</td>' +
                                                    '<td>' + (o.TimeProcess || '') + '</td>');
                                            });
                                        }
                                    }
                                    else {
                                        $('#mainview > table').remove();
                                        var div = $(response);
                                        $('#mainview').append(div.html());
                                    }
                                }
                                else {
                                    $('#mainview > table').remove();
                                }
                                //for test time
                                setTimeout(function () {
                                    SweetSoftScript.commonFunction.hideLoading();
                                }, 400);
                            });
                    });
                }
            }
            else {
                SweetSoftScript.mainFunction.notify(SweetSoftScript.ResourceText.notice, SweetSoftScript.ResourceText.pleaseSelectJob);
            }
        },
        initTrOrder: function () {
            var trColl = $('#gvdonhang tr');
            if (trColl.length > 0) {
                trColl.each(function () {
                    $(this).click(function () {
                        if ($(this).hasClass('active'))
                        { }
                        else {
                            var active = $('#gvdonhang tr.active');
                            if (active.length > 0)
                                active.removeClass('active');

                            $(this).addClass('active');
                        }

                        var dhcode = $.trim($(this).find('td:first > a').text());
                        $('h1').text(SweetSoftScript.Setting.title + ' : ' + dhcode);

                        var hdf = $(this).find('input[type="hidden"]');
                        if (hdf.length > 0 && hdf.val().length > 0) {
                            var curtime = $(this).attr('data-time');
                            if (typeof curtime !== 'undefined' && curtime.length > 0) {
                                var date;
                                try {
                                    date = new Date(curtime);
                                    if (typeof date !== 'undefined') {
                                        var diff = SweetSoftScript.mainFunction.caculateTime(new Date(), date);
                                        if (diff.inMinutes >= -1) {
                                            SweetSoftScript.mainFunction.loadInfoProgress(hdf.val());
                                        }
                                        else {
                                            //get from cache
                                            var statusArr = SweetSoftScript.Data.cache[hdf.val()];
                                            if (statusArr.length > 0) {
                                                $.each(statusArr, function (i, o) {
                                                    var link = $('#maindept td > a[data-id="' + o.Code + '"]');
                                                    if (link.length > 0) {
                                                        link.removeClass();
                                                        if (o.Status === 'dang_xu_ly')
                                                            link.addClass('inprogress');
                                                        else if (o.Status === 'chua_xu_ly')
                                                            link.addClass('notprocess');
                                                        else if (o.Status === 'da_xu_ly')
                                                            link.addClass('processed');
                                                    }
                                                });
                                            }
                                        }
                                    }
                                    else
                                        SweetSoftScript.mainFunction.loadInfoProgress(hdf.val());
                                }
                                catch (ex) {
                                    SweetSoftScript.mainFunction.loadInfoProgress(hdf.val());
                                }
                            }
                            else {
                                SweetSoftScript.mainFunction.loadInfoProgress(hdf.val());
                            }
                        }
                        return false;
                    });
                });
            }
        },
        initMessage: function () {
            SweetSoftScript.Setting.message.container = $("#container-message").notify();
            //message 
            SweetSoftScript.Setting.message.create = function (template, vars, opts) {
                return SweetSoftScript.Setting.message.container.notify("create", template, vars, opts);
            }

            SweetSoftScript.Setting.message.Notify = function (title, content) {
                SweetSoftScript.Setting.message.create("error-message", { title: title, text: content }, { expires: 5000 }); //default : close after 5s
            }
            //message
        },
        refreshGrid: function () {

            var idorder;
            var active = $('#gvdonhang tr.active');
            if (active.length > 0) {
                if (active.find('input[type="hidden"]').length > 0)
                    idorder = active.find('input[type="hidden"]').val();
            }
            var activedept = $('#maindept a.active');
            var datacode = activedept.attr('data-id');

            SweetSoftScript.commonFunction.showLoading(function () {
                SweetSoftScript.Data.ajRequest = SweetSoftScript.commonFunction.ajaxGet('../Timeline/job-list.aspx',
                'act=reloadjob', function (response) {
                    $('#gvdonhang').replaceWith($(response).children(':eq(0)'));
                    $('#maindonhang .counttotal').text($('#gvdonhang').attr('datacount'));
                    $('#gvdonhang').closest('.innerb').mCustomScrollbar('update');
                    setTimeout(function () {
                        SweetSoftScript.mainFunction.initTrOrder();

                        if (typeof idorder !== 'undefined') {
                            var inputHdf = $('input[type="hidden"][value="' + idorder + '"]');
                            if (inputHdf.length > 0) {
                                inputHdf.closest('tr').addClass('active');

                                activedept = $('#maindept a[data-id="' + datacode + '"]');
                                if (activedept.length > 0) {
                                    SweetSoftScript.mainFunction.loadInfoProgress(inputHdf.val(), function () {
                                        SweetSoftScript.mainFunction.loadInfoDept(activedept);
                                    });
                                }
                                else
                                    SweetSoftScript.commonFunction.hideLoading();
                            }
                            else
                                SweetSoftScript.commonFunction.hideLoading();
                        }
                        else {
                            activedept = $('#maindept a[data-id="' + datacode + '"]');
                            if (activedept.length > 0) {
                                SweetSoftScript.mainFunction.loadInfoProgress(inputHdf.val(), function () {
                                    SweetSoftScript.mainFunction.loadInfoDept(activedept);
                                });
                            }
                            else
                                SweetSoftScript.commonFunction.hideLoading();
                        }
                    }, 500);
                });
            });
        },
        init: function () {
            /// <summary>Init on page load</summary>

            //scroll left
            $('#gvdonhang').closest('.innerb').mCustomScrollbar({
                scrollButtons: { enable: true },
                theme: "dark-thick"
            });

            SweetSoftScript.Setting.title = $('h1').text();

            SweetSoftScript.mainFunction.initTrOrder();

            /*
            SweetSoftScript.mainFunction.initMessage();

            $('#btnrefresh').click(function () {
                SweetSoftScript.mainFunction.refreshGrid();
                return false;
            });
            */
        }
    }
}

$(function () {
    SweetSoftScript.mainFunction.init();
    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SweetSoftScript.mainFunction.init);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitFilter);
    }
});
