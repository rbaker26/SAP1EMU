// These are global so they presist throught the page lifetime
// Add Two Numbers Example
var EightBit_P5_Pseudocode_1;
var EightBit_P5_SAP1EmuCode_1;

// Count to 10 Example
var EightBit_P5_Pseudocode_2;
var EightBit_P5_SAP1EmuCode_2;

window.onload = function () {
    // Setup Code Editor Boxes

    // Add Two Numbers Example
    EightBit_P5_Pseudocode_1 = CodeMirror.fromTextArea(document.getElementById("8Bit_P5_Pseudocode_1"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "clike" },
        readOnly: true,
    });

    EightBit_P5_SAP1EmuCode_1 = CodeMirror.fromTextArea(document.getElementById("8Bit_P5_SAP1EmuCode_1"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });

    // Count to 10 Example
    EightBit_P5_Pseudocode_2 = CodeMirror.fromTextArea(document.getElementById("8Bit_P5_Pseudocode_2"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "clike" },
        readOnly: true,
    });

    EightBit_P5_SAP1EmuCode_2 = CodeMirror.fromTextArea(document.getElementById("8Bit_P5_SAP1EmuCode_2"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });

    EightBit_P5_Pseudocode_1.setValue("num1 = 5\nnum2 = 6\nresult = num1 + num2\nPrint(result)\nExitProgram()");
    EightBit_P5_SAP1EmuCode_1.setValue("LDA 0xF # Load num1\nADD 0xE # num1 + num2\nSTA 0xD # result = A Register\nLDA 0xD # Load result \nOUT 0x0 # Print(result)\nHLT 0x0 # Exit Program\n...\n0x0 0x0 # result\n0x0 0x6 # num2\n0x0 0x5 # num1");

    // Count to 10 Example
    EightBit_P5_Pseudocode_2.setValue("count = 1\nwhile (count <= 10)\n  count = count + 1\n  Print(count)\nExitProgram()");
    EightBit_P5_SAP1EmuCode_2.setValue("LDA 0xF # load count\nSUB 0xE # count - 10\nJEQ 0x7 # if count == exit loop\nADD 0xE # Add 10 back\nADD 0xD # count = count + 1\nOUT 0x0 # Print(count)\nJMP 0x1 # loop to line #2\nHLT 0x0 # Exit Program\n...\n0x0 0x1 # Incrementor\n0x0 0xA # 10 in hex(loop control)\n0x0 0x0 # count");
}