/// <reference path="jquery-1.11.1.min.js" />
(function ($) {
    jQuery.expr[':'].Contains = function (a, i, m) {
        return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
    };
}(jQuery));


function IsMerge(indx) {
    var ismerge = false;
    var other = $('#trfilter td>input[type="text"]');
    if (other.length > 0) {
        other.each(function (i, o) {
            if (i === (indx - 1)) {
                //console.log('ignore : ', i, indx);
            }
            else {
                var val = $(this).val();
                if (val.length > 0) {
                    ismerge = true;
                    return false;
                }
            }
        });
    }

    return ismerge;
}

function GetFilter(filter, indx) {
    var list = $("#gvdonhang");
    var ss = '';
    if (indx === 3)
        ss = 'td' + ':nth-child(' + indx + '):Contains(' + filter + '),td' + ':nth-child(' + (indx + 1) + '):Contains(' + filter + ')';
    else
        ss = 'td' + ':nth-child(' + indx + '):Contains(' + filter + ')';
    //console.log('ss 1 : ', ss);
    var matches = list.find(ss).parent();
    //console.log(matches);
    if (matches.length > 0) {
        var other = $('#trfilter td>input[type="text"]');
        if (other.length > 0) {
            other.each(function (i, o) {
                if (matches.length === 0)
                    return false;

                if (i + 1 === indx) {

                }
                else {
                    var val = $(o).val();
                    if (val.length > 0) {
                        ss = '';
                        if (indx === 3)
                            ss = 'td' + ':nth-child(' + (i + 1) + '):Contains(' + val + '),td' + ':nth-child(' + (i + 2) + '):Contains(' + val + ')';
                        else
                            ss = 'td' + ':nth-child(' + (i + 1) + '):Contains(' + val + ')';
                        //console.log('ss 2 : ', ss);
                        matches = matches.find(ss).parent();
                    }
                }
            });
        }
    }
    return matches;
}

function ZebraTable() {
    $("#gvdonhang tr:visible").each(function (i) {
        if (i % 2 === 0)
            $(this).removeClass().addClass('odd');
        else
            $(this).removeClass().addClass('even');
    });
}

function filterList(input, indx) {
    var list = $("#gvdonhang");
    //console.log(input);
    $(input)
      .change(function () {
          var filter = $(this).val();
          var $matche = [];
          if (filter.length > 0) {
              $matche = GetFilter(filter, indx);
          }
          else {
              var other = $('#trfilter td>input[type="text"]');
              if (other.length > 0) {
                  other.each(function (i, o) {
                      var val = $(o).val();
                      if (val.length > 0) {
                          if (i + 1 === indx) {
                              //console.log('ignore : ', i + 1, indx);
                          }
                          else {
                              if ($matche.length === 0)
                                  $matche = GetFilter(val, i + 1);
                              else
                                  $matche = $matche.filter(':Contains(' + val + ')');
                          }
                      }
                  });
              }
          }
          //console.log('$matche 2: ', $matche.length);
          if ($matche.length > 0) {
              //console.log('ss : ', ss);
              $('#maindonhang .counttotal').text($matche.length);
              $('tr', list).not($matche).hide();
              $matche.show();
              $('#gvdonhang').closest('.innerb').mCustomScrollbar('update');
          }
          else {
              var ismerge = IsMerge(indx);
              if (ismerge || (ismerge === false && filter.length > 0)) {
                  $('#maindonhang .counttotal').text($matche.length);
                  $(list).find("tr:visible").hide();
                  $('#gvdonhang').closest('.innerb').mCustomScrollbar('update');
              }
              else {
                  $('#maindonhang .counttotal').text($('#maindonhang .counttotal').attr('data-count'));
                  $(list).find("tr").show();
                  $('#gvdonhang').closest('.innerb').mCustomScrollbar('update');
              }
          }

          ZebraTable();

          return false;
      })
    .keyup(function () {
        $(this).change();
    });
}

function InitFilter() {
    ZebraTable();
    var inpColl = $('#trfilter td>input[type="text"]');
    if (inpColl.length > 0) {
        inpColl.each(function (indx) {
            filterList(this, (indx + 1));
        });
        $('#maindonhang .counttotal').attr('data-count', $('#maindonhang .counttotal').text());
    }
}

$(function () {
    InitFilter();
});
