//http://fstoke.me/jquery/window/
$(document).ready(function () {
    setTimeout(function () {
        $.window.prepare({
            dock: 'bottom',
            dockArea: $('#dockwindowpanel'),
            dock: 'bottom',       // change the dock direction: 'left', 'right', 'top', 'bottom'
            animationSpeed: 200,  // set animation speed
            minWinLong: 180
        });
    }, 500)


})

var customButton = [
{
    id: "btn_refresh",
    title: "refresh page",
    clazz: "refreshImg",
    image: "img/refreshImg.png",
    callback: function (btn, wnd) {
        var f = wnd.getContainer().find('iframe');
        f[0].contentWindow.location.reload(true);
    }
}
];
$('.window').each(function () {
    $(this).click(function () {
        var _this = $(this);
        var title = $(this).data('title');
        var url = $(this).data('href');
        var show = $(this).attr('data-show');
        if (show === "false") {
            openWindow(_this, title, url);
        }
    })
})

function openWindow(_this, title, url) {
    $('#relative').window({
        title: title,
        url: url,
        bookmarkable: false,
        checkBoundary: true,
        width: 900,
        height: 400,
        custBtns: customButton,
        iframeRedirectCheckMsg: "the window is going to redirect to " + url + "!!\r\nPlease select 'cancel' to stay here.",
        onOpen: function (wnd) {
            _this.attr('data-show', 'true');
            //var allWindow = $.window.getAll();
            //$.each(allWindow, function (i, o) {
            //    if (o.isMinimized()===false && o.isMaximized()) {
            //        o.minimize();
            //    }
            //});
            setTimeout(function () {
                wnd.maximize();
            }, 300)

        },
        onShow: function (wnd) {
        },
        onClose: function () {
            _this.attr('data-show', 'false');
        },
        afterMaximize: function (wnd) {
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
        },
        onCascade: function (wnd) {
        }
    });

}
