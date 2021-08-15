using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Catalog.Controller
{
    [ApiController]
    [Route("itemsAsync")]
    public class ItemsAsyncController : ControllerBase
    {
        private readonly IItemsRepository repository;
        public ItemsAsyncController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        //GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
            .Select(item => item.AsDto());
            return items;

        }

        //GET /items/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = (await repository.GetItemAsync(id));

            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        //POST /itemsAsync
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreateDate = DateTime.UtcNow
            };

            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        //PUT /itemsAsync/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = (await repository.GetItemAsync(id));

            if (existingItem is null)
            {
                return NotFound();
            }

            Item updateItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            //     // Item updatedItem = existingItem with
            //     // {
            //     //     Name = itemDto.Name,
            //     //     Price = itemDto.Price
            //     // };

            await repository.UpdateItemAsync(updateItem);
            return NoContent();
        }

        //DELETE /itemsAsync/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = (await repository.GetItemAsync(id));

            if (existingItem is null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }

}