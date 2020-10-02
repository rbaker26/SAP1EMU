function openWiki() {
    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-wiki");
}