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
    public class YearController : BaseController
    {
        public YearController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Year.GetAllAsync();

            var yearDto = result.Select(x => new YearDto
            {
                Id = x.Id,
                YearName = x.YearName
            }).ToList();

            return Ok(yearDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetYear", Name = "GetYear")]
        public async Task<IActionResult> GetYear([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Year.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new YearDto
            {
                Id = result.Id,
                YearName = result.YearName
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddYearsDto yearDto)
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
            var Added = await _unitOfWork.Year.AddAsync(new Year
            {
                YearName = yearDto.YearName
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

            return CreatedAtRoute("GetYear", new YearDto
            {
                Id = Added.Id,
                YearName = yearDto.YearName
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Year.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] YearDto yearDto)
        {
            var updated = await _unitOfWork.Year.UpdateAsync(new Year
            {
                Id = yearDto.Id,
                YearName = yearDto.YearName
            }, yearDto.Id);



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
            return Ok(yearDto);
        }
    }
}
