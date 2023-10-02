async function saveAsFile(data) {
    // Optional filename
    const opts = {
        types: [{
            description: 'Image Profile File',
            accept: {
                'text/plain': ['.csv'],
            },
        }],
    };

    // create a new handle
    const newHandle = await window.showSaveFilePicker(opts);

    // create a FileSystemWritableFileStream to write to
    const writableStream = await newHandle.createWritable();

    // write our file
    writableStream.write(data);

    // close the file and write the contents to disk.
    writableStream.close();

}