using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MabinogiBackuperLib.FileFunctions
{
    public enum SizeType
    {
        KiloByte,
        MegaByte,
        GigaByte
    }
    public static class FileSize
    {
        public static ulong TotalFileSize(IEnumerable<string> files)
        {
            ulong len = 0L;
            foreach (var file in files)
            {
                var info = new FileInfo(file);
                len += (ulong)info.Length;
            }

            return len;
        }

        public static (SizeType sizeType, string converted) ConvertToString(ulong size)
        {
            var bytes = size;
            var calcBytes = (double)bytes;
            var byteString = "Bytes";
            var type = SizeType.KiloByte;
            if (bytes > 1073741824)
            {
                calcBytes = ((double)bytes / 1024 / 1024 / 1024);
                byteString = "GB";
                type = SizeType.GigaByte;
            }
            else if (bytes > 1048576)
            {
                calcBytes = ((double)bytes / 1024 / 1024);
                byteString = "MB";
                type = SizeType.MegaByte;
            }
            else if (bytes > 1024)
            {
                calcBytes = ((double)bytes / 1024);
                byteString = "KB";
            }

            return (type, $"{calcBytes:0.000} {byteString}");
        }

        public static string ConvertToString(ulong size, SizeType type)
        {
            var bytes = (double)size;
            var byteString = "Bytes";

            if (type == SizeType.GigaByte)
            {
                bytes /= 1073741824;
                byteString = "GB";
            }
            else if (type == SizeType.MegaByte)
            {
                bytes /= 1048576;
                byteString = "MB";
            }
            else if (type == SizeType.KiloByte)
            {
                bytes /= 1024;
                byteString = "KB";
            }

            return $"{bytes:0.000} {byteString}";
        }
    }
}
