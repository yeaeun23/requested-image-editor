
var id = app.activeDocument.name.split('_')[0];
var fileName = id;


thumb_default_size = 128;
prev_default_size = 640;


preferences.rulerUnits = Units.PIXELS;


var folderPath = "/c/inddphoto/imgwork/";
var nWidth = app.activeDocument.width;
var nHeight = app.activeDocument.height;

var thumWidth = 0;
var thumHeigth = 0;

var prevWidth = 0;
var prevHeigth = 0;


//alert("test");

if (nWidth >= nHeight)
{
    rate = nHeight / nWidth;
    
    
    
    thumWidth = thumb_default_size;
    prevWidth = prev_default_size;

    thumHeigth = (thumb_default_size * rate);
    prevHeigth = (prev_default_size * rate);


}
else
{
    rate = nWidth / nHeight;

    thumHeigth = thumb_default_size;
    prevHeigth = prev_default_size;

    thumWidth = (thumb_default_size * rate);
    prevWidth =(prev_default_size * rate);


}


SaveFile(folderPath + "thumb/", thumWidth,thumHeigth);
SaveFile(folderPath + "prev/", prevWidth,prevHeigth);
SaveFile(folderPath + "real/", nWidth,nWidth);


function SaveFile(path, nWidth,nHeight) {

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
    
  
    desc.putUnitDouble(charIDToTypeID('Wdth'), unitPixels, nWidth);
    desc.putUnitDouble(charIDToTypeID('Hght'), unitPixels, nHeight);
    
    
    

    executeAction(runtimeEventID, desc, DialogModes.NO);

    app.activeDocument.saveAs(jpgFile, jpgSaveOptions, true, Extension.LOWERCASE);
    app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);
}