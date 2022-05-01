$(document).ready(function () { 
  //  SearchThought();
    //$('#tblCompanyList').paging({ limit: 2 });   
});

$(".alpha-only").on("input", function () {
    var regexp = /[^a-zA-Z]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-space-only").on("input", function () {
    var regexp = /[^a-zA-Z\s]+$/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-numeric-only").on("input", function () {
    var regexp = /[^a-zA-Z0-9]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

function ClearFields() {
    //$("#hdnThoughtId").val("0");
    //$("#txtThoughtMsg").val("");
    //$("#txtThoughtType").val("");
    window.location.href = "../Thought/ListThought";

}
 
 
function SearchThought() {
    var txtThoughtName = $("#txtThoughtMsg");
    var txtThoughtType = $("#txtThoughtType");
    var requestData = { 
        thoughtName: txtThoughtName.val(),
        thoughtType: txtThoughtType.val()
    }; 
    $.ajax({
        url: "../Thought/GetThoughtList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data); 
        }
    });
}
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);

}