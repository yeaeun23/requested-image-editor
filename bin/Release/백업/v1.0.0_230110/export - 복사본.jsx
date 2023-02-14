
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



SaveFile(folderPath + "real/", nWidth,nHeight);
SaveFile(folderPath + "prev/", prevWidth,prevHeigth);
SaveFile(folderPath + "thumb/", thumWidth,thumHeigth);



function SaveFile(path, nWidth,nHeight) {
    //var curdoc = app.activeDocument;

    var jobdoc = app.activeDocument.duplicate();
    
    
   
    var jpgFile = new File(path + fileName);
    var jpgSaveOptions = new JPEGSaveOptions();

    jpgSaveOptions.formatOptions = FormatOptions.OPTIMIZEDBASELINE;
    jpgSaveOptions.embedColorProfile = true;
    jpgSaveOptions.matte = MatteType.NONE;
    jpgSaveOptions.quality = 12;
  
    
    jobdoc.changeMode(ChangeMode.RGB);
    jobdoc.resizeImage(nWidth, nHeight, 72, ResampleMethod.BICUBICSMOOTHER);
  
    jobdoc.saveAs(jpgFile, jpgSaveOptions, true, Extension.LOWERCASE);
    jobdoc.close(SaveOptions.DONOTSAVECHANGES);
  
        
}