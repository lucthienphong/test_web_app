
var i = 0;
; (function ($, window, document, undefined) {

   var demos = {

        dtTableTools: function (target) {

            if ($.fn.dataTable) {

                target.dataTable({
                    "sDom": "<'dt_header'<'row'<'col-md-12 col-sm-12'T>>r>t",
                    //"sDom": "<'dt_header'<'row'<'col-md-4 col-sm-4'l><'col-md-12 col-sm-12'T>>r>t<'dt_footer'<'row'<'col-md-6 col-sm-6'i><'col-md-6 col-sm-6'p>>>",
                    "oTableTools": {
                        "sSwfPath": "js/plugins/datatables/TableTools/swf/copy_csv_xls_pdf.swf",
                        "aButtons": [
							//{
							//	"sExtends": "copy", 
							//	"sButtonText": '<i class="icol-clipboard-text"></i> Copy'
							//}, 
							//{
							//	"sExtends": "csv", 
							//	"sButtonText": '<i class="icol-doc-excel-csv"></i> CSV'
							//}, 
							//{
							//	"sExtends": "xls", 
							//	"sButtonText": '<i class="icol-doc-excel-table"></i> Excel'
							//}, 							
							//{
							//	"sExtends": "pdf", 
							//	"sButtonText": '<i class="icol-doc-pdf"></i> PDF'
							//}, 
							//{
							//	"sExtends": "print", 
							//	"sButtonText": '<i class="icol-printer"></i> Print'
							//},
                            {
                                "sExtends": "add",
                                "sButtonText": '<i class="icol-add"></i> Add',
                                "fnClick": function () {
                                    //chi ap dung cho demo
                                    var table = $(this)[0].dom.table;
                                    i++;
                                    if (table) {
                                        var newRow = '<tr class="newRow_' + i + '"><td class="col1"></td> <td class="col2"></td> <td class="col3" colspan="2"></td></tr>';
                                        $(table).find('tbody').prepend(newRow);
                                        $(table).find('.newRow_' + i + '').find('.col1').append('<input type="text" class="form-control" placeholder="Contact name" />')
                                        $(table).find('.newRow_' + i + '').find('.col2').append('<input type="text" class="form-control" placeholder="title" style="width:80px;" />')
                                        $(table).find('.newRow_' + i + '').find('.col3').append('<input type="text" class="form-control" placeholder="Designation" style="width:100%;"/>')
                                    }
                                }
                            },
                             {
                                 "sExtends": "save",
                                 "sButtonText": '<i class="icol-page-save"></i> Save',
                                 "fnClick": function () {
                                     //chi ap dung cho demo
                                     var table = $(this)[0].dom.table;
                                     for (var j = 1; j <= i; j++) {
                                         //var a = $(table).find('.newRow_' + j + '').find('.col1').find('input[type="text"]').val();
                                         //$(table).find('.newRow_' + j + '').find('.col1').remove();
                                         //var b = $(table).find('.newRow_' + j + '').find('.col2').find('input[type="text"]').val();
                                         //$(table).find('.newRow_' + j + '').find('.col2').remove();
                                         //var c = $(table).find('.newRow_' + j + '').find('.col3').find('input[type="text"]').val();
                                         //$(table).find('.newRow_' + j + '').find('.col3').remove();

                                         $(table).find('.newRow_' + j + '').find('input[type="text"]').each(function () {
                                             var a = $(this).val();
                                             if (a) {
                                                 $(this).parent().append(a);
                                                 $(this).remove();
                                             }
                                         })
                                     }
                                 }
                             },
                            {
                                "sExtends": "delete",
                                "sButtonText": '<i class="icol-delete"></i> Delete',
                                "fnClick": function () {

                                }
                            }

                        ]
                    },
                    "bLengthChange": false,
                    "aoColumns": [
                        null,
                        null,
                        null,
                         { "bSortable": false }
                    ]
                });
            }
        }

    };
    
    $(window).load(function () { });

    $(document).ready(function () {

        if ($.fn.dataTable) {

            $('table#demo-dtable-01').dataTable();

            $('table#demo-dtable-02').dataTable().columnFilter();



            setTimeout(function () {
                var cust_list = $('table#demo-dtable-cust-list').dataTable({
                    "aoColumns": [
                           null,
                           null,
                           null,
                           null,
                            { "bSortable": false }
                    ],


                    "sDom": "<''<''<''>>r>t<'dt_footer'<'row'<'col-md-6 col-sm-6'i><'col-md-6 col-sm-6'p>>>"
                }).columnFilter();

                $("#searchBoxName").keyup(function () {
                    cust_list.fnFilter(this.value);
                });
                $("#searchBoxCodeName").keyup(function () {
                    cust_list.fnFilter(this.value, 0, true);
                });
            }, 500);


            var temp = demos.dtTableTools($('table#demo-dtable-03'));

            $('table#demo-dtable-rivision').dataTable({
                "sDom": "<'dt_header'<'row'<'col-md-12 col-sm-12'T>>r>t",
                //"sDom": "<'dt_header'<'row'<'col-md-4 col-sm-4'l><'col-md-12 col-sm-12'T>>r>t<'dt_footer'<'row'<'col-md-6 col-sm-6'i><'col-md-6 col-sm-6'p>>>",
                "oTableTools": {
                    "sSwfPath": "js/plugins/datatables/TableTools/swf/copy_csv_xls_pdf.swf",
                    "aButtons": [
                        //{
                        //	"sExtends": "copy", 
                        //	"sButtonText": '<i class="icol-clipboard-text"></i> Copy'
                        //}, 
                        //{
                        //	"sExtends": "csv", 
                        //	"sButtonText": '<i class="icol-doc-excel-csv"></i> CSV'
                        //}, 
                        //{
                        //	"sExtends": "xls", 
                        //	"sButtonText": '<i class="icol-doc-excel-table"></i> Excel'
                        //}, 							
                        //{
                        //	"sExtends": "pdf", 
                        //	"sButtonText": '<i class="icol-doc-pdf"></i> PDF'
                        //}, 
                        //{
                        //	"sExtends": "print", 
                        //	"sButtonText": '<i class="icol-printer"></i> Print'
                        //},
                        {
                            "sExtends": "add",
                            "sButtonText": '<i class="icol-add"></i> Add',
                            "fnClick": function () {

                            }
                        },
                         {
                             "sExtends": "save",
                             "sButtonText": '<i class="icol-page-save"></i> Save',
                             "fnClick": function () {

                             }
                         },
                        {
                            "sExtends": "delete",
                            "sButtonText": '<i class="icol-delete"></i> Delete',
                            "fnClick": function () {

                            }
                        }

                    ]
                },
                "bLengthChange": false
            });

            $('.table-with-button').dataTable({
                "sDom": "<'dt_header'<'row'<'col-md-12 col-sm-12'T>>r>t",
                //"sDom": "<'dt_header'<'row'<'col-md-4 col-sm-4'l><'col-md-12 col-sm-12'T>>r>t<'dt_footer'<'row'<'col-md-6 col-sm-6'i><'col-md-6 col-sm-6'p>>>",
                "oTableTools": {
                    "sSwfPath": "js/plugins/datatables/TableTools/swf/copy_csv_xls_pdf.swf",
                    "aButtons": [
                        //{
                        //	"sExtends": "copy", 
                        //	"sButtonText": '<i class="icol-clipboard-text"></i> Copy'
                        //}, 
                        //{
                        //	"sExtends": "csv", 
                        //	"sButtonText": '<i class="icol-doc-excel-csv"></i> CSV'
                        //}, 
                        //{
                        //	"sExtends": "xls", 
                        //	"sButtonText": '<i class="icol-doc-excel-table"></i> Excel'
                        //}, 							
                        //{
                        //	"sExtends": "pdf", 
                        //	"sButtonText": '<i class="icol-doc-pdf"></i> PDF'
                        //}, 
                        //{
                        //	"sExtends": "print", 
                        //	"sButtonText": '<i class="icol-printer"></i> Print'
                        //},
                        {
                            "sExtends": "add",
                            "sButtonText": '<i class="icol-add"></i> Add',
                            "fnClick": function () {

                            }
                        },
                         {
                             "sExtends": "save",
                             "sButtonText": '<i class="icol-page-save"></i> Save',
                             "fnClick": function () {

                             }
                         },
                        {
                            "sExtends": "delete",
                            "sButtonText": '<i class="icol-delete"></i> Delete',
                            "fnClick": function () {

                            }
                        }

                    ]
                },
                "bLengthChange": false
            });

            var tab = $('#example').dataTable({
                "aoColumns": [{ "bSortable": false }, null, null, null, null, null, null, null, null, null, null, null, null]
            });
            if ($.fn.dataTable.FixedHeader) {
                //new $.fn.dataTable.FixedHeader(tab);
            }
            
        }
    });

})(jQuery, window, document);