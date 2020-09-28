// These are global so they presist throught the page lifetime
var EightBit_P3_Example_Left;
var EightBit_P3_Example_Right;
var EightBit_P3_Example_Comment_Left;
var EightBit_P3_Example_Comment_Right;

window.onload = function () {
    // Setup Code Editor Boxes

    // Macro Example
    EightBit_P3_Example_Left = CodeMirror.fromTextArea(document.getElementById("8Bit_P3_Example_Left"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });

    EightBit_P3_Example_Right = CodeMirror.fromTextArea(document.getElementById("8Bit_P3_Example_Right"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });


    // Comments Example
    EightBit_P3_Example_Comment_Left = CodeMirror.fromTextArea(document.getElementById("8Bit_P3_Example_Comment_Left"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });

    EightBit_P3_Example_Comment_Right = CodeMirror.fromTextArea(document.getElementById("8Bit_P3_Example_Comment_Right"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
    });

    // Macro Example
    EightBit_P3_Example_Left.setValue("LDA 0xF\nADD 0xE\nOUT 0x0\nHLT 0x0\n...\n0x7 0x1\n0xF 0xE");
    EightBit_P3_Example_Right.setValue("LDA 0xF\nADD 0xE\nOUT 0x0\nHLT 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x7 0x1\n0xF 0xE");

    // Comments Example
    EightBit_P3_Example_Comment_Left.setValue("LDA 0xF\nADD 0xE # A = A + RAM[14]\nJIC 0x4 # if Carry==1, exit loop\nJMP 0x1 # JMP to ADD 0xE\nOUT 0x0\nHLT 0x0\n...\n# Data Section:\n0x0 0x1 # 1\n0xF 0xD #253");
    EightBit_P3_Example_Comment_Right.setValue("LDA 0xF\nADD 0xE\nJIC 0x4\nJMP 0x1\nOUT 0x0\nHLT 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x0\n0x0 0x1\n0xF 0xD");


}