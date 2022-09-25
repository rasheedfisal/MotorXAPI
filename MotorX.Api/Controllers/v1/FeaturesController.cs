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
    public class FeaturesController : BaseController
    {
        public FeaturesController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetFeatures", Name = "GetFeatures")]
        public async Task<IActionResult> GetFeatures()
        {
            var AllFeaturetypes = await _unitOfWork.FeatureType.GetAllAsync();
            var feature = await _unitOfWork.Features.GetAllAsync();

            if (AllFeaturetypes is null) return NotFound("Item Not Found");

            var allFeatures = new List<FeaturetypeAllDto>();

            foreach (var featureType in AllFeaturetypes)
            {
                var features = feature.Where(x => x.FeaturetypeId == featureType.Id).ToList();
                allFeatures.Add(new FeaturetypeAllDto
                {
                    Id = featureType.Id,
                    TypeName = featureType.TypeName,
                    TypeNameAr = featureType.TypeNameAr,
                    Features = features.Select(x => new FeatureDto
                    {
                        Id = x.Id,
                        FeatureName = x.FeatureName,
                        FeatureNameAr = x.FeatureNameAr
                    }).ToList()
                });
            }

            return Ok(allFeatures);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Features.GetAllAsync();

            var featuresDto = result.Select(x => new FeaturesDto
            {
                Id = x.Id,
                FeatureName = x.FeatureName,
                FeatureNameAr = x.FeatureNameAr,
                FeaturesType = new FeatureTypesDto
                {
                    Id = x.FeaturesType.Id,
                    TypeName = x.FeaturesType.TypeName,
                    TypeNameAr = x.FeaturesType.TypeNameAr
                }
            }).ToList();

            return Ok(featuresDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetFeature", Name = "GetFeature")]
        public async Task<IActionResult> GetFeature([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Features.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new FeaturesDto
            {
                Id = result.Id,
                FeatureName = result.FeatureName,
                FeatureNameAr = result.FeatureNameAr,
                FeaturesType = new FeatureTypesDto
                {
                    Id = result.FeaturesType.Id,
                    TypeName = result.FeaturesType.TypeName,
                    TypeNameAr = result.FeaturesType.TypeNameAr
                }
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddFeatureDto featuresRequestDto)
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
            var Added = await _unitOfWork.Features.AddAsync(new Features
            {
                FeatureName = featuresRequestDto.FeatureName,
                FeatureNameAr = featuresRequestDto.FeatureNameAr,
                FeaturetypeId = featuresRequestDto.FeaturetypeId
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

            return CreatedAtRoute("GetFeature", new FeaturesRequestDto
            {
                Id = Added.Id,
                FeatureName = featuresRequestDto.FeatureName,
                FeatureNameAr = featuresRequestDto.FeatureNameAr,
                FeaturetypeId = featuresRequestDto.FeaturetypeId
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Features.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] FeaturesRequestDto featuresRequestDto)
        {
            var updated = await _unitOfWork.Features.UpdateAsync(new Features
            {
                Id = featuresRequestDto.Id,
                FeatureName = featuresRequestDto.FeatureName,
                FeatureNameAr = featuresRequestDto.FeatureNameAr,
                FeaturetypeId = featuresRequestDto.FeaturetypeId
            }, featuresRequestDto.Id);



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
            return Ok(featuresRequestDto);
        }
    }
}
