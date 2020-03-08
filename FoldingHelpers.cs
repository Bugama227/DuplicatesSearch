using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleTo16x16
{
    class FoldingHelpers
    {
        public static void CheckTemp(string FolderPath, Dictionary<string, string> TempOfRemoved)
        {
            if (!Directory.Exists($"{FolderPath}\\TempFolder"))
            {
                DirectoryInfo di = Directory.CreateDirectory($"{FolderPath}\\TempFolder");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (TempOfRemoved.Count > Constants.TEMP_AMOUNT - 1)
            {
                File.Delete($"{FolderPath}\\TempFolder\\{TempOfRemoved.Keys.First()}");
                File.Delete($"{FolderPath}\\TempFolder\\{TempOfRemoved.Values.First()}");

                TempOfRemoved.Remove(TempOfRemoved.Keys.First());
            }

        }

        public static void RemoveSelectedFiles(RemoveCase removeCase, string FolderPath, string leftMatch, string rightMatch)
        {
            string leftImageFile = $"{FolderPath}\\{leftMatch}";
            string rightImageFile = $"{FolderPath}\\{rightMatch}";

            if (!File.Exists(leftImageFile) || !File.Exists(rightImageFile)) return;

            string leftTempImageFile = $"{FolderPath}\\TempFolder\\{leftMatch}";
            string rightTempImageFile = $"{FolderPath}\\TempFolder\\{rightMatch}";

            switch (removeCase)
            {
                case RemoveCase.Left:
                    File.Move(leftImageFile, leftTempImageFile);
                    break;

                case RemoveCase.Right:
                    File.Move(rightImageFile, rightTempImageFile);
                    break;

                case RemoveCase.Both:
                    File.Move(leftImageFile, leftTempImageFile);
                    File.Move(rightImageFile, rightTempImageFile);
                    break;

                case RemoveCase.FalsePositive:
                    break;
            }
        }

        public static void MoveIfExists(string from, string to)
        {
            if (File.Exists(from)) File.Move(from, to);
        }

        public static void DeleteTempFolder(string folderPath)
        {
            if (folderPath != null && Directory.Exists($"{folderPath}\\TempFolder"))
                Directory.Delete($"{folderPath}\\TempFolder");
        }
    }
}
