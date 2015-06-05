(function($){
    $.fn.validationEngineLanguage = function(){
    };
    $.validationEngineLanguage = {
        newLang: function(){
            $.validationEngineLanguage.allRules = {
                "required": { // Add your regex rules here, you can take telephone as an example
                    "regex": "none",
                    "alertText": " <li> Bắt buộc nhập dữ liệu trường này !</li>",
                    "alertTextCheckboxMultiple": "<li> Vui lòng chọn một tùy chọn</li>",
                    "alertTextCheckboxe": "<li> Checkbox này bắt buộc</li>",
                    "alertTextDateRange": "<li> Cả hai trường ngày tháng đều bắt buộc</li>"
                },
                "requiredInFunction": { 
                    "func": function(field, rules, i, options){
                        return (field.val() == "test") ? true : false;
                    },
                    "alertText": "<li> Giá trị của trường phải là test</li>"
                },
                "dateRange": {
                    "regex": "none",
                    "alertText": "<li> Không đúng </li>",
                    "alertText2": "Khoảng ngày tháng"
                },
                "dateTimeRange": {
                    "regex": "none",
                    "alertText": "<li> Không đúng </li>",
                    "alertText2": "Khoảng thời gian"
                },
                "minSize": {
                    "regex": "none",
                    "alertText": "<li> Tối thiểu </li>",
                    "alertText2": " số ký tự được cho phép"
                },
                "maxSize": {
                    "regex": "none",
                    "alertText": "<li> Tối đa </li>",
                    "alertText2": " số ký tự được cho phép"
                },
				"groupRequired": {
                    "regex": "none",
                    "alertText": "<li> Bạn phải điền một trong những trường sau</li>"
                },
                "min": {
                    "regex": "none",
                    "alertText": "<li> Giá trị nhỏ nhất là </li>"
                },
                "max": {
                    "regex": "none",
                    "alertText": "<li> Giá trị lớn nhất là </li>"
                },
                "past": {
                    "regex": "none",
                    "alertText": "<li> Ngày kéo dài tới </li>"
                },
                "future": {
                    "regex": "none",
                    "alertText": "<li> Ngày đã qua </li>"
                },	
                "maxCheckbox": {
                    "regex": "none",
                    "alertText": "<li> Tối đa </li>",
                    "alertText2": " số tùy chọn được cho phép"
                },
                "minCheckbox": {
                    "regex": "none",
                    "alertText": "<li> Vui lòng chọn </li>",
                    "alertText2": " các tùy chọn"
                },
                "equals": {
                    "regex": "none",
                    "alertText": "<li> Giá trị các trường không giống nhau</li>"
                },
                "creditCard": {
                    "regex": "none",
                    "alertText": "<li> Số thẻ tín dụng sai</li>"
                },
                "phone": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([\+][0-9]{1,3}[\ \.\-])?([\(]{1}[0-9]{2,6}[\)])?([0-9\ \.\-\/]{3,20})((x|ext|extension)[\ ]?[0-9]{1,4})?$/,
                    "alertText": "<li> Số điện thoại sai"
                },
                "email": {
                    // HTML5 compatible email regex ( http://www.whatwg.org/specs/web-apps/current-work/multipage/states-of-the-type-attribute.html#    e-mail-state-%28type=email%29 )
                    "regex": /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/,
                    "alertText": "<li> Địa chỉ thư điện tử sai"
                },
                "integer": {
                    "regex": /^[\-\+]?\d+$/,
                    "alertText": "<li> Không đúng là số nguyên</li>"
                },
                "number": {
                    // Number, including positive, negative, and floating decimal. credit: orefalo
                    "regex": /^[\-\+]?((([0-9]{1,3})([,][0-9]{3})*)|([0-9]+))?([\.]([0-9]+))?$/,
                    "alertText": "<li> Không đúng là số thập phân</li>"
                },
                "date": {
                    "regex": /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/,
                    "alertText": "<li> Ngày sai, phải có định dạng dd/MM/yyyy</li>"
                },
                "ipv4": {
                    "regex": /^((([01]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))[.]){3}(([0-1]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))$/,
                    "alertText": "<li> Địa chỉ IP sa</li>i"
                },
                "url": {
                    "regex": /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i,
                    "alertText": "<li> URL sai</li>"
                },
                "onlyNumberSp": {
                    "regex": /^[0-9\ ]+$/,
                    "alertText": "<li> Chỉ điền số</li>"
                },
                "onlyLetterSp": {
                    "regex": /^[a-zA-Z\ \']+$/,
                    "alertText": "<li> Chỉ điền chữ</li>"
                },
                "onlyLetterNumber": {
                    "regex": /^[0-9a-zA-Z]+$/,
                    "alertText": "<li> Không được chứa ký tự đặc biệt</li>"
                },
                // --- CUSTOM RULES -- Those are specific to the demos, they can be removed or changed to your likings
                "ajaxUserCall": {
                    "url": "ajaxValidateFieldUser",
                    // you may want to pass extra data on the ajax call
                    "extraData": "name=eric",
                    "alertText": "<li> Tên này được dùng</li>",
                    "alertTextLoad": "<li> Đang xác nhận, vui lòng chờ</li>"
                },
				"ajaxUserCallPhp": {
                    "url": "phpajax/ajaxValidateFieldUser.php",
                    // you may want to pass extra data on the ajax call
                    "extraData": "name=eric",
                    // if you provide an "alertTextOk", it will show as a green prompt when the field validates
                    "alertTextOk": "<li> Tên người dùng này có thể dùng được</li>",
                    "alertText": "<li> Tên người dùng này đã được sử dụng</li>",
                    "alertTextLoad": "<li> Đang xác nhận, vui lòng chờ</li>"
                },
                "ajaxNameCall": {
                    // remote json service location
                    "url": "ajaxValidateFieldName",
                    // error
                    "alertText": "<li> Tên này được dùng</li>",
                    // if you provide an "alertTextOk", it will show as a green prompt when the field validates
                    "alertTextOk": "<li> Tên này có thể dùng</li>",
                    // speaks by itself
                    "alertTextLoad": "<li> Đang xác nhận, vui lòng chờ</li>"
                },
				 "ajaxNameCallPhp": {
	                    // remote json service location
	                    "url": "phpajax/ajaxValidateFieldName.php",
	                    // error
	                    "alertText": "<li> Tên này được dùng</li>",
	                    // speaks by itself
	                    "alertTextLoad": "<li> Đang xác nhận, vui lòng chờ</li>"
	                },
                "validate2fields": {
                    "alertText": "<li> Vui lòng nhập vào HELLO</li>"
                },
	            //tls warning:homegrown not fielded 
                "dateFormat":{
                    "regex": /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$|^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$/,
                    "alertText": "<li> Ngày sai"
                },
                //tls warning:homegrown not fielded 
				"dateTimeFormat": {
	                "regex": /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}\s+(am|pm|AM|PM){1}$|^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^((1[012]|0?[1-9]){1}\/(0?[1-9]|[12][0-9]|3[01]){1}\/\d{2,4}\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}\s+(am|pm|AM|PM){1})$/,
                    "alertText": "<li> Ngày sai hoặc định dạng ngày sai</li>",
                    "alertText2": "Định dạng đúng là: ",
                    "alertText3": "mm/dd/yyyy hh:mm:ss AM|PM hay ", 
                    "alertText4": "yyyy-mm-dd hh:mm:ss AM|PM"
	            }
            };
            
        }
    };

    $.validationEngineLanguage.newLang();
    
})(jQuery);
