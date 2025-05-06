using fiap_mongo.Model;
using fiap_mongo.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace fiap_mongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MongoService _mongoService;

        public UserController(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await _mongoService.GetAsync();
            var result = MapToResult(users);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User usuario)
        {
            await _mongoService.CreateAsync(usuario);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("ID inválido");

            var usuario = await _mongoService.GetByIdAsync(objectId);

            if (usuario is null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, User usuarioAtualizado)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("ID inválido");

            var usuarioExistente = await _mongoService.GetByIdAsync(objectId);

            if (usuarioExistente is null)
                return NotFound();

            usuarioAtualizado.Id = objectId;
            await _mongoService.UpdateAsync(objectId, usuarioAtualizado);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("ID inválido");

            var usuario = await _mongoService.GetByIdAsync(objectId);

            if (usuario is null)
                return NotFound();

            await _mongoService.DeleteAsync(objectId);
            return NoContent();
        }
        #region map
        private object MapToResult(List<User> users)
        {
            List<UserDto> result = new List<UserDto>();
            foreach (var user in users)
            {
                result.Add(new UserDto { Id = user.Id.ToString(), Nome = user.Nome });
            }
            
            return result;
        }
        #endregion
    }
    public class UserDto 
    {
        public string Id { get; set; }
        public string Nome { get; set; }
    }
}
