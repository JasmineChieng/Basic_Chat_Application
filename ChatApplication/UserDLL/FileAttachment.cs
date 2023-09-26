using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UserDLL
{
    public class FileAttachment
    {
        public string FileName { get; set; } // Name of the attached file
        public byte[] FileData { get; set; } // Byte array containing the file data
        public string ImageFormat { get; set; } // Store the image format as a string

        public bool isCompressed { get; set; }

        public bool isImage { get; set; }

    }
}
