function ShowError(idelement, message, focus) {
    if (typeof idelement !== 'undefined' && idelement !== null && idelement.length > 0) {
        var input = $('#' + idelement);
        if (input.length > 0) {
            console.log(input);
            if (typeof message === 'undefined' || message === null || message.length === 0)
                message = 'Có lỗi ở đây.';
            var error = $('<span>' + message + '</span>');
            if (input.attr('data-iconerror') === 'left')
                error.css('left', '-5px');
            if (input.parent().hasClass('input-group'))
                error.appendTo(input.parent().parent().css('position', 'relative'));
            else
                error.appendTo(input.parent().css('position', 'relative'));
            console.log(error);
            error.addClass('imgerror')
                .attr('title', error.text())
                //.tooltip({ trigger: 'focus click' });
                .qtip({
                    content: {
                        text: function (event, api) {
                            return $(this).text();
                        }
                    },
                    show: {
                        event: 'mouseenter click',
                        solo: true
                    },
                    position: {
                        my: 'left center',
                        at: 'right center',
                        viewport: $(window)
                    },
                    hide: {
                        event: 'unfocus mouseleave', fixed: true, delay: 1500
                        //when: { event: 'unfocus click mouseleave' }, fixed: true, delay: 1500
                    },
                    style: {
                        classes: 'qtip-light qtip-red'
                    },
                    events: {
                        show: function (event, api) {
                            //var $el = $(api.elements.target[0]);
                            //console.log($el.offset().left,($el.width()/2));
                            //var div = $(api.elements.tooltip);                                   
                        }
                    }
                });

            if (focus && focus === true)
                input.focus().trigger('focusin');
        }
    }
}

function AttackValidate() {
    if (typeof $.fn.validate === 'undefined')
        return;
    $("form").unbind();
    $("form").validate({
        ignore: [],
        highlight: function (element) {
            var error = $(element).closest('.form-group');
            error.removeClass('has-success').addClass('has-error');
        },
        unhighlight: function (element) {
            var error = $(element).closest('.form-group');
            error.removeClass('has-error');
            var sp = error.find('.imgerror');
            if (sp.length > 0) {
                sp.remove();
            }
        },
        errorPlacement: function (error, element) {
            if (element.attr('data-iconerror') === 'left')
                error.css('left', '-5px');
            if (element.parent().hasClass('input-group'))
                error.appendTo(element.parent().parent().css('position', 'relative'));
            else
                error.appendTo(element.parent().css('position', 'relative'));
            error.addClass('imgerror')
                .attr('title', error.text())
                //.tooltip({ trigger: 'focus click' });
                .qtip({
                    content: {
                        text: function (event, api) {
                            return $(this).text();
                        }
                    },
                    show: {
                        event: 'mouseenter click',
                        solo: true
                    },
                    position: {
                        my: 'left center',
                        at: 'right center',
                        viewport: $(window)
                    },
                    hide: {
                        event: 'unfocus mouseleave', fixed: true, delay: 1500
                        //when: { event: 'unfocus click mouseleave' }, fixed: true, delay: 1500
                    },
                    style: {
                        classes: 'qtip-light qtip-red'
                    },
                    events: {
                        show: function (event, api) {
                            //var $el = $(api.elements.target[0]);
                            //console.log($el.offset().left,($el.width()/2));
                            //var div = $(api.elements.tooltip);
                        }
                    }
                });
        },
        invalidHandler: function (event, validator) {
            //console.log(event, validator.numberOfInvalids(), validator);
            var errors = validator.numberOfInvalids();
            if (errors) {
                //console.log(errors, validator.errorList[0]);
                var div = $(validator.errorList[0].element).closest('.tab-pane');
                if (!div.hasClass('active')) {
                    var id = div.attr("id");
                    $('a[href="#' + id + '"]').tab('show');
                }
                if ($(validator.errorList[0].element).is('select'))
                    $(validator.errorList[0].element).select2('open');
                else
                    $(validator.errorList[0].element).focus();
            }
        }
    });
}