using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeatureTypesController : BaseController
    {
        public FeatureTypesController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.FeatureType.GetAllAsync();

            var featureTypesDto = result.Select(x => new FeatureTypesDto
            {
                Id = x.Id,
                TypeName = x.TypeName,
                TypeNameAr = x.TypeNameAr
            }).ToList();

            return Ok(featureTypesDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetFeatureType", Name = "GetFeatureType")]
        public async Task<IActionResult> GetById([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.FeatureType.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new FeatureTypesDto
            {
                Id = result.Id,
                TypeName = result.TypeName,
                TypeNameAr = result.TypeNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddFeatureTypeDto featureTypesDto)
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
            var Added = await _unitOfWork.FeatureType.AddAsync(new FeaturesType
            {
                TypeName = featureTypesDto.TypeName,
                TypeNameAr = featureTypesDto.TypeNameAr
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

            return CreatedAtRoute("GetFeatureType", new FeatureTypesDto
            {
                Id = Added.Id,
                TypeName = featureTypesDto.TypeName,
                TypeNameAr = featureTypesDto.TypeNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.FeatureType.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] FeatureTypesDto featureTypesDto)
        {
            var updated = await _unitOfWork.FeatureType.UpdateAsync(new FeaturesType
            {
                Id = featureTypesDto.Id,
                TypeName = featureTypesDto.TypeName,
                TypeNameAr = featureTypesDto.TypeNameAr
            }, featureTypesDto.Id);



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
            return Ok(featureTypesDto);
        }
    }
}
