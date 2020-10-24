var API_P3_POST_Body;
var API_P3_POST_EMULATE;
var API_P3_POST_EMULATE_Response;

window.onload = function () {

    API_P3_POST_Body = CodeMirror.fromTextArea(document.getElementById("API_P3_POST_Body"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "javascript", json: true },
        readOnly: true,
        lineWrapping: true,
    });

    API_P3_POST_Body.setValue(
        "{\n" +
        "\t\"CodeList\": [ \n" +
        "\t\t\"LDA 0xF\",  \n" +
        "\t\t\"ADD 0xE\",  \n" +
        "\t\t\"OUT 0x0\",  \n" +
        "\t\t\"HLT 0x0\",  \n" +
        "\t\t\"...\"       \n" +
        "\t\t\"0x0 0x1\",  \n" +
        "\t\t\"0xF 0xF\",  \n" +

        "\t],\n" +
        "\t\"SetName\": \"SAP1EMU\"\n" +
        "}"
    );


    API_P3_POST_EMULATE = CodeMirror.fromTextArea(document.getElementById("API_P3_POST_EMULATE"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "clike", mime: "text/x-csharp" },
        lineWrapping: true,
    });
    API_P3_POST_EMULATE.setValue(
        "using(var httpClient = new HttpClient())\n" +
        "{\n " +
        "\tusing (var httpContent = new StringContent(requestBody, Encoding.UTF8, \"application/json\"))\n" +
        "\t{\n" +
        "\t\tvar response = await httpClient.PostAsync(\"https://sap1emu.net/api/Assembler\", httpContent);\n" +
        "\t\tstring responseBody = await response.Content.ReadAsStringAsync();\n" +
        "\t\tList<string> asm = JsonSerializer.Deserialize<List<string>>(responseBody);\n" +
        "\t}\n" +
        "}"
    );

    API_P3_POST_EMULATE_Response = CodeMirror.fromTextArea(document.getElementById("API_P3_POST_EMULATE_Response"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "javascript", json: true },
        readOnly: true,
        lineWrapping: true,
    });

    API_P3_POST_EMULATE_Response.setValue(
        "[\n  {\n    \"instruction\": \"???\",\n    \"tState\": 1,\n    \"aReg\": \"00000000\",\n    \"bReg\": \"00000000\",\n    \"iReg\": \"00000000\",\n    \"iRegShort\": \"0000\",\n    \"mReg\": \"0000\",\n    \"oReg\": \"00000000\",\n    \"pc\": \"0000\",\n    \"alu\": \"00000000\",\n    \"seq\": \"01011110001111000\",\n    \"wBus\": \"00000000\",\n    \"raM_Reg\": \"00000000\",\n    \"overflow_Flag\": \"0\",\n    \"underflow_Flag\": \"0\",\n    \"zero_Flag\": \"0\",\n    \"ram\": [\n      \"00000000\",\n      \"11100000\",\n      \"11110000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\"\n\t]\n  },\n  {\n    \"instruction\": \"???\",\n    \"tState\": 2,\n    \"aReg\": \"00000000\",\n    \"bReg\": \"00000000\",\n    \"iReg\": \"00000000\",\n    \"iRegShort\": \"0000\",\n\t\"mReg\": \"0000\",\n\t\"oReg\": \"00000000\",\n\t\"pc\": \"0001\",\n\t\"alu\": \"00000000\",\n\t\"seq\": \"10111110001111000\",\n\t\"wBus\": \"00000000\",\n\t\"raM_Reg\": \"00000000\",\n\t\"overflow_Flag\": \"0\",\n\t\"underflow_Flag\": \"0\",\n\t\"zero_Flag\": \"0\",\n\t\"ram\": [\n      \"00000000\",\n      \"11100000\",\n      \"11110000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\",\n      \"00000000\"\n\t]\n  },\n  ...\n]"
    );
}