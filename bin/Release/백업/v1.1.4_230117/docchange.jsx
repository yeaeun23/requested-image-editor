var fileName = app.activeDocument.name;
var savepath = "/C/inddphoto/imgwork/event/change&" + fileName.replace(".", "=") + ".evt";
var fileWriter = File(savepath);

// �ܺ� ����, ���� ����(Ȯ���� ����) ����
if (fileName.substring(0, 3) == "zz-" && fileName.indexOf(".") != -1) {
    fileWriter.open("w");
    fileWriter.write(fileName);
    fileWriter.close();
}
