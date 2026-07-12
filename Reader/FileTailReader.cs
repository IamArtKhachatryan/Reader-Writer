using System;
using System.IO;
using System.Text;

namespace  Reader
{
    class FileTailReader
    {
        private readonly string _filePath;
        
        // this is the bookmark in order to read only the new text and not all the text each time
        private long _lastPosition;
    
        public FileTailReader(string filePath)
        {
            _filePath = filePath;
            _lastPosition = 0;
        }
    
        public string ReadNewContent() // <----
        {
            using (FileStream fs = new FileStream(
                       _filePath,
                       FileMode.Open,
                       FileAccess.Read,
                       FileShare.ReadWrite))
            {
                // if length is < bookmark then nothing was added
                if (fs.Length <= _lastPosition)
                {
                    return "";
                }
    
                //putting cursor ar a bookmark
                fs.Seek(_lastPosition, SeekOrigin.Begin);
    
                byte[] buffer = new byte[fs.Length - _lastPosition];
                fs.Read(buffer, 0, buffer.Length);
    
                _lastPosition = fs.Length;
    
                return Encoding.UTF8.GetString(buffer);
            }
        }
    
}

}