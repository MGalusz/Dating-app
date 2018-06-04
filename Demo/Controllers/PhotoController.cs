using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Demo.Data;
using Demo.DTOs;
using Demo.Helpers;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Demo.Controllers {
    [Authorize]
    [Route ("api/users/{userId}/photos")]
    public class PhotoController : Controller {
        private readonly IDatingRepositiry _repo;
        private Cloudinary _cloudinary;
        private readonly IOptions<CloudinartySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;

        public PhotoController (IDatingRepositiry repo,
            IMapper mapper,
            IOptions<CloudinartySettings> cloudinaryConfig) {
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repo;

            Account acc = new Account (
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary (acc);
        }

        [HttpGet ("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto (int id) {
            var photoFromRepo = await _repo.GetPhoto (id);

            var photo = _mapper.Map<PhotoFromReturnDto> (photoFromRepo);

            return Ok (photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser (int userId, PhotoForCreationDto photoDto) {
            var user = await _repo.GetUserAsync (userId);
            if (user == null)
                return BadRequest ("CouldNot find user");

            var currentUserId = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);

            if (currentUserId != user.Id)
                return Unauthorized ();

            var file = photoDto.File;
            var uploadResult = new ImageUploadResult ();
            if (file.Length > 0) {
                using (var stream = file.OpenReadStream ()) {
                    var uploadParams = new ImageUploadParams () {
                    File = new FileDescription (file.Name, stream),
                    Transformation = new Transformation ().Width (500).Height (500).Crop ("fill").Gravity ("face")
                    };
                    uploadResult = _cloudinary.Upload (uploadParams);
                }
            }

            photoDto.Url = uploadResult.Uri.ToString ();
            photoDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo> (photoDto);

            photo.User = user;
            if (!user.Photos.Any (m => m.IsMain))
                photo.IsMain = true;

            user.Photos.Add (photo);

            var photoToReturn = _mapper.Map<PhotoFromReturnDto> (photo);

            if (await _repo.SaveAll ()) {
                return CreatedAtRoute ("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest ("Could not add the photo");

        }

        [HttpPost ("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto (int userId, int id) {
            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();
            var photoFromRepo = await _repo.GetPhoto (id);
            if (photoFromRepo == null)
                return NotFound ();

            if (photoFromRepo.IsMain)
                return BadRequest ("This is already the main Photo");

            var currentMainPhoto = await _repo.GetMainPhotoForUser (userId);
            if (currentMainPhoto != null)
                currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll ())
                return NoContent ();

            return BadRequest ("Could not set Photo to Main");
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeletePhoto (int userId, int id) {

            if (userId != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();
            var photoFromRepo = await _repo.GetPhoto (id);
            if (photoFromRepo == null)
                return NotFound ();
            if (photoFromRepo.IsMain)
                return BadRequest ("You Can not deelet the main photo");

            if (photoFromRepo.PublicId != null) {
                var deleteParams = new DeletionParams (photoFromRepo.PublicId);
                var result = _cloudinary.Destroy (deleteParams);
                if (result.Result == "ok")
                    _repo.Delete (photoFromRepo);
            }

            if (photoFromRepo.PublicId != null) {
                _repo.Delete (photoFromRepo);
            }

            if (await _repo.SaveAll ())
                return Ok ();

            return BadRequest ("Faild to delete the photo");

        }
    }

}