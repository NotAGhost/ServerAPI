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

            return Ok(await _context.CloudFilesModel.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var file = await _context.CloudFilesModel.FirstOrDefaultAsync(p => p.FilesId == id);

            return file == null ? (IActionResult) NotFound() : Ok(file);
        }

        //Needs some work 
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromBody] FileData value) {

            var newFile = new Files {
                FileName = value.FileName,
                ParentFolderId = value.ParentFolderId,
                Size = value.Content.Length.ToString()
            };

            try {

                if (await _context.CloudFilesModel.FirstOrDefaultAsync(p =>
                    p.ParentFolderId == newFile.ParentFolderId && p.FileName == newFile.FileName) != null)
                    return BadRequest("File Already exists");

                var parentFolder =
                    await _context.CloudFolders.FirstOrDefaultAsync(p => p.FolderId == newFile.ParentFolderId);

                //Virtual Path
                newFile.VirtualPath = parentFolder.TruePath + "/" + newFile.FileName;

                newFile.FileContent = new FilesContent {
                    FileBytes = value.Content
                };

                parentFolder.Files.Add(newFile);

                _context.CloudFolders.Update(parentFolder);
                await _context.SaveChangesAsync();

                //Create Content Entry

                return Ok(newFile);
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadFile(int id) {

            var file = await _context.CloudFilesContent.FirstOrDefaultAsync(p => p.FilesDataId == id);

            return file == null ? (IActionResult) NotFound() : Ok(file);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FileData value) {

            var oldFile = await _context.CloudFilesModel.FirstOrDefaultAsync(p => p.FilesId == id);

            if (oldFile == null) return NotFound("File not found");

            oldFile.ParentFolderId = value.ParentFolderId;

            try {

                if (value.FileName != oldFile.FileName) {
                    var oldFileContent = await _context.CloudFilesContent
                        .FirstOrDefaultAsync(p => p.FilesDataId == oldFile.FilesId);

                    oldFileContent.FileBytes = value.Content;
                    oldFile.FileName = value.FileName;

                    _context.CloudFilesContent.Update(oldFileContent);

                }

                _context.CloudFilesModel.Update(oldFile);
                await _context.SaveChangesAsync();

                return Ok(oldFile);
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {

            var oldFile = await _context.CloudFilesModel.FirstOrDefaultAsync(p => p.FilesId == id);

            if (oldFile == null) return NotFound("File not found");

            try {
                _context.CloudFilesModel.Remove(oldFile);
                await _context.SaveChangesAsync();
                return Accepted();
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        /*
        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAll() {
            try {
                _context.CloudFiles.RemoveRange(_context.CloudFiles);
                await _context.SaveChangesAsync();
                return Accepted();
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }
        */
    }
}