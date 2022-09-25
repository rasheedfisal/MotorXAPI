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
    public class GearboxController : BaseController
    {
        public GearboxController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Gearbox.GetAllAsync();

            var GearboxDto = result.Select(x => new GearboxDto
            {
                Id = x.Id,
                GearboxName = x.GearboxName,
                GearboxNameAr = x.GearboxNameAr
            }).ToList();

            return Ok(GearboxDto);
        }
       // [AllowAnonymous]
        [HttpGet]
        [Route("GetGearbox", Name = "GetGearbox")]
        public async Task<IActionResult> GetGearbox([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Gearbox.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new GearboxDto
            {
                Id = result.Id,
                GearboxName = result.GearboxName,
                GearboxNameAr = result.GearboxNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddGearboxDto gearboxDto)
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
            var Added = await _unitOfWork.Gearbox.AddAsync(new Gearbox
            {
                GearboxName = gearboxDto.GearboxName,
                GearboxNameAr = gearboxDto.GearboxNameAr
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


            return CreatedAtRoute("GetGearbox", new GearboxDto
            {
                Id = Added.Id,
                GearboxName = gearboxDto.GearboxName,
                GearboxNameAr = gearboxDto.GearboxNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Gearbox.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] GearboxDto gearboxDto)
        {
            var updated = await _unitOfWork.Gearbox.UpdateAsync(new Gearbox
            {
                Id = gearboxDto.Id,
                GearboxName = gearboxDto.GearboxName,
                GearboxNameAr = gearboxDto.GearboxNameAr
            }, gearboxDto.Id);



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
            return Ok(gearboxDto);
        }
    }
}
