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
    public class ColorsController : BaseController
    {
        public ColorsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Colors.GetAllAsync();

            var colorDto = result.Select(x => new ColorsDto
            {
                Id = x.Id,
                ColorName = x.ColorName,
                ColorNameAr = x.ColorNameAr
            }).ToList();

            return Ok(colorDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetColor", Name = "GetColor")]
        public async Task<IActionResult> GetColor([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Colors.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new ColorsDto
            {
                Id = result.Id,
                ColorName = result.ColorName,
                ColorNameAr = result.ColorNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddColorsDto colorsDto)
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
            var Added = await _unitOfWork.Colors.AddAsync(new Colors
            {
                ColorName = colorsDto.ColorName,
                ColorNameAr = colorsDto.ColorNameAr
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

            return CreatedAtRoute("GetColor", new ColorsDto
            {
                Id = Added.Id,
                ColorName = colorsDto.ColorName,
                ColorNameAr = colorsDto.ColorNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Colors.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] ColorsDto colorsDto)
        {
            var updated = await _unitOfWork.Colors.UpdateAsync(new Colors
            {
                Id = colorsDto.Id,
                ColorName = colorsDto.ColorName,
                ColorNameAr = colorsDto.ColorNameAr
            }, colorsDto.Id);



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
            return Ok(colorsDto);
        }
    }
}
