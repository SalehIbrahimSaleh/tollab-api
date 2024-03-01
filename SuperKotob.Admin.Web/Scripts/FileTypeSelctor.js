

function checkFile( ShowAlert) {
    var fileElement = document.getElementById("DataType");
    if (ShowAlert === true) {
        alert("Please Select the "+getSelectedText('DataType')+" files only");
    }
        var fileNames = { Photo: [".gif,.jpg,.jpeg,.png"], PDF: [".pdf"], Video: [".mp4"], Audio: [".mp3"], Zip: [".zip,.rar"], Text: [".txt"], Doc: [".doc,.docx"], Excel: [".xlsx,.xls"] };

        var fileExtension = "";
        if (fileElement.value.lastIndexOf(".") > 0) {
            fileExtension = fileElement.value.substring(fileElement.value.lastIndexOf(".") + 1, fileElement.value.length);
        }
        if (getSelectedText('DataType') === "Photo") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Photo;
            return true;
        }
        else  if (getSelectedText('DataType') === "PDF") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.PDF;
            return true;
        }
        else  if (getSelectedText('DataType') === "Video") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Video;
            return true;
        }
        else if (getSelectedText('DataType') === "Audio") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Audio;
            return true;
        }
        else if (getSelectedText('DataType') === "Zip") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Zip;
            return true;
        }
        else if (getSelectedText('DataType') === "Text") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Text;
            return true;
        }
        else if (getSelectedText('DataType') === "Doc") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Doc;
            return true;
        }
        else if (getSelectedText('DataType') === "Excel") {
            var uploadfileinput = document.getElementById("uploadFile");
            uploadfileinput.accept = fileNames.Excel;
            return true;
        }
        else {
                   
            alert("You must select a data type of file");
            var btn = document.getElementById("sum");
            btn.innerHTML ="Create";
            btn.disabled = false;

            return false;

        }
    }


    
function getSelectedText(elementId) {
    var elt = document.getElementById(elementId);

    if (elt.selectedIndex == -1)
        return null;

    return elt.options[elt.selectedIndex].text;
}
