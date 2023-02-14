var fileName = app.activeDocument.name;
var savepath = "/C/inddphoto/imgwork/event/change&" + fileName.replace(".", "=") + ".evt";
var fileWriter = File(savepath);

// 외부 파일, 복제 파일(확장자 없음) 제외
if (fileName.substring(0, 3) == "zz-" && fileName.indexOf(".") != -1) {
    fileWriter.open("w");
    fileWriter.write(fileName);
    fileWriter.close();
}
