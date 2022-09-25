using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Services;
using MotorX.Api.Utils;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;
using System.Net.Http.Headers;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BrandController : BaseController
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IUriService _uriService;
        public BrandController(IUnitOfWork unitOfWork, IImageProcessor imageProcessor, IUriService uriService) : base(unitOfWork)
        {
            _imageProcessor = imageProcessor;
            _uriService = uriService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Brand.GetAllAsync();
            var brandDto = result.Select(x => new BrandDto
            {
                Id = x.Id,
                Name = x.Name,
                NameAr = x.NameAr,
                LogoPath = string.IsNullOrEmpty(x.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{x.LogoPath.Replace("\\", "/")}",
            }).ToList();

            return Ok(brandDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetBrand", Name = "GetBrand")]
        public async Task<IActionResult> GetBrand([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Brand.GetAsync(bid);
           
            if (result is null) return NotFound("Item Not Found");


            return Ok(new BrandDto
            {
                Id = result.Id,
                Name = result.Name!,
                NameAr = result.NameAr,
                LogoPath = string.IsNullOrEmpty(result.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{result.LogoPath.Replace("\\", "/")}",
            });
        }


        //[HttpPost, DisableRequestSizeLimit]
        [HttpPost]
        [RequestSizeLimit(bytes: 5_000_000)]
        [Route("Add")]
        public async Task<IActionResult> Add([FromForm] AddBrandRequestDto brandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "fill in required fields"
                    }
                });
            }


            var Added = await _unitOfWork.Brand.AddAsync(new Brand
            {
                Name = brandDto.Name,
                NameAr = brandDto.NameAr,
                LogoPath = await _imageProcessor.SaveImageAsync(brandDto.Logo)//CreateImgPath(brandDto.Logo)
            }); 


            if (Added is null)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Error Processing Request"
                    }
                });
            }
            await _unitOfWork.CompleteAsync();


            var BrandResponse = new BrandDto
            {
                Id = Added.Id,
                Name = Added.Name!,
                NameAr = Added.NameAr,
                LogoPath = string.IsNullOrEmpty(Added.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{Added.LogoPath.Replace("\\", "/")}",
            };

            return CreatedAtRoute("GetBrand", BrandResponse);
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {

            var checkIfResourseExist = await _unitOfWork.Brand.GetAsync(bid);
            if (checkIfResourseExist?.LogoPath is not null)
            {
                await _imageProcessor.RemoveImageAsync(checkIfResourseExist.LogoPath);
            }
            var IsDeleted = await _unitOfWork.Brand.DeleteAsync(bid);

            if (!IsDeleted)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Error Processing Request"
                    }
                });
            }
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromForm] BrandRequestDto brandDto)
        {
            if (brandDto.Logo is not null)
            {
                var GetExistingLogo = await _unitOfWork.Brand.GetAsync(brandDto.Id);
                
                if (GetExistingLogo?.LogoPath is not null)
                {
                    await _imageProcessor.RemoveImageAsync(GetExistingLogo.LogoPath);
                }
            }

            var updateBrandwithLogo = new Brand
            {
                Id = brandDto.Id,
                Name = brandDto.Name,
                NameAr = brandDto.NameAr,
                LogoPath = brandDto.Logo is not null ? await _imageProcessor.SaveImageAsync(brandDto.Logo) : null,
            };
            var updated = await _unitOfWork.Brand.UpdateAsync(updateBrandwithLogo, brandDto.Id);



            if (updated is null)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Error Processing Request"
                    }
                });
            }
            await _unitOfWork.CompleteAsync();
            var BrandResponse = new BrandDto
            {
                Id = updated.Id,
                Name = updated.Name!,
                NameAr = updated.NameAr,
                LogoPath = string.IsNullOrEmpty(updated.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{updated.LogoPath.Replace("\\", "/")}",
            };

            return Ok(BrandResponse);
        }

        //private async Task<string?> CreateImgPath(IFormFile? file)
        //{
        //    if (file is not null)
        //    {
        //        var folderName = Path.Combine("Resources", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        string? dbPath = null;
        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
        //            //var fileExt = Path.GetExtension(fileName);

        //            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

        //            var fullPath = Path.Combine(pathToSave, uniqueFileName);

        //            dbPath = Path.Combine(folderName, uniqueFileName);
        //            using FileStream? outputFile = new FileStream(fullPath, FileMode.Create);
        //            //await file.CopyToAsync(stream);
        //            var originalFile = file.OpenReadStream();
        //            // var imageUtil = new ImageUtil(stream);
        //            Resize.SaveImage(originalFile, 300, 200, true, outputFile);

        //        }
        //        return dbPath;
        //    }
        //    return null;
        //}
    }
}
