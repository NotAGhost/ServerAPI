using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Models {
    public class Files {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilesId { get; set; }

        public int ParentFolderId { get; set; }
        public string FileName { get; set; }
        public string VirtualPath { get; set; }
        public string Size { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual FilesContent FileContent { get; set; }
    }
}