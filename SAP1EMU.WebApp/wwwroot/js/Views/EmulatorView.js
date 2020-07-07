var asm_editor;
var ram_dump;
var frame_stack;

window.onload = function () {
    asm_editor = CodeMirror.fromTextArea(document.getElementById("asm_code"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
    });

    ram_dump = CodeMirror.fromTextArea(document.getElementById("ram_dump"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
        firstLineNumber: 0,
        lineNumberFormatter: function (line) { return "0x" + line.toString(16).toLocaleUpperCase(); },
    });



    initRam();



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

function initRam() {
    // Init RAM Box
    var ram_string = "";

    for (var i = 0; i < 15; i++) {
        ram_string += "0000 0000\n";
    }
    ram_string += "0000 0000";
    ram_dump.setValue(ram_string);
}

function loadRam(ram) {
    var tempString = "";
    for (var i = 0; i < 16; i++) {
        tempString += ram[i].substring(0, 4);
        tempString += " ";
        tempString += ram[i].substring(4, 8);
        if (i < 15) {
            tempString += "\n";
        }
    }
    ram_dump.setValue(tempString);
}

function RunEmulator() {

    var asm_code = asm_editor.getValue().split('\n');
    var langChoice = document.getElementById("langs").value;

    jsonData = JSON.stringify({ CodeList: asm_code, SetName: langChoice });
    console.log(jsonData);

    $.ajax({
        url: "../api/Emulator",
        type: "POST",
        contentType: 'application/json; charset=UTF-8',
        data: jsonData,
        success: function (data) {
            $('#emulator-out').html('<br />'); // clear the error msg box

            frame_stack = data;
            first_frame = frame_stack[0];

            loadRam(first_frame.ram);


            return data;
        },
        error: function (request, status, error) {
            initRam();
            $('#emulator-out').html(request.responseText);
        }
    });
}