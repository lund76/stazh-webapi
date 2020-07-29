using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Services;

namespace Stazh.Application.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IConfiguration _configuration;

        public ItemController( IConfiguration configuration, IItemService itemService)
        {
            _itemService = itemService;
            _configuration = configuration;
        }

        // GET: api/File
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/File/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/File
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(IFormCollection data)
        {
            var userID = User?.Identity?.Name ?? "1";

            var storageConfig = new StorageConfig
            {
                ConnectionString = _configuration["FileConnectionString"], FileContainer = "images"
            };

            var item = new Item
            {
                Description = data["description"],
                Name = data["Name"],
                Parent = _itemService.FindParentFromName(data["parentItem"]),
                Created = DateTime.UtcNow,
                ItemAttachments = new HashSet<Attachment>()
            };


            foreach (var file in data.Files)
            { 
              var addedFile = await  _itemService.AddFile(file.OpenReadStream(),storageConfig,file.FileName,userID);
              var attachment = new Attachment {OriginalFileName = addedFile.OriginalFileName, UniqueAttachmentName = addedFile.UniqueFilename};
              MemoryStream ms = new MemoryStream();
              await file.OpenReadStream().CopyToAsync(ms);
              attachment.Thumbnail = ms.ToArray();
              
              item.ItemAttachments.Add(attachment);
            }

            var apiItem = _itemService.InsertNewItem(item);

            return new JsonResult(apiItem);
             
        }

        // PUT: api/File/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
