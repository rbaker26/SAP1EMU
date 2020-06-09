var asm_editor;
var ram_dump;

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
        lineNumberFormatter: function (line) { return "0x" + line.toString(16).toLocaleUpperCase();},
    });

    // Init RAM Box
    ram_dump.setValue("0000 0000\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
}