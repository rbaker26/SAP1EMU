var asm_editor;
var bin_editor;

window.onload = function () {
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
}


function AssembleCode(btnClicked) {
    var asm_code = asm_editor.getValue().split('\n');
    var asm_json = JSON.stringify(asm_code);
    console.log(asm_code);
    console.log(asm_json);

    $.ajax({
        url: "../api/Assembler",
        type: "POST",
        contentType: 'application/json; charset=UTF-8',
        data: asm_json,
        success: function (data) {

            console.log(data.toString());
            console.log(data.toString().replace(/,/g, '\n'));
            $('#assembler-out').html('<br />');



            bin_editor.setValue(data.toString().replace(/,/g, '\n'));
            return data;
        },
        error: function (request, status, error) {
            bin_editor.setValue("");
            $('#assembler-out').html(request.responseText);
        }
    });

    return false;// if it's a link to prevent post

}