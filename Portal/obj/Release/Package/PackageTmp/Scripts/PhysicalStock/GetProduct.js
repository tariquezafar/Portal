$(document).ready(function () {
    /***********GetProduct During Search ***********/
    
   
    enableAutoComplete($("[ProductName]"));
    /******End GetProduct During Search ********/
});



function enableAutoComplete($element) {
   // alert("Om Namah Shivay");
        debugger;
        $element.autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "../Product/GetProductAutoCompleteList",
                    type: "GET",
                    dataType: "json",
                    jsonpCallback: 'jsonCallback',
                    data: { term: request.term },
                    success: function (data) {

                        response($.map(data, function (item) {
                            return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode};
                        }))
                    }
                })
            },
            focus: function (event, ui) {
                $(this).closest('tr').find('.txtProductName').val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $(this).closest('tr').find('.txtProductName').val(ui.item.label);
                /*Assign Value in hidden Field*/

                $(this).closest('tr').find(".hdnProductId").val(ui.item.value);



                //GetProductDetail(ui.item.value, this);
                return false;
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $(".txtProductName").val("");
                    $(".hdnProductId").val("0");

                    ShowModel("Alert", "Please select Product from List")

                }
                return false;
            }

        })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.desc + "</div>")
          .appendTo(ul);
    };
    }
