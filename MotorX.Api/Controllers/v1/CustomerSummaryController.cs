using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Services;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;
using System.Linq;
using System.Linq.Expressions;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerSummaryController : BaseController
    {
        public CustomerSummaryController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.summeryMostViewed.GetAllAsync();

            var summaryDto = result.Select(x => new CustomerSummaryDto
            {
                Id = x.Id,
                ModelName = x.CarOffer.BrandModel.ModelName,
                ModelNameAr = x.CarOffer.BrandModel.ModelNameAr,
                BrandName = x.CarOffer.BrandModel.Brand.Name,
                BrandNameAr = x.CarOffer.BrandModel.Brand.NameAr
            }).ToList();

            return Ok(summaryDto);
        }


        [HttpPost]
        [Route("FindBy")]
        public async Task<IActionResult> FindBy([FromBody] CustomerFilterRequestDto customerFilter)
        {
            var result = await _unitOfWork.summeryMostViewed.GetAllAsync();
            var summaryDto = new List<CustomerSummaryDto>();
            if (customerFilter.HasCriteria())
            {
                var filteredResult = result.AsQueryable().Where(customerFilter.ToExpression());

                summaryDto = filteredResult?.Select(x => new CustomerSummaryDto
                {
                    Id = x.Id,
                    ModelName = x.CarOffer.BrandModel.ModelName,
                    ModelNameAr = x.CarOffer.BrandModel.ModelNameAr,
                    BrandName = x.CarOffer.BrandModel.Brand.Name,
                    BrandNameAr = x.CarOffer.BrandModel.Brand.NameAr
                }).ToList();
            }
            summaryDto = result?.Select(x => new CustomerSummaryDto
            {
                Id = x.Id,
                ModelName = x.CarOffer.BrandModel.ModelName,
                ModelNameAr = x.CarOffer.BrandModel.ModelNameAr,
                BrandName = x.CarOffer.BrandModel.Brand.Name,
                BrandNameAr = x.CarOffer.BrandModel.Brand.NameAr
            }).ToList();

            return Ok(summaryDto);
        }

        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> Upsert([FromBody] Guid OfferId)
        {
            var result = await _unitOfWork.summeryMostViewed.UpsertAsync(OfferId);

            if (!result)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Error Processing Request"
                    }
                });
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetCount")]
        public async Task<IActionResult> GetCount()
        {
            var result = await _unitOfWork.summeryMostViewed.CountAsync();


            return Ok(result);
        }

        [HttpGet]
        [Route("GetCategoryStats")]
        public async Task<IActionResult> GetCategoryStats()
        {
            //var list = new[] { o, o1 }.ToList();
            var cat = new List<string>();
            var OfferClientPerview = new List<int>();
            var OfferPerview = new List<int>();

            //var mostViewed = await _unitOfWork.summeryMostViewed.GetAllAsync();
            var categories = await _unitOfWork.CarType.FindAllAsync(x => x.IsDeleted == false);
           
            foreach (var category in categories)
            {
                cat.Add(category.TypeName);
                var offerCategory = await _unitOfWork.CarOffer.FindAllAsync(x => x.IsDeleted == false && x.CarTypeId == category.Id);
              

                var ids = offerCategory.Select(x => x.Id).ToList();

                OfferClientPerview.Add(await _unitOfWork.summeryMostViewed.CountAsync(x => ids.Contains(x.CarOfferId)));
                OfferPerview.Add(offerCategory.Count());

            }


            return Ok(new { CategoryNames = cat, UserStats = OfferClientPerview, OfferStats = OfferPerview });
        }

    }
}
