using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Models {
    public class FilesContent {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentId { get; set; }

        public int FilesDataId { get; set; }

        public byte[] FileBytes { get; set; }

        public Files FileModel { get; set; }
    }
}