window.ScrollToBottom = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
};
window.ScrollToTop = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = 0;
};
window.PlayAudio = (elementName) => {
    document.getElementById(elementName).play();
};
window.BlazorDownloadFile= async (filename, contentType, data)=> {

    // Create the URL
    const file = new File([data], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);

    // Create the <a> element and click on it
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();

    // We don't need to keep the url, let's release the memory
    // On Safari it seems you need to comment this line... (please let me know if you know why)
    URL.revokeObjectURL(exportUrl);
    a.remove();
};

window.ReturnIndexPdfFile = async () => {
    // Create the URL
    var text = document.getElementsByTagName("input")[0];
    return text.value;
};