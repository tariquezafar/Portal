$(document).ready(function () {
    $("#txtEmpName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../EmployeeStateMap/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtEmpName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmpName").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);
            $("#txtEmpCode").val(ui.item.EmployeeCode);
            $("#txtMobileNo").val(ui.item.MobileNo);
            GetState();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmpName").val("");
                $("#hdnEmployeeId").val("0");
                $("#txtEmpCode").val("");
                $("#txtMobileNo").val("");
                ShowModel("Alert", "Please select Employee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};

});

function GetState() {
    var txtEmpName = $("#txtEmpName");
    var employeeId = $("#hdnEmployeeId").val();
    //if (txtEmpName.val() == "" || txtEmpName.val() == "0") {
    //    ShowModel("Alert", "Please select Employee")
    //    ddlRole.focus();
    //    return false;
    //}
    var requestData = { employeeId: employeeId };
    $.ajax({
        url: "../EmployeeStateMap/GetStateList",
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
function CheckAllAddAccess(obj) {
    $('.AddAccess').prop('checked', obj.checked);

}
function SaveData()
{
    var txtEmpName = $("#txtEmpName");
    var employeeId = $("#hdnEmployeeId").val();
    if (txtEmpName.val() == "" || txtEmpName.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        ddlRole.focus();
        return false;
    }
    var mappingStatus = false;
    var stateMappingList = [];
    $('.mapping-list tr').each(function (i, row) {
        var $row = $(row);
        var employeeStateMappingId = $row.find("#hdnEmployeeStateMappingId").val();
        var stateId = $row.find("#hdnStateId").val();
        var addAccess = $row.find("#chkstate").is(':checked') ? true : false;
        if(employeeStateMappingId != undefined) {
           var empStateMapping = {
                EmployeeStateMappingId: employeeStateMappingId,
                EmployeeId: employeeId,
                StateId: stateId,
               SelectState:addAccess

            };
            stateMappingList.push(empStateMapping);
            mappingStatus = true;
        }
    });
    if(mappingStatus = false)
    {
        ShowModel("Alert", "Please select at least one State");
        return false;
    } 
    var requestData = { employeeStateMappingList: stateMappingList };
    $.ajax({
        url: "../EmployeeStateMap/AddEditEmpStateMapping",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                $("#btnUpdate").show();
                GetState();
               // $("#btnUpdate").hide();
                
            }
            else
            {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });

}
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText); 
}
function ClearFields()
{
    $("#txtEmpName").val("");
    $("#txtEmpCode").val("");
    $("#txtMobileNo").val("");
    $('input:checkbox').prop('checked', false);
    $("#divList").html("");
 }
function CheckAllAddAccess(obj)
{
    $('.AddAccess').prop('checked', obj.checked);
}

