using System.IO;
using NuGet;

namespace DeReviewer.KnowledgeBase.Cases
{
    public sealed class FileWriterPatterns : Case
    {
        private void StaticWriteFile()
        {
            Pattern.CreateByName(it => File.WriteAllBytes(null, null));
//            Pattern.CreateByName(it => File.WriteAllLines(null, null));
//            Pattern.CreateByName(it => File.WriteAllText(null, null));
            
//            Pattern.CreateByName(it => File.AppendAllLines(null, null));
//            Pattern.CreateByName(it => File.AppendText(null));
//            Pattern.CreateByName(it => File.AppendAllText(null, null));
        }

        public void StaticCreateFile()
        {
            // Pattern.CreateByName(it => File.Create(""));            
            // Pattern.CreateByName(it => File.CreateText(""));            
            // Pattern.CreateByName(it => File.Open("", FileMode.OpenOrCreate));            
            Pattern.CreateByName(it => File.OpenWrite(""));            
            // Pattern.CreateByName(it => File.OpenText(""));            
        }

        private void StaticCopyMoveFile()
        {
            // Pattern.CreateByName(it => File.Move("", ""));
            // Pattern.CreateByName(it => File.Copy("", ""));
        }

        private void FileStream()
        {
            var fs = new FileStream(null, FileMode.Create);
            Pattern.CreateByName(it => fs.Write(null, 1, 1));
            Pattern.CreateByName(it => fs.WriteByte(1));
            Pattern.CreateByName(it => fs.WriteAsync(null, 1, 1));
            Pattern.CreateByName(it => fs.BeginWrite(null, 1, 1, null, null));
        }

        private void StreamWriter()
        {
            var sw = new StreamWriter("");
            Pattern.CreateByName(it => sw.Write(null, 1, 1));
            Pattern.CreateByName(it => sw.WriteAsync(null, 1, 1));
            Pattern.CreateByName(it => sw.WriteLineAsync(null, 1, 1));
            Pattern.CreateByName(it => sw.WriteLine(null, 1, 1));
        }

        private void Ctors()
        {
            //Pattern.CreateByName(it => new StreamWriter(""));
            // Pattern.CreateByName(it => new FileStream(null, FileMode.Create));
        }

        private void NuGet()
        {
            var pfs = new PhysicalFileSystem("qwert");
            Pattern.CreateByName(it => pfs.CreateFile(""));
            Pattern.CreateByName(it => pfs.AddFile("", (Stream)null));
        }
    }
}