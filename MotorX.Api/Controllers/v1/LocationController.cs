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
    public class LocationController : BaseController
    {
        public LocationController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Location.GetAllAsync();

            var locationDto = result.Select(x => new LocationDto
            {
                Id = x.Id,
                LocationName = x.LocationName,
                LocationNameAr = x.LocationNameAr
            }).ToList();

            return Ok(locationDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetLocation", Name = "GetLocation")]
        public async Task<IActionResult> GetLocation([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Location.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new LocationDto
            {
                Id = result.Id,
                LocationName = result.LocationName,
                LocationNameAr = result.LocationNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddLocationDto locationDto)
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
            var Added = await _unitOfWork.Location.AddAsync(new Location
            {
                LocationName = locationDto.LocationName,
                LocationNameAr = locationDto.LocationNameAr
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

            return CreatedAtRoute("GetLocation", new LocationDto
            {
                Id = Added.Id,
                LocationName = locationDto.LocationName,
                LocationNameAr = locationDto.LocationNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Location.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] LocationDto locationDto)
        {
            var updated = await _unitOfWork.Location.UpdateAsync(new Location
            {
                Id = locationDto.Id,
                LocationName = locationDto.LocationName,
                LocationNameAr = locationDto.LocationNameAr
            }, locationDto.Id);



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
            return Ok(locationDto);
        }
    }
}
