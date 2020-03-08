using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleTo16x16
{
    enum RemoveCase
    {
        Left = 0,
        Right = 1,
        Both = 10,
        FalsePositive = 11
    };

    class FoldingManager
    {
        private string[] Paths;
        private string FolderPath;
        private Dictionary<string, string> TempOfRemoved = new Dictionary<string, string>();

        public FoldingManager(string folderPath)
        {
            this.FolderPath = folderPath;
            this.Paths = this.GetAllImagesPaths();
        }

        public string GetFolderPath()
        {
            return this.FolderPath;
        }

        public string[] GetPaths()
        {
            return this.Paths;
        }

        public int GetTempFilesCount()
        {
            return this.TempOfRemoved.Count;
        }

        public void CheckTemp()
        {
            if (!Directory.Exists($"{this.FolderPath}\\TempFolder"))
            {
                DirectoryInfo di = Directory.CreateDirectory($"{this.FolderPath}\\TempFolder");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (this.TempOfRemoved.Count > Constants.TEMP_AMOUNT - 1)
            {
                File.Delete($"{this.FolderPath}\\TempFolder\\{this.TempOfRemoved.Keys.First()}");
                File.Delete($"{this.FolderPath}\\TempFolder\\{this.TempOfRemoved.Values.First()}");

                this.TempOfRemoved.Remove(this.TempOfRemoved.Keys.First());
            }
        }

        public void RemoveSelectedFiles(RemoveCase removeCase)
        {
            var leftMatch = this.TempOfRemoved.Keys.Last();
            var rightMatch = this.TempOfRemoved.Values.Last();

            string leftImageFile = $"{this.FolderPath}\\{rightMatch}";
            string rightImageFile = $"{this.FolderPath}\\{leftMatch}";

            if (!File.Exists(leftImageFile) || !File.Exists(rightImageFile)) return;

            string leftTempImageFile = $"{this.FolderPath}\\TempFolder\\{leftMatch}";
            string rightTempImageFile = $"{this.FolderPath}\\TempFolder\\{rightMatch}";

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

            this.TempOfRemoved.Add(leftMatch, rightMatch);
        }

        public void DeleteTempFolder()
        {
            if (Directory.Exists($"{this.FolderPath}\\TempFolder"))
                Directory.Delete($"{this.FolderPath}\\TempFolder");
        }

        public (string, string)? Retrieve()
        {
            if (this.GetTempFilesCount() > 0)
            {
                string leftMatch = this.TempOfRemoved.Keys.Last();
                string rightMatch = this.TempOfRemoved.Values.Last();

                FoldingHelpers.MoveIfExists($"{this.FolderPath}\\TempFolder\\{leftMatch}", $"{this.FolderPath}\\{leftMatch}");
                FoldingHelpers.MoveIfExists($"{this.FolderPath}\\TempFolder\\{rightMatch}", $"{this.FolderPath}\\{rightMatch}");

                this.TempOfRemoved.Remove(leftMatch);

                return (leftMatch, rightMatch);
            }

            return null;
        }

        private static string[] GetAllImagesPaths()
        {
            return Directory
                .GetFiles(this.FolderPath, "*.*")
                .AsParallel()
                .Where(s =>
                    s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    s.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase)
                )
                .ToArray();
        }
    }
}
