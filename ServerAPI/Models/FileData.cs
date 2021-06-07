using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Models {
    public class FileData {
        public string FileName { get; set; }
        public int ParentFolderId { get; set; }
        public byte[] Content { get; set; }
    }
}