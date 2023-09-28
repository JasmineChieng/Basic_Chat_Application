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
        public string FileName { get; set; } 
        public byte[] FileData { get; set; } 
        public string ImageFormat { get; set; } 

        public bool isCompressed { get; set; }

    }
}
