using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Models {
    public class Files {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilesId { get; set; }
        public int ParentFolderId { get; set; }
        public string FileName { get; set; }
        public string TruePath { get; set; }
        public byte[] Bytes { get; set; }

        public int Size { get; set; }

        public virtual Folder ParentFolder { get; set; }
    }
}