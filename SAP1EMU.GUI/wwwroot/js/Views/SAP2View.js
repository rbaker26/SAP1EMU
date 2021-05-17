var asm_editor;
var ram_dump;
var frame_stack;
var interval_slider;
var interval_time = 500;

const ConvertBase = {
    bin2dec: s => parseInt(s, 2).toString(10),
    bin2hex: s => parseInt(s, 2).toString(16),
    dec2bin: s => parseInt(s, 10).toString(2),
    dec2hex: s => parseInt(s, 10).toString(16),
    hex2bin: s => parseInt(s, 16).toString(2),
    hex2dec: s => parseInt(s, 16).toString(10)
};

const FlagResults = {
    None: 0,
    Carry: 1,
    Parity: 4,
    AuxiliaryCarry: 16,
    Zero: 64,
    Sign: 128
}


window.onload = function () {
    interval_slider = document.getElementById("formControlRange");

    asm_editor = CodeMirror.fromTextArea(document.getElementById("asm_code"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
    });
    asm_editor.setSize("100%", 300);

    //Check when the user is typing
    asm_editor.on("change", function (cm, obj) { updateGutter(cm); });

    //Check when it updates the DOM so pasting, hitting enter, etc...
    asm_editor.on("update", function (cm) { updateGutter(cm); });

    ram_dump = CodeMirror.fromTextArea(document.getElementById("ram_dump"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "gas_sap1", architecture: "x86" },
        readOnly: true,
        firstLineNumber: 0,
        lineNumberFormatter: function (line) { return "0x" + (line + 2048).toString(16).toLocaleUpperCase(); },
    });
    ram_dump.setSize("100%", 437);


    initRam();
    initBoard();
    setControlButtonsDisabled(true);

    // Must be last line of function
    //preloadCode();
}


function initBoard() {
    //$('#pc-block').html("0000");
    //$('#wbus-block').html("0000 0000");
    //$('#areg-block').html("0000 0000");
    //$('#mar-block').html("0000");
    //$('#alu-block').html("0000 0000");
    //$('#ram-block').html("0000 0000");
    //$('#breg-block').html("0000 0000");
    //$('#ireg-block').html("0000 0000");
    //$('#oreg-block').html("0000 0000");
    //$('#seq-block').html("0011 1110 0011 11");
    //$('#dis-block').html("0");
    //$('#carryFlagBox').html("0");
    //$('#underflowFlagBox').html("0");
    //$('#zeroFlagBox').html("0");
}

function updateBoard(frame) {

    
    
    $('#pc-block').html(frame.pc.match(/.{1,4}/g).join(' '));
    $('#iport2-block').html(frame.iPort1.match(/.{1,4}/g).join(' '));
    $('#iport2-block').html(frame.iPort1.match(/.{1,4}/g).join(' '));
    $('#mar-block').html(frame.mar.match(/.{1,4}/g).join(' '));
    $('#ram-block').html(frame.raM_Reg.match(/.{1,4}/g).join(' '));
    $('#mdr-block').html(frame.mar.match(/.{1,4}/g).join(' '));
    $('#ireg-block').html(frame.iReg.match(/.{1,4}/g).join(' '));
    var tempSeq = frame.seq.padEnd(36, '0');
    $('#seq-block').html(tempSeq.match(/.{1,4}/g).join(' '));


    var wbusUpperTemp = '0x' + ConvertBase.bin2hex(frame.wBus.substring(0, 8)).padStart(2, '0');
    var wbusLowerTemp = '0x' + ConvertBase.bin2hex(frame.wBus.substring(8)).padStart(2, '0');
    $('#wbus-block').html(wbusUpperTemp + ' ' + wbusLowerTemp);


    $('#areg-block').html(frame.aReg.match(/.{1,4}/g).join(' '));
    $('#alu-block').html(frame.alu.match(/.{1,4}/g).join(' '));
    $('#treg-block').html(frame.tReg.match(/.{1,4}/g).join(' '));
    $('#breg-block').html(frame.bReg.match(/.{1,4}/g).join(' '));
    $('#creg-block').html(frame.cReg.match(/.{1,4}/g).join(' '));
    $('#oport1-block').html(frame.oPort1.match(/.{1,4}/g).join(' '));
    $('#oport2-block').html(frame.oPort2.match(/.{1,4}/g).join(' '));


    //$('#carryFlagBox').val(frame.overflow_Flag);
    //$('#underflowFlagBox').val(frame.underflow_Flag);
    //$('#zeroFlagBox').val(frame.zero_Flag);

    var posVal = parseInt(frame.oReg, 2);
    var negVal = posVal;

    if (posVal > 127) {
        negVal = (-1) * (256 - posVal);
    }
    var displayString = "" + posVal;
    if (negVal < 0) {
        displayString += " " + negVal;
    }

    $('#dis-block').html(displayString);
}

function initRam() {
    // Init RAM Box
    var ram_string = "";

    for (var i = 0; i < 0xFFFF - 0x0800; i++) {
        ram_string += "0000 0000\n";
    }
    ram_string += "0000 0000";
    ram_dump.setValue(ram_string);
}

function loadRam(ram) {
    //var tempString = "";
    //for (var i = 0; i < 16; i++) {
    //    tempString += ram[i].substring(0, 4);
    //    tempString += " ";
    //    tempString += ram[i].substring(4, 8);
    //    if (i < 15) {
    //        tempString += "\n";
    //    }
    //}
    //ram_dump.setValue(tempString);
}

function resetBoard(frame) {
    updateBoard(frame);

    ////Change the instruction and tstate to default state
    //$('#instruction-box').text("???");
    //$('#tstate-box').val("T1");

    ////Set current frame back to 0 and make progress 0 since its a new program loaded
    //current_frame = 0;
    //updateProgressBar(0, frame_stack.length); //In case anyone has a previously loaded program in to know when its loaded.
}

function LoadIntoRAM() {
    var asm_code = asm_editor.getValue().split('\n');
    var langChoice = "Malvino"; // document.getElementById("langs").value;


    var emulationId = null;
    $.ajax({
        url: "../api/emulator/sap2/session/create",
        type: "GET",
        async: false,
        cache: false,
        success: function (data) {
            emulationId = data;
            return data;
        },
        error: function (request, status, error) {
            $('#emulator-out').html(request.responseJSON.message);
            return null;
        }
    });

    jsonData = JSON.stringify({ EmulationID: emulationId, Code: asm_code, SetName: langChoice });


    $.ajax({
        url: "../api/emulator/sap2/emulate",
        type: "POST",
        cache: false,
        contentType: 'application/json; charset=UTF-8',
        data: jsonData,
        success: function (data) {
            console.log(data);

            $('#emulator-out').html(''); // clear the error msg box
            hideErrorAlertBadge();
            
            frame_stack = data;
            first_frame = frame_stack[15];

            updateBoard(first_frame);

            //loadRam(first_frame.ram);

            return data;
        },
        error: function (request, status, error) {
            initRam();
            console.log(request);
            $("#error-alert-badge").show();
            $('#emulator-out').html(request.responseJSON.message);
        }
    });

    setControlButtonsDisabled(false);
}

var job_id = null;
var justPaused = false;
function play_button_onclick() {
    if (job_id == null) {
        $("#play-pause-img").attr("src", "/img/pause-24px.svg");
        job_id = setInterval(frame_advance, interval_time);

        // Disable back and next
        $("#back-button").prop('disabled', true);
        $("#next-button").prop('disabled', true);
    }
    else {
        justPaused = true;
        clearInterval(job_id);
        job_id = null;
        $("#play-pause-img").attr("src", "/img/play_arrow-24px.svg");

        // Enable back and next
        $("#back-button").prop('disabled', false);
        $("#next-button").prop('disabled', false);
    }
}

function back_button_onclick() {
    //frame_reverse();
}

function next_button_onclick() {
    //frame_advance();
}

function reset_button_onclick() {
    //current_frame = 0;
    //updateBoard(frame_stack[current_frame]);
    //loadRam(frame_stack[current_frame].ram);
    //$("#instruction-box").text(frame_stack[current_frame].instruction);
    //$("#tstate-box").val('T' + frame_stack[current_frame].tState);
    //updateProgressBar(current_frame, frame_stack.length);
}

var current_frame = 0;
function frame_advance() {
    if (current_frame < frame_stack.length - 1) {
        current_frame++;
        updateBoard(frame_stack[current_frame]);
        loadRam(frame_stack[current_frame].ram);
        $("#instruction-box").text(frame_stack[current_frame].instruction);
        $("#tstate-box").val('T' + frame_stack[current_frame].tState);

        // Update Progress Bar
        updateProgressBar(current_frame, frame_stack.length);
    }
    else {
        $('#frameProgressBar').css("width", "100%");
        clearInterval(job_id);
        job_id = null;
    }

    console.log(frame_stack[current_frame]);
}

function frame_reverse() {
    //if (justPaused) {
    //    current_frame--;
    //    justPaused = false;
    //}
    //if (current_frame > 0) {
    //    current_frame--;
    //    updateBoard(frame_stack[current_frame]);
    //    loadRam(frame_stack[current_frame].ram);
    //    $("#instruction-box").text(frame_stack[current_frame].instruction);
    //    $("#tstate-box").val('T' + frame_stack[current_frame].tState);

    //    // Update Progress Bar
    //    updateProgressBar(current_frame, frame_stack.length);
    //}
}

function getFromFile() {
    //readFromFile(".s,.asm", asm_editor, "emulator-out");
}

function setControlButtonsDisabled(isDisabled) {
    //$("#back-button").prop('disabled', isDisabled);
    //$("#play-pause-button").prop('disabled', isDisabled);
    //$("#next-button").prop('disabled', isDisabled);
    //$("#reset-button").prop('disabled', isDisabled);
}

function updateProgressBar(currentFrame, frameStackLength) {
    if (currentFrame == frameStackLength - 1) {
        $('#frameProgressBar').css("width", "100%");
    }
    else {
        var frameProgress = (current_frame / frame_stack.length) * 100;
        $('#frameProgressBar').css("width", frameProgress + "%");
    }
}

function changeIntervalTiming(value) {
    // keep the time from getting too long
    if (value <= .250) {
        value = .250;
    }
    interval_time = (1 / value) * 500;

    // If we currently have a job in process meaning the code is executing then
    //  clear it and change the interval time and start again
    if (job_id != null) {
        clearInterval(job_id);
        job_id = setInterval(frame_advance, interval_time);
    }
}

function hideErrorAlertBadge() {
    $("#error-alert-badge").hide();
}