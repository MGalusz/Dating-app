using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Data;
using Demo.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers {
    // [Authorize]
    [Route ("api/[controller]")]
    public class UsersController : Controller {
        private readonly IDatingRepositiry _repo;
        private readonly IMapper _mapper;
        public UsersController (IDatingRepositiry repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers () {
            var users = await _repo.GetUsersAsync ();
            var userToReturn = _mapper.Map<IEnumerable<UserForListDto>> (users);
            return Ok (userToReturn);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetUser (int id) {
            var user = await _repo.GetUserAsync (id);
            var userToReturn = _mapper.Map<UserForDetailedDto> (user);
            return Ok (userToReturn);

        }
        //api/users/1 PUT:
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateUser (int id, [FromBody] UserForUpdateDto userForUpdateDto) {
            // int a = 10;
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            var currentUserId = int.Parse (User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUserAsync(id);

            if (userFromRepo == null) {
                return NotFound ($"Could not find user with ID of {id}");
            }
            if (currentUserId != userFromRepo.Id) {
                return Unauthorized ();
            }
            _mapper.Map (userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll ()) {
                return NoContent ();
            }
            throw new Exception ($"Updating use {id} failed on server");
        }
    }

}