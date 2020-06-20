

function openWiki() {
    console.log("Open Wiki");
    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-wiki");
}
