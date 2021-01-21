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

    //Check when the user is typing
    asm_editor.on("change", function (cm, obj) { updateGutter(cm); });

    //Check when it updates the DOM so pasting, hitting enter, etc...
    asm_editor.on("update", function (cm) { updateGutter(cm); });

    $.ajax({
        url: "../api/Assembler/supported_emulators",
        type: "GET",
        success: function (data) {
            var selectDOM = document.getElementById("emulators");
            var options = data;

            for (var i = 0; i < options.length; i++) {
                var opt = options[i];

                var elem = document.createElement("option");
                elem.text = opt;
                elem.value = opt;

                selectDOM.add(elem);
            }

            getSupportedSets();
        },
        error: function (request, status, error) {
            alert("SAP1EMU ERROR: JSON CONFIG FILE ERROR:\n" + request.responseText);
        }
    });
}

function AssembleCode() {
    var asm_code = asm_editor.getValue().split('\n');
    var langChoice = document.getElementById("langs").value;
    var emulator = document.getElementById('emulators').value;

    jsonData = JSON.stringify({ CodeList: asm_code, SetName: langChoice, Emulator: emulator });

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

function getFromFile() {
    readFromFile(".s,.asm", asm_editor, "assembler-out");
}

function getSupportedSets() {
    // Setup ComboBox
    $.ajax({
        url: "../api/Assembler/supported_sets",
        type: "GET",
        data: {
            "Emulator": document.getElementById('emulators').value
        },
        success: function (data) {
            var selectDOM = document.getElementById("langs");
            $('#langs').empty();
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