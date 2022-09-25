using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Requests.Queries;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Helpers;
using MotorX.Api.Services;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;

namespace MotorX.Api.Controllers.v1
{
    public class CusomerInfoController : BaseController
    {

        private readonly IUriService _uriService;
        public CusomerInfoController(IUnitOfWork unitOfWork, IUriService uriService) : base(unitOfWork)
        {
            _uriService = uriService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var baseURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
            var route = Request.Path.Value;

            var result = await _unitOfWork.CustomerInfo.GetAllAsync();

            var customerDto = result.Select(x => new CustomerOfferReceiveDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Address = x.Address,
                PhoneNo = x.PhoneNo,
                Email = x.Email ?? null,
                MarkAsRead = x.MarkAsRead,
                Offer =  new MGetAllOfferDto
                {
                    Id = x.Id,
                    Description = x.Offer.Description,
                    BrandModel = new BrandModelDto
                    {
                        Id = x.Offer.BrandModel.Id,
                        ModelName = x.Offer.BrandModel.ModelName,
                        ModelNameAr = x.Offer.BrandModel.ModelNameAr,
                        //Brand = new BrandDto
                        //{
                        //    Id = x.Offer.BrandModel.Brand.Id,
                        //    Name = x.Offer.BrandModel.Brand.Name,
                        //    NameAr = x.Offer.BrandModel.Brand.NameAr,
                        //    LogoPath = string.IsNullOrEmpty(x.Offer.BrandModel.Brand.LogoPath) ? null : $"{baseURL}/{x.Offer.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                        //}
                    },
                    Year = new YearDto
                    {
                        Id = x.Offer.Year.Id,
                        YearName = x.Offer.Year.YearName
                    },
                    Seats = x.Offer.Seats,
                    Kilometer = x.Offer.Kilometer ?? null,
                    Price = x.Offer.Price,
                    Currency = new CurrencyDto
                    {
                        Id = x.Offer.Currency.Id,
                        CurrencyName = x.Offer.Currency.CurrencyName,
                        CurrencyNameAr = x.Offer.Currency.CurrencyNameAr
                    },
                    IsActive = x.Offer.IsActive,
                    YTLink = x.Offer.YTLink ?? null
                }
            }).ToList();

            var totalRecords = await _unitOfWork.CustomerInfo.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse(customerDto, paginationQuery, totalRecords, _uriService, route!);

            return Ok(pagedResponse);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCustomer", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromQuery] Guid bid)
        {
            var baseURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            var result = await _unitOfWork.CustomerInfo.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");



            return Ok(new CustomerOfferDto
            {
                Id = result.Id,
                FullName = result.FullName,
                Address = result.Address,
                PhoneNo = result.PhoneNo,
                Email = result.Email ?? null,
                Offer = new MGetAllOfferDto
                {
                    Id = result.Id,
                    Description = result.Offer.Description,
                    BrandModel = new BrandModelDto
                    {
                        Id = result.Offer.BrandModel.Id,
                        ModelName = result.Offer.BrandModel.ModelName,
                        ModelNameAr = result.Offer.BrandModel.ModelNameAr,
                        //Brand = new BrandDto
                        //{
                        //    Id = result.Offer.BrandModel.Brand.Id,
                        //    Name = result.Offer.BrandModel.Brand.Name,
                        //    NameAr = result.Offer.BrandModel.Brand.NameAr,
                        //    LogoPath = string.IsNullOrEmpty(result.Offer.BrandModel.Brand.LogoPath) ? null : $"{baseURL}/{result.Offer.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                        //}
                    },
                    Year = new YearDto
                    {
                        Id = result.Offer.Year.Id,
                        YearName = result.Offer.Year.YearName
                    },
                    Seats = result.Offer.Seats,
                    Kilometer = result.Offer.Kilometer ?? null,
                    Price = result.Offer.Price,
                    Currency = new CurrencyDto
                    {
                        Id = result.Offer.Currency.Id,
                        CurrencyName = result.Offer.Currency.CurrencyName,
                        CurrencyNameAr = result.Offer.Currency.CurrencyNameAr
                    },
                    IsActive = result.Offer.IsActive,
                    YTLink = result.Offer.YTLink ?? null
                }
            });
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] CustomerOfferRequestDto customerOffer)
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
            var Added = await _unitOfWork.CustomerInfo.AddAsync(new OfferCustomerInfo
            {
                FullName = customerOffer.FullName,
                Address = customerOffer.Address,
                PhoneNo = customerOffer.PhoneNo,
                Email = customerOffer.Email,
                MarkAsRead = false
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

            var AddedCustomer = new CustomerOfferDto
            {
                Id = Added.Id,
                FullName = Added.FullName,
                Address = Added.Address,
                PhoneNo = Added.PhoneNo,
                Email = Added.Email,
            };

            return CreatedAtRoute("GetCustomer", AddedCustomer);
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.CustomerInfo.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] CustomerOfferReceiveDto customerOffer)
        {
            var updated = await _unitOfWork.CustomerInfo.UpdateAsync(new OfferCustomerInfo
            {
                Id = customerOffer.Id,
                FullName = customerOffer.FullName,
                Address = customerOffer.Address,
                PhoneNo = customerOffer.PhoneNo,
                Email = customerOffer.Email,
                MarkAsRead = customerOffer.MarkAsRead
            }, customerOffer.Id);



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
            return Ok(customerOffer);
        }

        [HttpPatch]
        [Route("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead([FromBody] Guid CustomerInfoId)
        {
            var updated = await _unitOfWork.CustomerInfo.UpdateMarkAsRead(CustomerInfoId);

            if (!updated)
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
    }
}
