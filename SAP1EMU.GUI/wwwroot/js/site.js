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

function updateGutter(cm) {
    lineNumber = 1;

    const doc = cm.getDoc();
    const cursor = doc.getCursor();

    //Since the html is exactly the same i need a way to distinguish between the code mirrors.
    //In order for this to work the column needs to be defined as editor if you want it to strip and comment rows only of numbering
    //and then some issues with copy paste rose with a gutter being in no mans land so it started from 2 on. This contains an array
    //of the gutter elements found by jquery recursive search with the certain parent of CodeMirror-code since gutter is a child of this
    gutterElements = $('.editor').find('.CodeMirror-code').find('.CodeMirror-gutter-elt');

    for (i = 0; i < doc.lineCount(); i++) {
        line = doc.getLine(i);

        //If the line matches a comment style or its a empty line (they hit enter or on a new line) and its not the current line we editing, hide the number in gutter
        if (line.match(/(^#w*)|(^\s+#w*)/g) || (line.length == 0 && cursor.line != i)) {
            gutterElements.eq(i).text('');
        } else {
            gutterElements.eq(i).text(lineNumber);
            lineNumber++;
        }
    }
}