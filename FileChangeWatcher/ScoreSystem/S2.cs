using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher.ScoreSystem
{
    /// <summary>
    /// S2 점수 클래스
    /// </summary>
    class S2 : AbstractScoreSystem
    {
        private Dictionary<string, int> wrongExtensionDictionary;

        public override void Calculate()
        {
            InitWorngExtensionDictionary();
        }

        /// <summary>
        /// 잘못된 확장자는 초기화하는 메소드
        /// </summary>
        private void InitWorngExtensionDictionary()
        {
            foreach (string extension in dbms.ChangeFileList)
            {
                if (this.CheckExtension(extension))
                    continue;

                if(this.wrongExtensionDictionary.ContainsKey(extension))
                    this.wrongExtensionDictionary[extension]++;
                else
                    this.wrongExtensionDictionary.Add(extension, 1);
            }
        }

        /// <summary>
        /// 올바른 확장자인지 확인하는 메소드
        /// </summary>
        /// <param name="extension">확장자</param>
        /// <returns>올바른 확장자: true, 올바르지 못한 확장자: false</returns>
        private bool CheckExtension(string extension)
        {
            bool result;

            switch(extension)
            {
                case "aac":
                case "adt":
                case "adts":
                case "accdb":
                case "accde":
                case "accdr":
                case "accdt":
                case "aif":
                case "aifc":
                case "aiff":
                case "aspx":
                case "avi":
                case "bat":
                case "bin":
                case "bmp":
                case "cab":
                case "cda":
                case "csv":
                case "dll":
                case "doc":
                case "docm":
                case "docx":
                case "dot":
                case "dotx":
                case "eml":
                case "eps":
                case "exe":
                case "flv":
                case "gif":
                case "htm":
                case "html":
                case "ini":
                case "iso":
                case "jar":
                case "Jpg":
                case "jpeg":
                case "m4a":
                case "mdb":
                case "mid":
                case "midi":
                case "mov":
                case "mp3":
                case "mp4":
                case "mpeg":
                case "mpg":
                case "msi":
                case "mui":
                case "pdf":
                case "png":
                case "pot":
                case "potm":
                case "potx":
                case "ppam":
                case "pps":
                case "ppsm":
                case "ppsx":
                case "ppt":
                case "pptm":
                case "pptx":
                case "psd":
                case "pst":
                case "pub":
                case "rar":
                case "rtf":
                case "sldm":
                case "sldx":
                case "swf":
                case "sys":
                case "tif":
                case "tiff":
                case "tmp":
                case "txt":
                case "vob":
                case "vsd":
                case "vsdm":
                case "vsdx":
                case "vss":
                case "vssm":
                case "vst":
                case "vstm":
                case "vstx":
                case "wav":
                case "wbk":
                case "wks":
                case "wma":
                case "wmd":
                case "wmv":
                case "wmz":
                case "wms":
                case "wpd":
                case "wp5":
                case "xla":
                case "xlam":
                case "xll":
                case "xlm":
                case "xls":
                case "xlsm":
                case "xlsx":
                case "xlt":
                case "xltm":
                case "xltx":
                case "xps":
                case "zip":
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
