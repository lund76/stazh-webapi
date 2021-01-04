using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models.Api;
using Stazh.Core.Services;

namespace Stazh.Application.WebApi.Helpers
{
    public class ItemHelper
    {
        public static async Task<Item> CreateItem(ApiItemCreation<IFormFileCollection> data , string userId, IUserService userService, IItemService itemService)
        {
            var item = new Item
            {
                Owner = userService.GetOrCreateUser(userId),
                Description = data.Description,
                Name = data.Name,
                Created = DateTime.UtcNow,
                ItemAttachments = new HashSet<Attachment>()
            };
            if (data.Files == null) return item;
            foreach (var file in data.Files)
            {
                var addedFile = await itemService.AddFile(file.OpenReadStream(), file.FileName, userId);
                var attachment = new Attachment { OriginalFileName = addedFile.OriginalFileName, UniqueAttachmentName = addedFile.UniqueFilename };
                item.ItemAttachments.Add(attachment);
            }


            return item;
        }

        public static HashSet<Item> ResolveChildren(int childItemId, IItemService itemService)
        {
           var child = itemService.GetItem(childItemId);
           var children = new HashSet<Item> {child};
           return children;
        }

        public static Item ResolveParent( int itemId ,IItemService itemService)
        {
            var parent = itemService.FindParentFromId(itemId.ToString());
            return parent;
        }
    }
}
