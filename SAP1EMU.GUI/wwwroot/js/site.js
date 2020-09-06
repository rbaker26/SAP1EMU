// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function readFromFile(type, codeMirror, errorBoxID) {
    var input = document.createElement('input');
    input.type = 'file';
    input.accept = type;
    input.onchange = e => {
        var file = e.target.files[0];

        // Read the file contents
        var reader = new FileReader();
        reader.readAsText(file, 'UTF-8');
        
        // Send contents to CodeMiror Box
        reader.onload = readerEvent => {
            content = readerEvent.target.result;
            codeMirror.setValue(content);
        }

        reader.onerror = readerEvent => {
            $("#" + errorBoxID).setValue(readerEvent.target.error);
        }

    }
    input.click();
}
