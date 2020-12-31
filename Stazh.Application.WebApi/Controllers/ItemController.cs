using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stazh.Application.WebApi.Helpers;
using Stazh.Application.WebApi.Utility;
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
        public async  Task<IActionResult> UploadParent([FromForm] ApiItemCreation<IFormFileCollection> newItem)
        {
            var item = await ItemHelper.CreateItem(newItem, ExtUserId, _userService, _itemService);

            var childItem = ItemHelper.ResolveChildren(newItem.childId,_itemService);
            item.Children = childItem;

            var apiItm = _itemService.InsertNewItem(item);
            return new JsonResult(apiItm);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromForm] ApiItemCreation<IFormFileCollection> data)
        {

            var item = await ItemHelper.CreateItem(data, ExtUserId,_userService,_itemService);
            item.Parent = ItemHelper.ResolveParent(data.ParentId, _itemService);

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
