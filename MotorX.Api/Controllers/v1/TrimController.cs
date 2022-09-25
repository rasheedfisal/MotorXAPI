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
    public class TrimController : BaseController
    {
        public TrimController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Trim.GetAllAsync();

            var trimDto = result.Select(x => new TrimDto
            {
                Id = x.Id,
                TrimName = x.TrimName,
                TrimNameAr = x.TrimNameAr
            }).ToList();

            return Ok(trimDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetTrim", Name = "GetTrim")]
        public async Task<IActionResult> GetTrim([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Trim.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new TrimDto
            {
                Id = result.Id,
                TrimName = result.TrimName,
                TrimNameAr = result.TrimNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddTrimDto trimDto)
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
            var Added = await _unitOfWork.Trim.AddAsync(new Trim
            {
                TrimName = trimDto.TrimName,
                TrimNameAr = trimDto.TrimNameAr
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

            return CreatedAtRoute("GetTrim", new TrimDto
            {
                Id = Added.Id,
                TrimName = trimDto.TrimName,
                TrimNameAr = trimDto.TrimNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Trim.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] TrimDto trimDto)
        {
            var updated = await _unitOfWork.Trim.UpdateAsync(new Trim
            {
                Id = trimDto.Id,
                TrimName = trimDto.TrimName,
                TrimNameAr = trimDto.TrimNameAr
            }, trimDto.Id);



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
            return Ok(trimDto);
        }
    }
}
