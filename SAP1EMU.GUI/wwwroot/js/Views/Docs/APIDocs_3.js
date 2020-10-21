// These are global so they presist throught the page lifetime
var API_P3_ISets;
var API_P3_ISets_Response;


window.onload = function () {
    // Setup Code Editor Boxes


    // Get ISet Example
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
        "]"
    );
}