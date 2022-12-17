using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;
using System.Net;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BrandModelController : BaseController
    {
        public BrandModelController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.BrandModel.GetAllAsync();

            var brandDto = result.Select(x => new BrandModelDto
            {
                Id = x.Id,
                ModelName = x.ModelName,
                ModelNameAr = x.ModelNameAr,
                Brand = new BrandDto
                {
                    Id = x.Brand.Id,
                    Name = x.Brand.Name,
                    NameAr = x.Brand.NameAr,
                    LogoPath = x.Brand.LogoPath,
                }
            }).ToList();

            return Ok(brandDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetBrandModel", Name = "GetBrandModel")]
        public async Task<IActionResult> GetBrandModel([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.BrandModel.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new BrandModelDto
            {
                Id = result.Id,
                ModelName = result.ModelName,
                ModelNameAr = result.ModelNameAr,
                Brand = new BrandDto
                {
                    Id = result.Brand.Id,
                    Name = result.Brand.Name,
                    NameAr = result.Brand.NameAr,
                    LogoPath = result.Brand.LogoPath,
                }
            });
        }
        [HttpGet]
        [Route("GetModelByBrand", Name = "GetModelByBrand")]
        public async Task<IActionResult> GetModelByBrand([FromQuery] Guid bid)
        {
            var result = (await _unitOfWork.BrandModel.FindAllAsync(x => x.BrandId == bid && x.IsDeleted == false)).ToList();

            if (result is null) return NotFound("Item Not Found");



            return Ok(result.Select(x => new BrandModel
            {
                Id = x.Id,
                ModelName = x.ModelName,
                ModelNameAr = x.ModelNameAr,
            }));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddModelRequestDto brandModelRequestDto)
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
            var Added = await _unitOfWork.BrandModel.AddAsync(new BrandModel
            {
                ModelName = brandModelRequestDto.ModelName,
                ModelNameAr = brandModelRequestDto.ModelNameAr,
                BrandId = brandModelRequestDto.BrandId
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



            return CreatedAtRoute("GetBrandModel", new BrandModelRequestDto
            {
                Id = Added.Id,
                ModelName = brandModelRequestDto.ModelName,
                ModelNameAr = brandModelRequestDto.ModelNameAr,
                BrandId = brandModelRequestDto.BrandId
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.BrandModel.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] BrandModelRequestDto modelRequestDto)
        {
            var updated = await _unitOfWork.BrandModel.UpdateAsync(new BrandModel
            {
                Id = modelRequestDto.Id,
                ModelName = modelRequestDto.ModelName,
                ModelNameAr = modelRequestDto.ModelNameAr,
                BrandId = modelRequestDto.BrandId
            }, modelRequestDto.Id);



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
            return Ok(modelRequestDto);
        }
    }
}
