// These are global so they presist throught the page lifetime
var asm_editor;
var bin_editor;

window.onload = function () {
    // Setup Code Editor Boxes
    asm_editor = CodeMirror.fromTextArea(document.getElementById("asm_code"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
    });

    bin_editor = CodeMirror.fromTextArea(document.getElementById("bin_code"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });


    // Setup ComboBox
    $.ajax({
        url: "../api/Assembler/supported_sets",
        type: "GET",
        success: function (data) {

            var selectDOM = document.getElementById("langs");
            var options = data;

            for (var i = 0; i < options.length; i++) {
                var opt = options[i];

                var elem = document.createElement("option");
                elem.text = opt;
                elem.value = opt;

                selectDOM.add(elem);
            }
        },
        error: function (request, status, error) {
            alert("SAP1EMU ERROR: JSON CONFIG FILE ERROR:\n" + request.responseText);
        }
    });
}


function AssembleCode() {
    var asm_code = asm_editor.getValue().split('\n');
    var langChoice = document.getElementById("langs").value;

    jsonData = JSON.stringify({ CodeList: asm_code, SetName: langChoice });

    $.ajax({
        url: "../api/Assembler",
        type: "POST",
        contentType: 'application/json; charset=UTF-8',
        data: jsonData,
        success: function (data) {
            $('#assembler-out').html('<br />');

            bin_editor.setValue(data.toString().replace(/,/g, '\n'));
            return data;
        },
        error: function (request, status, error) {
            bin_editor.setValue("");
            $('#assembler-out').html(request.responseText);
        }
    });

    return false;

}



function openFromFile() {
    // Send Request to ASP.NET
    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-from-file-asm");

    // Receive code back from ASP.NET
    ipcRenderer.once("code-from-file-asm",
        (sender, asm_code) => {
            asm_editor.setValue(asm_code);
        }
    );

}