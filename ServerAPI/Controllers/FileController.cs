using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.DataBase;
using ServerAPI.Models;

namespace ServerAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller {
        private readonly CloudContext _context;

        public FilesController(CloudContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {


            return Ok(await _context.CloudFiles.Select(p => new Files {
                FilesId = p.FilesId,
                FileName = p.FileName,
                ParentFolderId = p.ParentFolderId,
                Size = p.Size
            }).ToListAsync());


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var file = await _context.CloudFiles.Select(p => new Files {
                FilesId = p.FilesId,
                FileName = p.FileName,
                ParentFolderId = p.ParentFolderId,
                Size = p.Size
            }).FirstOrDefaultAsync(p => p.FilesId == id);

            return file == null ? (IActionResult) NotFound() : Ok(file);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromBody] ContentU value) {

            var newFile = new Files {
                FileName = value.FileName,
                ParentFolderId = value.ParentFolderId,
                TruePath = value.FilePath,
                Bytes = value.Content,
                Size = value.Content.Length
            };

            try {
                if (_context.CloudFiles.FirstOrDefaultAsync(p =>
                    p.ParentFolderId == newFile.ParentFolderId && p.FileName == newFile.FileName) != null)
                    return BadRequest("File Already exists");

                var parentFolder =
                    await _context.CloudFolders.FirstOrDefaultAsync(p => p.FolderId == newFile.ParentFolderId);

                parentFolder.Files.Add(newFile);

                _context.CloudFolders.Update(parentFolder);
                await _context.SaveChangesAsync();
                newFile.Bytes = new byte[0];

                return Ok(newFile);
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadFile(int id) {

            var file = await _context.CloudFiles.FirstOrDefaultAsync(p => p.FilesId == id);

            return file == null ? (IActionResult)NotFound() : Ok(file);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContentU value) {
            var oldFile = await _context.CloudFiles.FirstOrDefaultAsync(p => p.FilesId == id);

            if (oldFile == null) return NotFound("File not found");

            oldFile.FileName = value.FileName;
            oldFile.ParentFolderId = value.ParentFolderId;
            try {
                _context.CloudFiles.Update(oldFile);
                await _context.SaveChangesAsync();
                oldFile.Bytes =new byte[0];
                return Ok(oldFile);
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var oldFile = await _context.CloudFiles.FirstOrDefaultAsync(p => p.FilesId == id);
            if (oldFile == null) return NotFound("File not found");

            try {
                _context.CloudFiles.Remove(oldFile);
                await _context.SaveChangesAsync();
                return Accepted();
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        public class ContentU {
            public string FileName { get; set; }
            public int ParentFolderId { get; set; }
            public string FilePath { get; set; }
            public byte[] Content { get; set; }
        }
    }
}