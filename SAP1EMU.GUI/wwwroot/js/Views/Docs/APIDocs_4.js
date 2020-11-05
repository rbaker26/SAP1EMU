var API_P3_POST_Body;
var API_P3_POST_EMULATE;
var API_P3_POST_EMULATE_Response;

var EMULATOR_CODE_LISTINGS = {
    'python3': (
        "from urllib.request import Request, urlopen\n"+
        "import json\n"+
        "uri = \"https://sap1emu.net/api/Emulator\"\n"+
        "request_content = {\n"+
        "    \"CodeList\": [\n"+
        "        \"LDA 0xF\",\n"+
        "        \"ADD 0xE\",\n"+
        "        \"OUT 0x0\",\n"+
        "        \"HLT 0x0\",\n"+
        "        \"...\",\n"+
        "        \"0x0 0x1\",\n"+
        "        \"0xF 0xF\"],\n"+
        "    \"SetName\": \"SAP1EMU\"\n"+
        "}\n"+
        "request_body = json.dumps(request_content).encode()\n"+
        "response = urlopen(Request(uri, method=\"POST\", data=request_body, headers={\"Content-Type\": \"application/json\"}))\n"+
        "response_body = response.read()\n"+
        "asm = json.loads(response_body)\n"
    ),
    "java11": (
        "import com.google.gson.Gson;\n\n" +
        "import java.io.IOException;\n" +
        "import java.net.URI;\n" +
        "import java.net.URISyntaxException;\n" +
        "import java.net.http.HttpClient;\n" +
        "import java.net.http.HttpRequest;\n" +
        "import java.net.http.HttpResponse;\n\n" +
        "public class Main {\n" +
        "    public static void main(String[] args) {\n" +
        "        try {\n" +
        "            HttpClient client = HttpClient.newHttpClient();\n" +
        "            HttpRequest request = HttpRequest.newBuilder(new URI(\"https://sap1emu.net/api/Emulator/\"))\n" +
        "                    .header(\"Content-Type\", \"application/json\")\n" +
        "                    .POST(HttpRequest.BodyPublishers.ofString(\n" +
        "                            \"{\n" +
        "                                \"\\\"CodeList\\\": [\" +\n" +
        "                                \"\\\"LDA 0xF\\\",\" +\n" +
        "                                \"\\\"ADD 0xE\\\",\" +\n" +
        "                                \"\\\"OUT 0x0\\\",\" +\n" +
        "                                \"\\\"HLT 0x0\\\",\" +\n" +
        "                                \"\\\"...\\\",\" +\n" +
        "                                \"\\\"0x0 0x1\\\",\" +\n" +
        "                                \"\\\"0xF 0xF\\\"\" +\n" +
        "                                \"],\" +\n" +
        "                                \"\\\"SetName\\\": \\\"SAP1EMU\\\"\" +\n" +
        "                           \"}\"\n" +
        "                    ))\n" +
        "                    .build();\n" +
        "            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());\n" +
        "            Gson gson = new Gson();\n" +
        "            Frame[] frameData = gson.fromJson(response.body(), Frame[].class);\n" +
        "        }\n" +
        "        catch (URISyntaxException | IOException | InterruptedException e) {\n" +
        "            e.printStackTrace();\n" +
        "        }\n" +
        "    }\n" +
        "}"
    )
};

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
        "\t\tvar response = await httpClient.PostAsync(\"https://sap1emu.net/api/Emulator\", httpContent);\n" +
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
