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
    public class CarFeaturesController : BaseController
    {
        public CarFeaturesController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetCarFeatures", Name = "GetCarFeatures")]
        public async Task<IActionResult> GetCarFeatures([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.CarFeature.FindAllAsync(x => x.CarOfferId == bid);
            if (result is null) return NotFound("Item Not Found");


            var AllFeatureTypes = await _unitOfWork.FeatureType.GetAllAsync();
            var features = await _unitOfWork.Features.GetAllAsync();


            var carsFeature = new List<FeaturetypeAllDto>();


            foreach (var featureType in AllFeatureTypes)
            {
                var allFeatures = features.Where(x => x.FeaturetypeId == featureType.Id).ToList();
                //var filteredresult = result
                var setSelectedFeature = new List<FeatureDto>();
                foreach (var feat in allFeatures)
                {
                    var isExist = result.SingleOrDefault(x => x.FeatureId == feat.Id);

                    if (isExist is not null)
                    {
                        setSelectedFeature.Add(new FeatureDto
                        {
                            Id = feat.Id,
                            FeatureName = feat.FeatureName,
                            FeatureNameAr = feat.FeatureNameAr,
                            isSelected = true
                        });
                    }
                    else
                    {
                        setSelectedFeature.Add(new FeatureDto
                        {
                            Id = feat.Id,
                            FeatureName = feat.FeatureName,
                            FeatureNameAr = feat.FeatureNameAr
                        });
                    }
                }
                carsFeature.Add(new FeaturetypeAllDto
                {
                    Id = featureType.Id,
                    TypeName = featureType.TypeName,
                    TypeNameAr = featureType.TypeNameAr,
                    Features = setSelectedFeature
                });
            }
            return Ok(carsFeature);
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] List<CarFeaturesRequestDto> carFeatureRequestsDto, [FromQuery] Guid oid)
        {
            try
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

                var deleteExisting = await _unitOfWork.CarFeature.DeleteAsync(oid);
                if (!deleteExisting)
                {
                    return BadRequest(new ErrorsDto
                    {
                        Errors = new List<string>()
                        {
                            "Error processing request"
                        }
                    });
                }
                foreach (var carFeat in carFeatureRequestsDto)
                {
                    var Added = await _unitOfWork.CarFeature.AddAsync(new CarFeatures
                    {
                        CarOfferId = oid,
                        FeatureId = carFeat.FeatureId
                    });


                }

                await _unitOfWork.CompleteAsync();

                return CreatedAtRoute("GetCarOfferGallary", carFeatureRequestsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                });
            }

        }
    }
}
