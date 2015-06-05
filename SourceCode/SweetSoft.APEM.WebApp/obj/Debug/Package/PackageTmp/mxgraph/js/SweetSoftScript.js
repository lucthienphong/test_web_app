var SweetSoftScript = {
    Setting: {
        MAX_REQUEST_SIZE: 50000,
        SAVE_URL: '',
        agreeBtnId: '',
        closeBtnId: '',
        saveBtnId: '',
        wdId: '#wfMessage',
        labelMessage: '',
        colorSubLine: '#e43ed2',
        colorBothType: '#0D8517',
        reset_workflow: false,
        add_main_workflow: false,
        delete_workflow: false
    },
    Data: {
        fileColl: [],
        defTitle: '',
        defProcessBtnValue: '',
        defCancelBtnValue: ''
    },
    ResourceText: undefined,
    commonFunction: {
        initLoad: function () {

            var timewd = setInterval(function () {
                var wd = $(SweetSoftScript.Setting.wdId);

                if (wd !== null) {
                    SweetSoftScript.Data.defTitle = wd.find('h4[id$="lblMessageTitle"]').text();
                    window.clearInterval(timewd);
                }
            }, 500);


            /*
            var saveButton = document.getElementById('saveButton');
            mxEvent.addListener(saveButton, 'click', function(evt) {
            SweetSoftScript.mainFunction.OpenRadWindow();
            });
            */

            var agreeBtn = document.getElementById(SweetSoftScript.Setting.agreeBtnId);
            if (agreeBtn !== null) {
                SweetSoftScript.Data.defProcessBtnValue = agreeBtn.value;
                agreeBtn.removeAttribute('onclick');
                mxEvent.addListener(agreeBtn, 'click', function (evt) {
                    evt.preventDefault();
                    SweetSoftScript.mainFunction.CloseConfirmWindow();
                    return false;
                });
            }

            var saveBtn = document.getElementById(SweetSoftScript.Setting.saveBtnId);
            if (saveBtn !== null) {
                saveBtn.removeAttribute('onclick');
                mxEvent.addListener(saveBtn, 'click', function (evt) {
                    evt.preventDefault();
                    SweetSoftScript.mainFunction.CloseConfirmWindow();
                    return false;
                });
            }

            var closeBtn = document.getElementById(SweetSoftScript.Setting.closeBtnId);
            if (closeBtn !== null) {
                SweetSoftScript.Data.defCancelBtnValue = closeBtn.value;
                closeBtn.removeAttribute('onclick');
                mxEvent.addListener(closeBtn, 'click', function (evt) {
                    evt.preventDefault();
                    SweetSoftScript.mainFunction.CloseConfirmWindow();
                    return false;
                });
            }
        },
        include: function (url) {
            document.write('<script src="' + url + '" type="text/javascript"></script>');
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
        BindConfirmDelete: function (btn) {
            mxEvent.removeListener(btn, 'click');
        },
        BindConfirmNotSave: function (btn) {
            mxEvent.removeListener(btn, 'click');
        }
    },
    mainFunction: {
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
        OpenRadWindow: function (title, message, type, dataAction1, dataAction2) {
            var wd = $(SweetSoftScript.Setting.wdId);
            var btnCancel = null;
            if (wd.length > 0) {
                //show or hide process button
                var btn1 = document.getElementById(SweetSoftScript.Setting.agreeBtnId);
                var btn2 = document.getElementById(SweetSoftScript.Setting.saveBtnId);
                if (btn1 !== null) {
                    if (typeof type !== 'undefined') {
                        switch (type) {
                            case 'alert':
                                btn1.style.display = 'none';
                                btn2.style.display = 'none';
                                btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = mxResources.get('ok');
                                break;
                            case 'confirmDelete':
                                btn1.style.display = 'inline-block';
                                btn2.style.display = 'none';
                                if (typeof dataAction1 !== 'undefined') {
                                    mxEvent.removeAllListeners(btn1);
                                    mxEvent.addListener(btn1, 'click', function (evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = SweetSoftScript.Data.defCancelBtnValue;
                                if (typeof dataAction2 !== 'undefined') {
                                    mxEvent.removeAllListeners(btnCancel);
                                    mxEvent.addListener(btnCancel, 'click', function (evt) {
                                        evt.preventDefault();
                                        dataAction2();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                break;
                            case 'confirmResetDesign':
                                btn1.style.display = 'inline-block';
                                btn2.style.display = 'none';
                                btn1.value = SweetSoftScript.ResourceText.btnApply;

                                if (typeof dataAction1 !== 'undefined') {
                                    mxEvent.removeAllListeners(btn1);
                                    mxEvent.addListener(btn1, 'click', function (evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = SweetSoftScript.Data.defCancelBtnValue;
                                if (typeof dataAction2 !== 'undefined') {
                                    mxEvent.removeAllListeners(btnCancel);
                                    mxEvent.addListener(btnCancel, 'click', function (evt) {
                                        evt.preventDefault();
                                        dataAction2();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }
                                break;
                            case 'confirmCancelSave':
                                btn1.style.display = 'inline-block';
                                btn2.style.display = 'inline-block';
                                btn1.value = 'Có';
                                btn2.value = 'Không';
                                if (typeof dataAction2 !== 'undefined') {
                                    mxEvent.removeAllListeners(btn1);
                                    mxEvent.addListener(btn1, 'click', function (evt) {
                                        evt.preventDefault();
                                        dataAction2();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                if (typeof dataAction1 !== 'undefined') {
                                    mxEvent.removeAllListeners(btn2);
                                    mxEvent.addListener(btn2, 'click', function (evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = SweetSoftScript.Data.defCancelBtnValue;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (message.length > 0) {
                    $('span[id$="lbConfirmTitle"]').text(message).parent().show();
                }
                else {
                    $('span[id$="lbConfirmTitle"]').text('').parent().hide();
                }

                wd.modal('show');

                //title of window
                if (typeof title !== 'undefined' && title.length > 0)
                    $('h4[id$="lblMessageTitle"]').text(title);
                else
                    $('h4[id$="lblMessageTitle"]').text(SweetSoftScript.Data.defTitle);

                //console.log(btn1, btn2, btnCancel);
                if (btn1 !== null && btn1.style.display !== 'none') {
                    btn1.className = 'focus btn btn-default';
                    btn1.focus();
                    if (btnCancel !== null)
                        btnCancel.className = 'btn btn-default';
                }
                else if (btnCancel !== null && btnCancel.style.display !== 'none') {
                    btnCancel.className = 'focus btn btn-default';
                    btnCancel.focus();
                    if (btn1 !== null)
                        btn1.className = 'btn btn-default';
                }
            }
        },
        CloseConfirmWindow: function () {
            var radWindowConfirm = $(SweetSoftScript.Setting.wdId);
            if (radWindowConfirm.length > 0) {
                radWindowConfirm.modal('hide');
            }
            return false;
        }
    }
}