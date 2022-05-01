
function PrintDiv(dvContents) {
    var divContents = $(dvContents).html();
    if (divContents != '') {
        var divContents = $(dvContents).html();
        var printWindow = window.open('', '', 'height=800,width=800');
        printWindow.document.write('<html><head><title></title>');
        printWindow.document.write('</head><body>');
        printWindow.document.write(divContents);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    }
    else {
        alert("No record to print.");
    }
}
function ExporttoExcel(dvContents) {
    var data = $(dvContents).html();
    window.open('data:application/vnd.ms-excel,' + encodeURIComponent(data));
}
