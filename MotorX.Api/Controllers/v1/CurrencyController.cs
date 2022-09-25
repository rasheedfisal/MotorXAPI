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
    public class CurrencyController : BaseController
    {
        public CurrencyController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Currency.GetAllAsync();

            var currencyDto = result.Select(x => new CurrencyDto
            {
                Id = x.Id,
                CurrencyName = x.CurrencyName,
                CurrencyNameAr = x.CurrencyNameAr
            }).ToList();

            return Ok(currencyDto);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCurrency", Name = "GetCurrency")]
        public async Task<IActionResult> GetCurrency([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Currency.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new CurrencyDto
            {
                Id = result.Id,
                CurrencyName = result.CurrencyName,
                CurrencyNameAr = result.CurrencyNameAr
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] AddCurrencyDto currencyDto)
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
            var Added = await _unitOfWork.Currency.AddAsync(new Currency
            {
                CurrencyName = currencyDto.CurrencyName,
                CurrencyNameAr = currencyDto.CurrencyNameAr
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

            return CreatedAtRoute("GetCurrency", new CurrencyDto
            {
                Id = Added.Id,
                CurrencyName = currencyDto.CurrencyName,
                CurrencyNameAr = currencyDto.CurrencyNameAr
            });
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.Currency.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] CurrencyDto currencyDto)
        {
            var updated = await _unitOfWork.Currency.UpdateAsync(new Currency
            {
                Id = currencyDto.Id,
                CurrencyName = currencyDto.CurrencyName,
                CurrencyNameAr = currencyDto.CurrencyNameAr
            }, currencyDto.Id);



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
            return Ok(currencyDto);
        }
    }
}
