using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stazh.Application.WebApi.Utility;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Models.Api;
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
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        private string ExtUserId => ClaimHelper.GetUserIdFromClaim(User.Claims);

        public ItemController( IConfiguration configuration, IItemService itemService, IUserService userService, IFileService fileService)
        {
            _itemService = itemService;
            _configuration = configuration;
            _userService = userService;
            _fileService = fileService;
        }

        // GET: api/item
        [HttpGet]
        public IEnumerable<ApiItem> Get()
        {
            var items =  _itemService.FindAllBaseItems(ExtUserId);
            return items;
        }

        // GET: api/File/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<ApiItem> Get(int id)
        {
            if (id == 0) return Get();
            var items = _itemService.GetChildItemsFor(ExtUserId, id);
            return items;
        }

        // POST: api/File
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(IFormCollection data)
        {
            var userId = ClaimHelper.GetUserIdFromClaim(User.Claims);
            
         

            var item = new Item
            {
                Owner = _userService.GetOrCreateUser(userId),
                Description = data["description"],
                Name = data["Name"],
                Parent = _itemService.FindParentFromId(data["parentItem"]),
                Created = DateTime.UtcNow,
                ItemAttachments = new HashSet<Attachment>()
            };


            foreach (var file in data.Files)
            { 
              var addedFile = await  _itemService.AddFile(file.OpenReadStream(),file.FileName,userId);
              var attachment = new Attachment {OriginalFileName = addedFile.OriginalFileName, UniqueAttachmentName = addedFile.UniqueFilename};
              
              //Removing thumbnails from database
              //MemoryStream ms = new MemoryStream();
              //await file.OpenReadStream().CopyToAsync(ms);
              //attachment.Thumbnail = ms.ToArray();
              
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
            var userId = ClaimHelper.GetUserIdFromClaim(User.Claims); 
            _itemService.DeleteItem(userId,id);
        }

   
    }
}
