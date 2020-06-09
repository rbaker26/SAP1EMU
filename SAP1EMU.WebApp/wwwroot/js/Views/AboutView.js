

function openGithub() {
    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-github-profile");
}


function openBenEater() {
    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-ben-eater");
}