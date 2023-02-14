//alert(app.activeDocument.name);
//alert(app.activeDocument.fullName);

var id = app.activeDocument.name.split('_')[0];
var fileName = id;
//var filename_thumb = id + "_T_0.jpg";
//var filename_prev = id + "_P_0.jpg";
//var filename_real = id + "_R_0.jpg";
var folderPath = "/c/inddphoto/imgwork/";
var width_real = app.activeDocument.width;
var height_real = app.activeDocument.height;

// todo: ini 값
SaveFile(folderPath + "thumb/", 150);
SaveFile(folderPath + "prev/", 580);
SaveFile(folderPath + "real/", Math.max(width_real, height_real));


function SaveFile(path, rate) {
    var size_real = Math.max(width_real, height_real);

    if (size_real != rate)
        rate = (size_real < rate) ? size_real : rate;

    app.activeDocument.duplicate();

    var jpgFile = new File(path + fileName);
    var jpgSaveOptions = new JPEGSaveOptions();

    jpgSaveOptions.formatOptions = FormatOptions.OPTIMIZEDBASELINE;
    jpgSaveOptions.embedColorProfile = true;
    jpgSaveOptions.matte = MatteType.NONE;
    jpgSaveOptions.quality = 12;

    var runtimeEventID = stringIDToTypeID("3caa3434-cb67-11d1-bc43-0060b0a13dc4");
    var desc = new ActionDescriptor();
    var unitPixels = charIDToTypeID('#Pxl');

    desc.putUnitDouble(charIDToTypeID('Wdth'), unitPixels, rate);
    desc.putUnitDouble(charIDToTypeID('Hght'), unitPixels, rate);

    executeAction(runtimeEventID, desc, DialogModes.NO);

    app.activeDocument.saveAs(jpgFile, jpgSaveOptions, true, Extension.LOWERCASE);
    app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);
}
