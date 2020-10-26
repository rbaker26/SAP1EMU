// These are global so they presist throught the page lifetime
var API_P3_ISets;
var API_P3_ISets_Response;
var API_P3_POST_Body;
var API_P3_POST_ASSEMBLE;
var API_P3_POST_ASSEMBLE_Response;

var SUPPORTED_SETS_CODE_LISTINGS = {
    'python3': (
        "from urllib.request import urlopen\n"+
        "import json\n"+
        "uri = \"https://sap1emu.net/api/Assembler/supported_sets\"\n"+
        "response = urlopen(uri)\n"+
        "result = response.read()\n"+
        "sets_list = json.loads(result)\n"),
};

var ASSEMBLER_CODE_LISTINGS = {
    'python3': (
        "from urllib.request import Request, urlopen\n"+
        "import json\n"+
        "uri = \"https://sap1emu.net/api/Assembler\"\n"+
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
        "asm = json.loads(response_body)\n"+
    ),
};


window.onload = function () {
    // Setup Code Editor Boxes


    // GET ISet Example
    API_P3_ISets = CodeMirror.fromTextArea(document.getElementById("API_P3_ISets"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "clike", mime: "text/x-csharp" },
        readOnly: true,
        lineWrapping: true,
    });
    API_P3_ISets.setValue(
        "List<string> setsList;\n"+
        "using (var httpClient = new HttpClient())\n" +
        "{\n" +
        "string uri = \"https://sap1emu.net/api/Assembler/supported_sets\"\n"+
        "\tstring responseBody = await httpClient.GetStringAsync(uri);\n" +
        "\tsetsList = JsonSerializer.Deserialize<List<string>>(responseBody);\n" +
        "}"
    );

    API_P3_ISets_Response = CodeMirror.fromTextArea(document.getElementById("API_P3_ISets_Response"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "javascript", json: true },
        readOnly: true,
        lineWrapping: true,
    });
    API_P3_ISets_Response.setValue(
        "[\n" +
        "\t\"SAP1Emu\",\n" +
        "\t\"Malvino\",\n" +
        "\t\"BenEater\"\n" +
        "]\n\n"
    );

    // POST Assembler Example
    API_P3_POST_Body = CodeMirror.fromTextArea(document.getElementById("API_P3_POST_Body"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "javascript", json: true },
        readOnly: true,
        lineWrapping: true,
    });

    API_P3_POST_Body.setValue(
        "{\n"+
        "\t\"CodeList\": [ \n"         +
        "\t\t\"LDA 0xF\",  \n"         +
        "\t\t\"ADD 0xE\",  \n"         +
        "\t\t\"OUT 0x0\",  \n"         +
        "\t\t\"HLT 0x0\",  \n"         +
        "\t\t\"...\"       \n" +
        "\t\t\"0x0 0x1\",  \n" +
        "\t\t\"0xF 0xF\",  \n" +

        "\t],              \n"         +
        "\t\"SetName\": \"SAP1EMU\"\n" +
        "}\n\n\n\n\n\n"
    );


    API_P3_POST_ASSEMBLE = CodeMirror.fromTextArea(document.getElementById("API_P3_POST_ASSEMBLE"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "clike", mime: "text/x-csharp" },
        readOnly: true,
        lineWrapping: true,
    });
    API_P3_POST_ASSEMBLE.setValue(                                                                 
    "using(var httpClient = new HttpClient())\n"+
    "{\n "+
    "\tusing (var httpContent = new StringContent(requestBody, Encoding.UTF8, \"application/json\"))\n"+
    "\t{\n"+
    "\t\tvar response = await httpClient.PostAsync(\"https://sap1emu.net/api/Assembler\", httpContent);\n"+
    "\t\tstring responseBody = await response.Content.ReadAsStringAsync();\n"+
    "\t\tList<string> asm = JsonSerializer.Deserialize<List<string>>(responseBody);\n"+
    "\t}\n"+
    "}"
    );   

    API_P3_POST_ASSEMBLE_Response = CodeMirror.fromTextArea(document.getElementById("API_P3_POST_ASSEMBLE_Response"), {
        lineNumbers: true,
        matchBrackets: true,
        mode: { name: "javascript", json: true },
        readOnly: true,
        lineWrapping: true,
        scrollbarStyle: "null",
        viewportMargin: Infinity,
        
    });
    API_P3_POST_ASSEMBLE_Response.setValue(
        "[\n" +
        "\t\"00001111\"\n"+
        "\t\"00011110\"\n"+
        "\t\"11100000\"\n"+
        "\t\"11110000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000000\"\n"+
        "\t\"00000001\"\n"+
        "\t\"11111111\"\n"+
        "]"
    );
}
