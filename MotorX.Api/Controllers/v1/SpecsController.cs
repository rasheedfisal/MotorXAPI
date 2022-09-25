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
    public class SpecsController : BaseController
    {
        public SpecsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Specs.GetAllAsync();

            var specsDto = result.Select(x => new SpecsDto
            {
                Id = x.Id,
                SpecsName = x.SpecsName,
                SpecsNameAr = x.SpecsNameAr
            }).ToList();

            return Ok(specsDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetSpec", Name = "GetSpec")]
        public async Task<IActionResult> GetSpec([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Specs.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new SpecsDto
            {
                Id = result.Id,
                SpecsName = result.SpecsName,
                SpecsNameAr = result.SpecsNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddSpecsDto specsDto)
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
            var Added = await _unitOfWork.Specs.AddAsync(new Specs
            {
                SpecsName = specsDto.SpecsName,
                SpecsNameAr = specsDto.SpecsNameAr
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

            return CreatedAtRoute("GetSpec", new SpecsDto
            {
                Id = Added.Id,
                SpecsName = specsDto.SpecsName,
                SpecsNameAr = specsDto.SpecsNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Specs.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] SpecsDto specsDto)
        {
            var updated = await _unitOfWork.Specs.UpdateAsync(new Specs
            {
                Id = specsDto.Id,
                SpecsName = specsDto.SpecsName,
                SpecsNameAr = specsDto.SpecsNameAr
            }, specsDto.Id);



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
            return Ok(specsDto);
        }
    }
}
