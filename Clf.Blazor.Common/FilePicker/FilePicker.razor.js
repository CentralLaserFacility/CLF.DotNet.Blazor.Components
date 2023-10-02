// https://developer.mozilla.org/en-US/docs/Web/API/Window/showSaveFilePicker
// https://developer.mozilla.org/en-US/docs/Web/API/Window/showOpenFilePicker
// https://github.com/KristofferStrube/Blazor.FileSystemAccess
// https://github.com/KristofferStrube/Blazor.FileAPI

export async function saveToFileUsingFilePicker(options, content) {
  const newHandle = await window.showSaveFilePicker(options);
  // create a FileSystemWritableFileStream to write to
  const writableStream = await newHandle.createWritable();
  // write our file
  writableStream.write(content);
  // close the file and write the contents to disk.
  writableStream.close();
}

export async function getFilesUsingFilePicker(options, returnAsText=true) {
  const fileHandles = await window.showOpenFilePicker(options)

  const files = await Promise.all(
    fileHandles.map(async (fileHandle) => {
      const file = await fileHandle.getFile();
      const content = returnAsText ? await file.text() : new Uint8Array(await file.arrayBuffer());
      const fileObj = {
        lastModified: new Date(file.lastModified),
        name: file.name,
        webkitRelativePath: file.webkitRelativePath,
        size: file.size,
        type: file.type,
        content: content
      };
      return fileObj;
    })
  );
  return files;
}