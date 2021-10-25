import os

def Modulation(fileName):
    f = open(fileName, "a");
    f.writelines("aa");
    f.close();
    

def Exploring(rootPath):
    for fileName in os.listdir(rootPath):
        filePath = os.path.join(rootPath, fileName)
        #print("현재 작업: " + filePath)

        if os.path.isdir(filePath):
            Exploring(filePath)
        else:
            Modulation(filePath);
            filePathStruct = os.path.splitext(filePath)
            try:
                os.rename(filePath, filePath + '.TESTVIRUS')
            except:
                print("오류")

def main():
    Exploring("D:\\Code\\Capstone\\FileChangeWatcher\\FileChangeDataset")

main()