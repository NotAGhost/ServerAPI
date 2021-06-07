using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.DataBase;
using ServerAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase {
        private readonly CloudContext _context;

        public FolderController(CloudContext context) {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var list = await _context.CloudFolders.ToListAsync();

            list.RemoveAll(p => p.ParentFolderId != null);

            return Ok(list);
        }


        // GET api/<FolderController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var folder = await _context.CloudFolders.FirstOrDefaultAsync(p => p.FolderId == id);
            return folder == null ? (IActionResult) NotFound() : Ok(folder);
        }

        // GET api/<FolderController>/5
        [HttpGet("{id}/Files")]
        public async Task<IActionResult> GetFolderFiles(int id) {
            var files = await _context.CloudFilesModel.Where(p => p.ParentFolderId == id).ToListAsync();

            return files == null ? (IActionResult) NotFound() : Ok(files);
        }


        // POST api/<FolderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FolderContent value) {

            var checkFolder = await _context.CloudFolders.FirstOrDefaultAsync(p =>
                p.ParentFolderId == value.ParentFolderId && p.Name == value.Name);

            if (checkFolder != null) return BadRequest("Folder Already Exists in this directory");

            var entity = new Folder {
                ParentFolderId = value.ParentFolderId,
                Name = value.Name,
                TruePath = value.Path
            };

            await _context.CloudFolders.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        // PUT api/<FolderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FolderContent value) {

            var oldFolder = await _context.CloudFolders.FirstOrDefaultAsync(p => p.FolderId == id);

            if (oldFolder == null) return NotFound("File not found");

            oldFolder.Name = value.Name;
            oldFolder.ParentFolderId = value.ParentFolderId;

            try {
                _context.CloudFolders.Update(oldFolder);
                await _context.SaveChangesAsync();

                return Ok(oldFolder);
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        //Deletes the Folder and Its contents
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {

            var oldFolder = await _context.CloudFolders.Include(p => p.Folders).ThenInclude(p => p.Files)
                .FirstOrDefaultAsync(p => p.FolderId == id);

            if (oldFolder == null) return NotFound("File not found");

            try {
                DeleteAllFolders(oldFolder);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e) {
                return BadRequest(e);
            }
        }

        private void DeleteAllFolders(Folder parent) {

            foreach (var folder in parent.Folders) {

                if (folder.Folders.Count == 0) {
                    if (folder.Files.Count == 0) continue;

                    _context.CloudFilesModel.RemoveRange(folder.Files);
                    continue;

                }

                DeleteAllFolders(folder);
                _context.CloudFilesModel.RemoveRange(folder.Files);
                _context.CloudFolders.Remove(folder);
            }

            _context.CloudFolders.Remove(parent);
        }
    }

    public class FolderContent {
        public string Name { get; set; }
        public string Path { get; set; }

        public int? ParentFolderId { get; set; }
    }
}