function showMessageBox(id) {
    /*@cc_on @*/
    $(id).modal('show');
}

function closeMessageBox(id) {
    /*@cc_on @*/
    $(id).modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}

function OpenDiaglog(link) {
    parent.$('a[data-href="' + link + '"]').click();
}