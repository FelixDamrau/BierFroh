window.downloadFileFromStream = async (fileName, base64Content) => {
    const byteCharacters = atob(base64Content);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: "application/octet-stream" });
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement("a");
    anchorElement.download = fileName;
    anchorElement.href = url;
    document.body.appendChild(anchorElement); // Needed for Firefox
    anchorElement.click();
    document.body.removeChild(anchorElement);
};
