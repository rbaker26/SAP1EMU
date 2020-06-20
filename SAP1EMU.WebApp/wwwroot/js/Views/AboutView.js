

function openGithub() {
    console.log("Open GitHub");

    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-github-profile");
}


function openBenEater() {
    console.log("Open Ben");

    const { ipcRenderer } = require("electron");
    ipcRenderer.send("open-ben-eater");
    
}