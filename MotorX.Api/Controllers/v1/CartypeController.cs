using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Services;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;
using System.Net.Http.Headers;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartypeController : BaseController
    {
        private readonly IUriService _uriService;
        private readonly IImageProcessor _imageProcessor;
        public CartypeController(IUnitOfWork unitOfWork, IUriService uriService, IImageProcessor imageProcessor) : base(unitOfWork)
        {
            _uriService = uriService;
            _imageProcessor = imageProcessor;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.CarType.GetAllAsync();
            var cartypeDto = result.Select(x => new CartypeDto
            {
                Id = x.Id,
                TypeName = x.TypeName,
                TypeNameAr = x.TypeNameAr,
                ImgPath = string.IsNullOrEmpty(x.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.ImgPath!.Replace("\\", "/")}",
            }).ToList();

            return Ok(cartypeDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCartype", Name = "GetCartype")]
        public async Task<IActionResult> GetCartype([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.CarType.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");

            var CarTypeResponse = new CartypeDto
            {
                Id = result.Id,
                TypeName = result.TypeName,
                TypeNameAr = result.TypeNameAr,
                ImgPath = string.IsNullOrEmpty(result.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{result.ImgPath!.Replace("\\", "/")}",
            };

            return Ok(CarTypeResponse);
        }

        [HttpPost]
        [RequestSizeLimit(bytes: 5_000_000)]
        [Route("Add")]
        public async Task<IActionResult> Add([FromForm] CarTypeRequestDto cartypeDto)
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
            var Added = await _unitOfWork.CarType.AddAsync(new Cartype
            {
                TypeName = cartypeDto.TypeName,
                TypeNameAr = cartypeDto.TypeNameAr,
                ImgPath = await _imageProcessor.SaveImageAsync(cartypeDto.ImgPath)//CreateImgPath(cartypeDto.ImgPath!)
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

            var CarTypeResponse = new CartypeDto
            {
                Id = Added.Id,
                TypeName = Added.TypeName,
                TypeNameAr = Added.TypeNameAr,
                ImgPath = $"{_uriService.GetBaseRoot()}/{Added.ImgPath!.Replace("\\", "/")}",
            };
            return CreatedAtRoute("GetCartype", CarTypeResponse);
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.CarType.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromForm] UpdateCarTypeRequestDto updatedCartypeDto)
        {
            if (updatedCartypeDto.ImgPath is not null)
            {
                var GetExistingLogo = await _unitOfWork.CarType.GetAsync(updatedCartypeDto.Id);

                if (GetExistingLogo?.ImgPath is not null)
                {
                    await _imageProcessor.RemoveImageAsync(GetExistingLogo.ImgPath);
                }
            }
            var cartypeWithLogo = new Cartype
            {
                Id = updatedCartypeDto.Id,
                TypeName = updatedCartypeDto.TypeName,
                TypeNameAr = updatedCartypeDto.TypeNameAr,
                ImgPath = updatedCartypeDto.ImgPath is not null ? await _imageProcessor.SaveImageAsync(updatedCartypeDto.ImgPath) : null,
            };
            var updated = await _unitOfWork.CarType.UpdateAsync(cartypeWithLogo, updatedCartypeDto.Id);



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
            var CarTypeResponse = new CartypeDto
            {
                Id = updated.Id,
                TypeName = updated.TypeName,
                TypeNameAr = updated.TypeNameAr,
                ImgPath = string.IsNullOrEmpty(updated.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{updated.ImgPath!.Replace("\\", "/")}",
            };

            return Ok(CarTypeResponse);
        }
    }
}
