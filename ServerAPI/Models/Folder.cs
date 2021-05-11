using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Models {
    public class Folder {
        public Folder() {
            Folders = new List<Folder>();
            Files = new List<Files>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FolderId { get; set; }

        public int? ParentFolderId { get; set; }
        public string Name { get; set; }
        public string TruePath { get; set; }

        public virtual Folder ParentFolder { get; set; }

        public ICollection<Files> Files { get; set; }
        public ICollection<Folder> Folders { get; set; }
    }
}