var asm_editor;
var ram_dump;
var frame_stack;
//var playerInstance;

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
    initBoard();



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

   // playerInstance = new player;

}


function initBoard() {
    $('#pc-block').html("0000");
    $('#wbus-block').html("0000 0000");
    $('#areg-block').html("0000 0000");
    $('#mar-block').html("0000");
    $('#alu-block').html("0000 0000");
    $('#ram-block').html("0000 0000");
    $('#breg-block').html("0000 0000");
    $('#ireg-block').html("0000 0000");
    $('#oreg-block').html("0000 0000");
    $('#seq-block').html("0011 1110 0011 11");
    $('#dis-block').html("0");

}

function updateBoard(frame) {
    $('#pc-block').html(frame.pc);
    $('#wbus-block').html(frame.wBus.match(/.{1,4}/g).join(' '));
    $('#areg-block').html(frame.aReg.match(/.{1,4}/g).join(' '));
    $('#mar-block').html(frame.mReg.match(/.{1,4}/g).join(' '));
    $('#alu-block').html(frame.alu.match(/.{1,4}/g).join(' '));
    $('#ram-block').html(frame.raM_Reg.match(/.{1,4}/g).join(' '));
    $('#breg-block').html(frame.bReg.match(/.{1,4}/g).join(' '));
    $('#ireg-block').html(frame.iReg.match(/.{1,4}/g).join(' '));
    $('#oreg-block').html(frame.oReg.match(/.{1,4}/g).join(' '));
    $('#seq-block').html(frame.seq.substring(0,13)); // TODO This substring should be handled at the API level, not the UI level
    $('#dis-block').html(parseInt(frame.oReg, 2) + " " + parseInt("0" + frame.oReg, 2));
}


//class player  {
//    interval= 500; // in ms
//    current_frame = 0;
//    frame_count = 0;

//    job_id = null;

    
//    play() {
//        $('#back-button').prop('disabled', true);
//        $('#next-button').prop('disabled', true);

//        job_id = setInterval(this.forward, this.interval);


//    }
//    pause() {
//        $('#back-button').prop('disabled', false);
//        $('#next-button').prop('disabled', false);
//    }

//    back() {

//    }

//    forward() {
//        if (this.current_frame < this.frame_count) {
//            updateBoard(frame_stack[this.current_frame]);
//            this.current_frame++;
//        }
//        else {
//            if (this.job_id != null) {
//                clearInterval(job_id);
//            }
//            else {
//                alert("Unknown JavaScript Failure");
//            }
//        }
//    }

//    reset () {
//        $('#back-button').prop('disabled', false);
//        $('#next-button').prop('disabled', false);
//    }
//    init() {
//        $('#back-button').prop('disabled', false);
//        $('#next-button').prop('disabled', false);

//        if (job_id != null) {
//            clearInterval(job_id);
//        }

//        frame_count = frame_stack.length;
//    }

//}

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


    // Make sure the player is cleared and halted
   // playerInstance.init();
}


var job_id = null;
function play_button_onclick() {
    if (job_id == null) {
        job_id = setInterval(frame_advance, 500);
    }
    else {
        clearInterval(job_id);
        job_id = null;
    }
   
}

var current_frame = 0;
function frame_advance() {
    if (current_frame < frame_stack.length) {
        updateBoard(frame_stack[current_frame]);
        current_frame++;
    }
    else {
        clearInterval(job_id);
        job_id = null;
    } 
        
    console.log(frame_stack[current_frame]);
}