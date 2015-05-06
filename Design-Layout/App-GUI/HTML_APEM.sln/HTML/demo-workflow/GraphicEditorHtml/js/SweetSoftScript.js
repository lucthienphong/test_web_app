var SweetSoftScript = {
    Setting: {
        MAX_REQUEST_SIZE: 50000,
        SAVE_URL: '',
        agreeBtnId: '',
        closeBtnId: '',
        saveBtnId: '',
        wdId: '',
        labelMessage: '',
        colorSubLine: '#e43ed2'
    },
    Data: {
        fileColl: [],
        defTitle: '',
        defProcessBtnValue: '',
        defCancelBtnValue: '',
        notifyTitle: 'Thông báo'
    },
    ResourceText: {
        mainworkTitle: 'Bản thiết kế chính',
        ghinhanThongSo: 'Ghi nhận thông số sản xuất',
        btnShowRecord: 'Ghi nhận thông số',
        btnInfo: 'Thông tin đối tượng',
        btnCheckAll: 'Chọn tất cả',
        btnUnCheckAll: 'Bỏ chọn tất cả',
        btnSaveRecord: 'Đồng ý',
        btnSelect: 'Chọn',
        btnSelectAll: 'Chọn tất cả',
        btnApply: 'Đồng ý',
        btnRemoveSelect: 'Bỏ chọn',
        btnDetail: 'Chi tiết',
        btnAddSubwork: 'Thành phần phụ',
        rdoLineMain: 'Đường chuyển chính',
        rdoLineSub: 'Đường chuyển phụ',
        worktask: 'công việc chính',
        confirmLostData: 'Bản thiết kế này đã có sự thay đổi, bạn có muốn lưu không ?',
        processChangeDeptorWork: 'Chuyển công tác',
        changeDeptorWorkDone: 'Chuyển công tác thành công.',
        confirmDeleteLine: 'Khi xóa đường chuyển sẽ ảnh hưởng đến tiến trình ghi nhận và sản xuất bên trong.',
        cannotDeleteMultiNode: 'Không thể xóa nhiều công việc hoặc phòng ban!',
        dept: 'bộ phận',
        start: 'Đầu vào',
        end: 'Đầu ra',
        subwork: 'công việc phụ',
        worktaskofSubwork: 'Công việc liên quan',     
        mustEnterSubworkName: 'Thành phần phụ phải có tên.<br/>Bạn có chắc chắn muốn xóa thành phần phụ này ?',      
        titleOutline: 'Tổng thể',
        //info window
        edge: 'Qui cách đường chuyển',
        vertex: 'Qui cách thành phần',
        groupSubwork: 'Nhóm công việc phụ',
        groupWorktask: 'Công việc chính',
        groupDept: 'Bộ phận',
        startPlace: 'Nơi bắt đầu :',
        endPlace: 'Nơi kết thúc :',
        xulytheoong: 'Xử lý theo ống :',
        doesnot: 'Không có',
        ComponentName: 'Tên thành phần :',
        ComponentType: 'Loại thành phần :',
        relatedWork: 'Công việc liên quan :',
        relatedMainStep: 'Bước chính liên quan :',
        //info window
        listWorktask: 'Danh sách công việc',
        listDept: 'Danh sách phòng ban',
        listMachinery: 'Danh sách trục',
        listGraphictype: 'Danh sách loại mẫu đồ họa',
        notHaveModifyRight: 'Bạn không thể chỉnh sửa bản thiết kế vì chưa được phân quyền.',
        resetThisDesign: 'Làm lại bản thiết kế này',
        createProductionButton: 'Tạo ghi nhận sản xuất',
        confirmResetThisDesign: 'Bạn có chắc chắn sẽ làm lại bản thiết kế này ?',
        viewOutlineDesign: 'Xem tổng thể thiết kế',
        saveThisDesign: 'Lưu màn hình làm việc hiện tại',
        backToMainWork: 'Trở về màn hình chính',
        cannotDeleteStartAndEnd: 'Thành phần ở bước đầu và bước cuối không thể bị xóa.',
        errorWhileProcess: 'Đã xảy ra lỗi trong quá trình thực hiện.'
    },
    commonFunction: {
        initLoad: function () {

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
                mxEvent.addListener(agreeBtn, 'click', function(evt) {
                    evt.preventDefault();
                    SweetSoftScript.mainFunction.CloseConfirmWindow();
                    return false;
                });
            }

            var saveBtn = document.getElementById(SweetSoftScript.Setting.saveBtnId);
            if (saveBtn !== null) {
                saveBtn.removeAttribute('onclick');
                mxEvent.addListener(saveBtn, 'click', function(evt) {
                    evt.preventDefault();
                    SweetSoftScript.mainFunction.CloseConfirmWindow();
                    return false;
                });
            }

            var closeBtn = document.getElementById(SweetSoftScript.Setting.closeBtnId);
            if (closeBtn !== null) {
                SweetSoftScript.Data.defCancelBtnValue = closeBtn.value;
                closeBtn.removeAttribute('onclick');
                mxEvent.addListener(closeBtn, 'click', function(evt) {
                    evt.preventDefault();
                    SweetSoftScript.mainFunction.CloseConfirmWindow();
                    return false;
                });
            }
        },
        include: function(url) {
            document.write('<script src="' + url + '" type="text/javascript"></script>');
        },
        getIEVersion: function() {
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
        includeToHead: function(url) {
            var head = document.getElementsByTagName('head')[0];
            var script = document.createElement('script'); script.type = 'text/javascript';
            script.src = url;
            head.appendChild(script);
        },
        preloadLoadImage: function(imgSrc, callback) {
            var imgPreload = new Image();
            imgPreload.src = imgSrc;
            //console.log(imgSrc);
            if (imgPreload.complete) {
                callback(imgPreload);
            }
            else {
                imgPreload.onload = function() {
                    callback(imgPreload);
                }
                imgPreload.onerror = function() {
                    callback(undefined);
                }
            }
        },
        sortObjectProperties: function(obj) {
            var arr = [];
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    arr.push({
                        'key': prop,
                        'value': obj[prop]
                    });
                }
            }
            arr.sort(function(a, b) {
                return (a.value.localeCompare(b.value));
            });
            return arr;
        },
        BindConfirmDelete: function(btn) {
            mxEvent.removeListener(btn, 'click');
        },
        BindConfirmNotSave: function(btn) {
            mxEvent.removeListener(btn, 'click');
        }
    },
    mainFunction: {
        getWindowSize: function() {
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
        DefaultTitle: function() {
            return $find(SweetSoftScript.Setting.wdId)._title;
        },
        OpenRadWindow: function(title, message, type, dataAction1, dataAction2) {
            var wd = $find(SweetSoftScript.Setting.wdId);
            //console.log(wd,title);
            if (wd !== null) {
                //show or hide process button
                var btn1 = document.getElementById(SweetSoftScript.Setting.agreeBtnId);
                var btn2 = document.getElementById(SweetSoftScript.Setting.saveBtnId);
                if (btn1 !== null) {
                    if (typeof type !== 'undefined') {
                        switch (type) {
                            case 'alert':
                                btn1.style.display = 'none';
                                btn2.style.display = 'none';
                                var btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = '  ' + mxResources.get('ok');
                                break;
                            case 'confirmDelete':
                                btn1.style.display = 'inline-block';
                                btn2.style.display = 'none';
                                if (typeof dataAction1 !== 'undefined') {
                                    mxEvent.removeAllListeners(btn1);
                                    mxEvent.addListener(btn1, 'click', function(evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                var btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = SweetSoftScript.Data.defCancelBtnValue;
                                if (typeof dataAction2 !== 'undefined') {
                                    mxEvent.removeAllListeners(btnCancel);
                                    mxEvent.addListener(btnCancel, 'click', function(evt) {
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
                                    mxEvent.addListener(btn1, 'click', function(evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                var btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = SweetSoftScript.Data.defCancelBtnValue;
                                if (typeof dataAction2 !== 'undefined') {
                                    mxEvent.removeAllListeners(btnCancel);
                                    mxEvent.addListener(btnCancel, 'click', function(evt) {
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
                                    mxEvent.addListener(btn1, 'click', function(evt) {
                                        evt.preventDefault();
                                        dataAction2();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                if (typeof dataAction1 !== 'undefined') {
                                    mxEvent.removeAllListeners(btn2);
                                    mxEvent.addListener(btn2, 'click', function(evt) {
                                        evt.preventDefault();
                                        dataAction1();
                                        SweetSoftScript.mainFunction.CloseConfirmWindow();
                                    });
                                }

                                var btnCancel = document.getElementById(SweetSoftScript.Setting.closeBtnId);
                                if (btnCancel !== null)
                                    btnCancel.value = SweetSoftScript.Data.defCancelBtnValue;
                                break;
                            default:
                                break;
                        }
                    }
                }

                //message
                var lb = SweetSoftScript.Setting.labelMessage;
                if (typeof message !== 'undefined' && message.length > 0 && typeof lb !== 'undefined') {
                    document.getElementById(lb).style.display = 'block';
                    document.getElementById(lb).innerHTML = message;
                }
                else {
                    document.getElementById(lb).style.display = 'none';
                    document.getElementById(lb).innerHTML = '';
                }
                //title of window
                if (typeof title !== 'undefined' && title.length > 0)
                    wd.set_title(title);
                else
                    wd.set_title(SweetSoftScript.Data.defTitle);

                //console.log(title, message, type, dataAction);

                if (wd.isClosed() === true)
                    wd.show();
                else {
                }
                if (btn1 !== null && btn1.style.display !== 'none') {
                    btn1.className = 'focus btnIcon fa-2x';
                    btn1.focus();
                    if (btnCancel !== null)
                        btnCancel.className = 'btnIcon fa-2x';
                }
                else if (btnCancel !== null && btnCancel.style.display !== 'none') {
                    btnCancel.className = 'focus btnIcon fa-2x';
                    btnCancel.focus();
                    if (btn1 !== null)
                        btn1.className = 'btnIcon fa-2x';
                }
            }
        },
        CloseConfirmWindow: function() {
            var radWindowConfirm = $find(SweetSoftScript.Setting.wdId);
            if (radWindowConfirm !== null) {
                radWindowConfirm.close();
            }
            return false;
        }
    }
}