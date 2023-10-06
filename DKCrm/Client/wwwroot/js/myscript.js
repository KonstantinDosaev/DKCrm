window.ScrollToBottom = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}

window.PlayAudio = (elementName) => {
    document.getElementById(elementName).play();
}