using CosomoDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosomoDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IDocumentClient _doumentClient;
        readonly string _databaseId;
        readonly string _collectionId;
        public IConfiguration _configuration;

        public ItemController(IDocumentClient doumentClient, IConfiguration configuration)
        {
            _doumentClient = doumentClient;
            _configuration = configuration;
            _databaseId = _configuration["DatabaseId"];
            _collectionId = "Items";
            BuildCollection().Wait();
        }
        private async Task BuildCollection()
        {
            await _doumentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            await _doumentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseId), new DocumentCollection { Id = _collectionId });  
        }

        [HttpGet]
        public IQueryable<Item> Get()
        {
            return _doumentClient.CreateDocumentQuery<Item>(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = 20 });
        }

        [HttpGet("{id}")]
        public IQueryable<Item> Get(string id)
        {
            return _doumentClient.CreateDocumentQuery<Item>(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = 1 }).Where((i) => i.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var response = await _doumentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Item item)
        {
            await _doumentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id),
                item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _doumentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
            return Ok();
        }
    }
}
