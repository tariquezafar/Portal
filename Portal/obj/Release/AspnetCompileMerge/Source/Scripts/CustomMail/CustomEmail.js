$(document).ready(function () {
   
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


function SendCustomMail() {
    var hdnMailId = $("#hdnMailId");
    var hdnfromMailId = $("#hdnfromMailId");
    var txtMailTo = $("#txtMailTo");
    var txtMailCC = $("#txtMailCC");
    var txtMailBCC = $("#txtMailBCC");
    var txtMailSubject = $("#txtMailSubject");
    var txtEditor = $("#txtEditor");
    if (txtMailTo.val().trim() == "") {
        ShowModel("Alert", "Please Enter Mail To")
        txtMailTo.focus();
        return false;
    }

    if (txtMailSubject.val().trim() == "") {
        ShowModel("Alert", "Please select Email Mail Subject")
        ddlCountry.focus();
        return false;
    }
    if (txtEditor.Editor("getText") == "") {
        ShowModel("Alert", "Please Enter Email Description");
        txtEditor.focus();
        return false;
    }
    var Documents = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (documentName != undefined) {
                var mailDocument = {
                    DocumentSequenceNo: documentSequenceNo,
                    DocumentName: documentName,
                    DocumentPath: documentPath
                };
                Documents.push(mailDocument);
            
            }
        });

            var customMailViewModel = {
                hdnMailId: hdnMailId.val(),
                MailFrom: hdnfromMailId.val(),
                MailTo: txtMailTo.val().trim(),
                MailCC: txtMailCC.val(),
                MailBCC: txtMailBCC.val(),
                MailSubject: txtMailSubject.val(),
                MailBody: txtEditor.Editor("getText").trim()
            };
            var requestData = { customMailViewModel: customMailViewModel, mailDocuments: Documents };
            $.ajax({
                url: "../CustomMail/SendCustomMail",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(requestData),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        ShowModel("Alert", data.message);
                        ClearFields(); 
                    }
                    else {
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

        function GetMailDocumentList(mailDocuments) {
            var requestData = { mailDocuments: mailDocuments };
            $.ajax({
                url: "../CustomMail/GetMailSupportingDocumentList",
                cache: false,
                data: JSON.stringify(requestData),
                dataType: "html",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                error: function (err) {
                    $("#divDocumentList").html("");
                    $("#divDocumentList").html(err);
                },
                success: function (data) {
                    $("#divDocumentList").html("");
                    $("#divDocumentList").html(data);
                    ShowHideDocumentPanel(2);
                }
            });
        }

        function RemoveDocumentRow(obj) {
            if (confirm("Do you want to remove selected Document?")) {
                var row = $(obj).closest("tr");
                ShowModel("Alert", "Document Removed from List.");
                row.remove();
            }
        }
        
        function SaveDocument() { 
            if (window.FormData !== undefined) {
                var uploadfile = document.getElementById('FileUpload2');
                var fileData = new FormData();
                if (uploadfile.value == "") {
                    alert("Please Select File");
                    return false;
                } 
                
                else {
                    var fileUpload = $("#FileUpload2").get(0);
                    var files = fileUpload.files;

                    if (uploadfile.files[0].size > 50000000) {
                        uploadfile.files[0].name.length = 0;
                        ShowModel("Alert", "File is too big")
                        uploadfile.value = ""; 
                        return "";
                    }

                    for (var i = 0; i < files.length; i++) {
                        fileData.append(files[i].name, files[i]);
                    }
                  
                }
              
            }
            else {
                ShowModel("Alert", "FormData is not supported.")
                return "";
            }

            $.ajax({
                url: "../CustomMail/SaveSupportingDocument",
                type: "POST",
                asnc: false,
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                error: function () {
                    ShowModel("Alert", "An error occured")
                    return "";
                },
                success: function (result) {
                    if (result.status == "SUCCESS") {
                        var newFileName = result.message;

                        var docEntrySequence = 0;
                        var hdnDocumentSequence = $("#hdnDocumentSequence");
                        var FileUpload1 = $("#FileUpload2");
                        if (FileUpload1.val() == undefined || FileUpload1.val() == "") {
                            ShowModel("Alert", "Please select File To Upload")
                            return false;
                        }

                        var Documents = [];
                        if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                            docEntrySequence = 1;
                        }
                        $('#tblDocumentList tr').each(function (i, row) {
                            var $row = $(row);
                            var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                            var documentName = $row.find("#hdnDocumentName").val();
                            var documentPath = $row.find("#hdnDocumentPath").val();

                            if (documentName != undefined) {
                                if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                                    var mailDocument = {
                                        DocumentSequenceNo: documentSequenceNo,
                                        DocumentName: documentName,
                                        DocumentPath: documentPath
                                    };
                                    Documents.push(mailDocument);
                                    docEntrySequence = parseInt(docEntrySequence) + 1;
                                }
                                else if (hdnDocumentSequence.val() == documentSequenceNo) {
                                    var mailDocument = {
                                        DocumentSequenceNo: hdnDocumentSequence.val(),
                                        DocumentName: newFileName,
                                        DocumentPath: newFileName
                                    };
                                    Documents.push(mailDocument);
                                }
                            }
                        });
                        if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                            hdnDocumentSequence.val(docEntrySequence);
                        }

                        var mailDocumentAddEdit = {
                            DocumentSequenceNo: hdnDocumentSequence.val(),
                            DocumentName: newFileName,
                            DocumentPath: newFileName
                        };
                        Documents.push(mailDocumentAddEdit);
                        hdnDocumentSequence.val("0");
                        GetMailDocumentList(Documents);
                    
                    }
                    else {

                        ShowModel("Alert", result.message);
                    }
                }
            });
        }
       
        function ClearFields() {
            $("#txtEmailTemplateName").val("");
            $("#hdnEmailTemplateId").val("0");
            $("#hdnAccessMode").val("0");
            $("#ddlEmailTemplateType").val("0");
            $("#txtMailTo").val("");
            $("#txtMailCC").val("");
            $("#txtMailBCC").val("");
            
            $("#FileUpload2").val("");
            $("#hdnDocumentSequence").val("0");
            $("#hdnDocumentPath").val("0");
            
            $("#txtMailSubject").val("");
            $("#txtEditor").Editor("setText", "");
            $(".documentsection").hide();
            $("#divDocumentList").html("");
            //$("#chkStatus").prop('checked', false);

        }
        
        function ShowHideDocumentPanel(action) {
            if (action == 1) {
                $(".documentsection").show();
            }
            else {
                $(".documentsection").hide();
                $("#FileUpload1").val("");
                $("#btnAddDocument").show();
                $("#btnUpdateDocument").hide();
            }
        }
    