using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Net.Http.Headers;
using MotorX.Extensions;
using System.Security.Claims;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OfferController : BaseController
    {
        private readonly IUriService _uriService;
        private readonly IImageProcessor _imageProcessor;

        public OfferController(IUnitOfWork unitOfWork, IUriService uriService, IImageProcessor imageProcessor) : base(unitOfWork)
        {
            _uriService = uriService;
            _imageProcessor = imageProcessor;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromBody] CarOfferFilterRequestDto carOfferFilter)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
            var route = Request.Path.Value;

            string? searchWord = carOfferFilter.search;
            string? clientNo = carOfferFilter.ClientId;
            var result = string.IsNullOrEmpty(searchWord) && string.IsNullOrEmpty(clientNo)
               ? await _unitOfWork.CarOffer.GetAllAsync(paginationFilter)
               : await _unitOfWork.CarOffer.FindAllAsync(x => x.BrandModel.Brand.Name.ToLower().Contains(searchWord.ToLower())
                    || x.BrandModel.Brand.NameAr.ToLower().Contains(searchWord.ToLower())
                    || x.BrandModel.ModelName.ToLower().Contains(searchWord.ToLower())
                    || x.BrandModel.ModelNameAr.ToLower().Contains(searchWord.ToLower())
                    || x.Cartype.TypeName.ToLower().Contains(searchWord.ToLower())
                    || x.Cartype.TypeNameAr.ToLower().Contains(searchWord.ToLower()), paginationFilter);

            var carOfferDto = result.Select(x => new CarOfferDto
            {
                Id = x.Id,
                Description = x.Description,
                BrandModel = new BrandModelDto
                {
                    Id = x.BrandModel.Id,
                    ModelName = x.BrandModel.ModelName,
                    ModelNameAr = x.BrandModel.ModelNameAr,
                    //Brand = new BrandDto
                    //{
                    //    Id = x.BrandModel.Brand.Id,
                    //    Name = x.BrandModel.Brand.Name,
                    //    NameAr = x.BrandModel.Brand.NameAr,
                    //    LogoPath = string.IsNullOrEmpty(x.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{x.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                    //}
                },
                Trim = new TrimDto
                {
                    Id = x.Trim.Id,
                    TrimName = x.Trim.TrimName,
                    TrimNameAr = x.Trim.TrimNameAr
                } ?? null,
                Year = new YearDto
                {
                    Id = x.Year.Id,
                    YearName = x.Year.YearName
                },
                Colors = new ColorsDto
                {
                    Id = x.Colors.Id,
                    ColorName = x.Colors.ColorName,
                    ColorNameAr = x.Colors.ColorNameAr
                },
                Cartype = new CartypeDto
                {
                    Id = x.Cartype.Id,
                    TypeName = x.Cartype.TypeName,
                    TypeNameAr = x.Cartype.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(x.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.Cartype.ImgPath.Replace("\\", "/")}",
                },
                Gearbox = new GearboxDto
                {
                    Id = x.Gearbox.Id,
                    GearboxName = x.Gearbox.GearboxName,
                    GearboxNameAr = x.Gearbox.GearboxNameAr
                } ?? null,
                Specs = new SpecsDto
                {
                    Id = x.Specs.Id,
                    SpecsName = x.Specs.SpecsName,
                    SpecsNameAr = x.Specs.SpecsNameAr
                } ?? null,
                Location = new LocationDto
                {
                    Id = x.Location.Id,
                    LocationName = x.Location.LocationName,
                    LocationNameAr = x.Location.LocationNameAr
                } ?? null,
                Seats = x.Seats,
                Kilometer = x.Kilometer ?? null,
                Price = x.Price,
                Currency = new CurrencyDto
                {
                    Id = x.Currency.Id,
                    CurrencyName = x.Currency.CurrencyName,
                    CurrencyNameAr = x.Currency.CurrencyNameAr
                },
                IsActive = x.IsActive,
                ImageGallaries = x.ImageGallaries.Select(y => new ImageGallaryDto
                {
                    Id = y.Id,
                    FilePath = $"{_uriService.GetBaseRoot()}/{y.FilePath.Replace("\\", "/")}",
                    OrderNo = y.OrderNo,
                }).ToList(),
                FeaturesTypes = x.CarFeatures.Select(f => new FeaturetypeAllDto
                {
                    Id = f.Features.FeaturesType.Id,
                    TypeName = f.Features.FeaturesType.TypeName,
                    TypeNameAr = f.Features.FeaturesType.TypeNameAr,
                    Features = f.Features.FeaturesType.Features.Where(c => c.Id == f.FeatureId).Select(x => new FeatureDto
                    {
                        Id = x.Id,
                        FeatureName = x.FeatureName,
                        FeatureNameAr = x.FeatureNameAr,
                    }).ToList(),
                }).ToList(),
                YTLink = x.YTLink ?? null,
            }).ToList();
            //var totalRecords = result.Count();
            var totalRecords = await _unitOfWork.CarOffer.CountAsync(x => x.IsDeleted == false);
            var pagedResponse = PaginationHelper.CreatePagedReponse(carOfferDto, paginationQuery, totalRecords, _uriService, route!);

            return Ok(pagedResponse);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("MGetAll")]
        public async Task<IActionResult> MGetAll([FromQuery] PaginationQuery paginationQuery, [FromBody] CarOfferFilterRequestDto carOfferFilter)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
            var route = Request.Path.Value;
            string? searchWord = carOfferFilter.search;
            string? clientNo = carOfferFilter.ClientId;
            //var result = string.IsNullOrEmpty(searchWord)
            //   ? await _unitOfWork.CarOffer.GetMAllOfferAsync(carOfferFilter.ClientId, paginationFilter)
            //   : await _unitOfWork.CarOffer.FindMAllAsync(x => x.BrandModel.Brand.Name.ToLower().Contains(searchWord.ToLower())
            //       || x.BrandModel.Brand.NameAr.ToLower().Contains(searchWord.ToLower())
            //       || x.BrandModel.ModelName.ToLower().Contains(searchWord.ToLower())
            //       || x.BrandModel.ModelNameAr.ToLower().Contains(searchWord.ToLower())
            //       || x.Cartype.TypeName.ToLower().Contains(searchWord.ToLower())
            //       || x.Cartype.TypeNameAr.ToLower().Contains(searchWord.ToLower()), paginationFilter);
            var result = string.IsNullOrEmpty(searchWord)
               ? await _unitOfWork.CarOffer.GetMAllOfferAsync(null, clientNo, paginationFilter)
               : await _unitOfWork.CarOffer.GetMAllOfferAsync(x => x.BrandModel.Brand.Name.ToLower().Contains(searchWord.ToLower())
                   || x.BrandModel.Brand.NameAr.ToLower().Contains(searchWord.ToLower())
                   || x.BrandModel.ModelName.ToLower().Contains(searchWord.ToLower())
                   || x.BrandModel.ModelNameAr.ToLower().Contains(searchWord.ToLower())
                   || x.Cartype.TypeName.ToLower().Contains(searchWord.ToLower())
                   || x.Cartype.TypeNameAr.ToLower().Contains(searchWord.ToLower()), clientNo, paginationFilter);

            var carOfferDto = result.Select(x => new MGetAllOfferDto
            {
                Id = x.Id,
                Description = x.Description,
                BrandModel = new BrandModelDto
                {
                    Id = x.BrandModel.Id,
                    ModelName = x.BrandModel.ModelName,
                    ModelNameAr = x.BrandModel.ModelNameAr,
                    Brand = new BrandDto
                    {
                        Id = x.BrandModel.Brand.Id,
                        Name = x.BrandModel.Brand.Name,
                        NameAr = x.BrandModel.Brand.NameAr,
                        LogoPath = string.IsNullOrEmpty(x.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{x.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                    }
                },
                Year = new YearDto
                {
                    Id = x.Year.Id,
                    YearName = x.Year.YearName
                },
                Seats = x.Seats,
                Kilometer = x.Kilometer ?? null,
                Price = x.Price,
                Currency = new CurrencyDto
                {
                    Id = x.Currency.Id,
                    CurrencyName = x.Currency.CurrencyName,
                    CurrencyNameAr = x.Currency.CurrencyNameAr
                },
                Cartype = new CartypeDto
                {
                    Id = x.Cartype.Id,
                    TypeName = x.Cartype.TypeName,
                    TypeNameAr = x.Cartype.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(x.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.Cartype.ImgPath?.Replace("\\", "/")}",
                },
                IsActive = x.IsActive,
                YTLink = x.YTLink ?? null,
                MainImg = $"{_uriService.GetBaseRoot()}/{x.ImageGallaries?.FirstOrDefault()?.FilePath.Replace("\\", "/")}",
                IsFavorite = x.Favorite is null || x.Favorite.Count == 0 ? false : x.Favorite.First().IsFavorite
            }).ToList();
            //var totalRecords = result.Count();
            var totalRecords = await _unitOfWork.CarOffer.CountAsync(x => x.IsDeleted == false);
            var pagedResponse = PaginationHelper.CreatePagedReponse(carOfferDto, paginationQuery, totalRecords, _uriService, route!);

            return Ok(pagedResponse);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("MGetAllDev")]
        public async Task<IActionResult> MGetAllDev([FromQuery] PaginationQuery paginationQuery, [FromBody] CarOfferFilterRequestDto carOfferFilter)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
            var route = Request.Path.Value;
            string? searchWord = carOfferFilter.search;
            string? clientNo = carOfferFilter.ClientId;
            //var result = string.IsNullOrEmpty(searchWord)
            //   ? await _unitOfWork.CarOffer.GetMAllOfferDevAsync(carOfferFilter.ClientId, paginationFilter)
            //   : await _unitOfWork.CarOffer.FindMAllAsync(x => x.BrandModel.Brand.Name.ToLower().Contains(searchWord.ToLower())
            //       || x.BrandModel.Brand.NameAr.ToLower().Contains(searchWord.ToLower())
            //       || x.BrandModel.ModelName.ToLower().Contains(searchWord.ToLower())
            //       || x.BrandModel.ModelNameAr.ToLower().Contains(searchWord.ToLower())
            //       || x.Cartype.TypeName.ToLower().Contains(searchWord.ToLower())
            //       || x.Cartype.TypeNameAr.ToLower().Contains(searchWord.ToLower()), paginationFilter);

            var result = string.IsNullOrEmpty(searchWord)
              ? await _unitOfWork.CarOffer.GetMAllOfferDevAsync(null, clientNo, paginationFilter)
              : await _unitOfWork.CarOffer.GetMAllOfferDevAsync(x => x.BrandModel.Brand.Name.ToLower().Contains(searchWord.ToLower())
                  || x.BrandModel.Brand.NameAr.ToLower().Contains(searchWord.ToLower())
                  || x.BrandModel.ModelName.ToLower().Contains(searchWord.ToLower())
                  || x.BrandModel.ModelNameAr.ToLower().Contains(searchWord.ToLower())
                  || x.Cartype.TypeName.ToLower().Contains(searchWord.ToLower())
                  || x.Cartype.TypeNameAr.ToLower().Contains(searchWord.ToLower()), clientNo, paginationFilter);

            var carOfferDto = result.Select(x => new MGetAllOfferDto
            {
                Id = x.Id,
                Description = x.Description,
                BrandModel = new BrandModelDto
                {
                    Id = x.BrandModel.Id,
                    ModelName = x.BrandModel.ModelName,
                    ModelNameAr = x.BrandModel.ModelNameAr,
                    Brand = new BrandDto
                    {
                        Id = x.BrandModel.Brand.Id,
                        Name = x.BrandModel.Brand.Name,
                        NameAr = x.BrandModel.Brand.NameAr,
                        LogoPath = string.IsNullOrEmpty(x.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{x.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                    }
                },
                Year = new YearDto
                {
                    Id = x.Year.Id,
                    YearName = x.Year.YearName
                },
                Seats = x.Seats,
                Kilometer = x.Kilometer ?? null,
                Price = x.Price,
                Currency = new CurrencyDto
                {
                    Id = x.Currency.Id,
                    CurrencyName = x.Currency.CurrencyName,
                    CurrencyNameAr = x.Currency.CurrencyNameAr
                },
                Cartype = new CartypeDto
                {
                    Id = x.Cartype.Id,
                    TypeName = x.Cartype.TypeName,
                    TypeNameAr = x.Cartype.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(x.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.Cartype.ImgPath?.Replace("\\", "/")}",
                },
                IsActive = x.IsActive,
                YTLink = x.YTLink ?? null,
                MainImg = $"{_uriService.GetBaseRoot()}/{x.ImageGallaries?.FirstOrDefault()?.FilePath.Replace("\\", "/")}",
                IsFavorite = x.Favorite is null || x.Favorite.Count == 0 ? false : x.Favorite.First().IsFavorite
            }).ToList();


            //var totalRecords = result.Count();
            var totalRecords = await _unitOfWork.CarOffer.CountAsync(x => x.IsDeleted == false);
            var pagedResponse = PaginationHelper.CreatePagedReponse(carOfferDto, paginationQuery, totalRecords, _uriService, route!);

            return Ok(pagedResponse);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("MGetOffersByCarType")]
        public async Task<IActionResult> MGetOffersByCarType([FromQuery] PaginationQuery paginationQuery, [FromBody] CarOfferFilterRequestDto carOfferFilter)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };
            var route = Request.Path.Value;
            string? searchWord = carOfferFilter.search;
            string? clientNo = carOfferFilter.ClientId;
            //var result = string.IsNullOrEmpty(searchWord)
            //   ? await _unitOfWork.CarOffer.GetMAllOfferAsync(carOfferFilter.ClientId, paginationFilter)
            //   : await _unitOfWork.CarOffer.FindMAllAsync(x => x.Cartype.Id == Guid.Parse(searchWord), paginationFilter);

            var result = string.IsNullOrEmpty(searchWord)
               ? await _unitOfWork.CarOffer.GetMAllOfferAsync(null, clientNo, paginationFilter)
               : await _unitOfWork.CarOffer.GetMAllOfferAsync(x => x.Cartype.Id == Guid.Parse(searchWord), clientNo, paginationFilter);

            var carOfferDto = result.Select(x => new MGetAllOfferDto
            {
                Id = x.Id,
                Description = x.Description,
                BrandModel = new BrandModelDto
                {
                    Id = x.BrandModel.Id,
                    ModelName = x.BrandModel.ModelName,
                    ModelNameAr = x.BrandModel.ModelNameAr,
                    Brand = new BrandDto
                    {
                        Id = x.BrandModel.Brand.Id,
                        Name = x.BrandModel.Brand.Name,
                        NameAr = x.BrandModel.Brand.NameAr,
                        LogoPath = string.IsNullOrEmpty(x.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{x.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                    }
                },
                Year = new YearDto
                {
                    Id = x.Year.Id,
                    YearName = x.Year.YearName
                },
                Seats = x.Seats,
                Kilometer = x.Kilometer ?? null,
                Price = x.Price,
                Currency = new CurrencyDto
                {
                    Id = x.Currency.Id,
                    CurrencyName = x.Currency.CurrencyName,
                    CurrencyNameAr = x.Currency.CurrencyNameAr
                },
                Cartype = new CartypeDto
                {
                    Id = x.Cartype.Id,
                    TypeName = x.Cartype.TypeName,
                    TypeNameAr = x.Cartype.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(x.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.Cartype.ImgPath?.Replace("\\", "/")}",
                },
                IsActive = x.IsActive,
                YTLink = x.YTLink ?? null,
                MainImg = $"{_uriService.GetBaseRoot()}/{x.ImageGallaries?.FirstOrDefault()?.FilePath.Replace("\\", "/")}",
                IsFavorite = x.Favorite is null || x.Favorite.Count == 0 ? false : x.Favorite.First().IsFavorite
            }).ToList();

            //var totalRecords = result.Count();

            var totalRecords = await _unitOfWork.CarOffer.CountAsync(x => x.IsDeleted == false);
            var pagedResponse = PaginationHelper.CreatePagedReponse(carOfferDto, paginationQuery, totalRecords, _uriService, route!);

            return Ok(pagedResponse);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetOffer", Name = "GetOffer")]
        public async Task<IActionResult> GetOffer([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.CarOffer.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");


            return Ok(new CarOfferDto
            {
                Id = result.Id,
                Description = result.Description,
                BrandModel = new BrandModelDto
                {
                    Id = result.BrandModel.Id,
                    ModelName = result.BrandModel.ModelName,
                    ModelNameAr = result.BrandModel.ModelNameAr,
                    Brand = new BrandDto
                    {
                        Id = result.BrandModel.Brand.Id,
                        Name = result.BrandModel.Brand.Name,
                        NameAr = result.BrandModel.Brand.NameAr,
                        LogoPath = string.IsNullOrEmpty(result.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{result.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                    }
                },
                Trim = new TrimDto
                {
                    Id = result.Trim.Id,
                    TrimName = result.Trim.TrimName,
                    TrimNameAr = result.Trim.TrimNameAr
                } ?? null,
                Year = new YearDto
                {
                    Id = result.Year.Id,
                    YearName = result.Year.YearName
                },
                Colors = new ColorsDto
                {
                    Id = result.Colors.Id,
                    ColorName = result.Colors.ColorName,
                    ColorNameAr = result.Colors.ColorNameAr
                },
                Cartype = new CartypeDto
                {
                    Id = result.Cartype.Id,
                    TypeName = result.Cartype.TypeName,
                    TypeNameAr = result.Cartype.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(result.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{result.Cartype.ImgPath.Replace("\\", "/")}",
                },
                Gearbox = new GearboxDto
                {
                    Id = result.Gearbox.Id,
                    GearboxName = result.Gearbox.GearboxName,
                    GearboxNameAr = result.Gearbox.GearboxNameAr
                } ?? null,
                Specs = new SpecsDto
                {
                    Id = result.Specs.Id,
                    SpecsName = result.Specs.SpecsName,
                    SpecsNameAr = result.Specs.SpecsNameAr
                } ?? null,
                Location = new LocationDto
                {
                    Id = result.Location.Id,
                    LocationName = result.Location.LocationName,
                    LocationNameAr = result.Location.LocationNameAr
                } ?? null,
                Seats = result.Seats,
                Kilometer = result.Kilometer ?? null,
                Price = result.Price,
                Currency = new CurrencyDto
                {
                    Id = result.Currency.Id,
                    CurrencyName = result.Currency.CurrencyName,
                    CurrencyNameAr = result.Currency.CurrencyNameAr
                },
                IsActive = result.IsActive,
                ImageGallaries = result.ImageGallaries.Select(y => new ImageGallaryDto
                {
                    Id = y.Id,
                    FilePath = $"{_uriService.GetBaseRoot()}/{y.FilePath.Replace("\\", "/")}",
                    OrderNo = y.OrderNo,
                }).ToList(),
                FeaturesTypes = result.CarFeatures.Select(f => new FeaturetypeAllDto
                {
                    Id = f.Features.FeaturesType.Id,
                    TypeName = f.Features.FeaturesType.TypeName,
                    TypeNameAr = f.Features.FeaturesType.TypeNameAr,
                    Features = f.Features.FeaturesType.Features.Where(c => c.Id == f.FeatureId).Select(x => new FeatureDto
                    {
                        Id = x.Id,
                        FeatureName = x.FeatureName,
                        FeatureNameAr = x.FeatureNameAr,
                    }).ToList(),
                }).ToList(),
                YTLink = result.YTLink ?? null,
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UpdateFavorite")]
        public async Task<IActionResult> UpdateFavorite([FromBody] UpdateFavoriteRequestDto updateFavorite)
        {
            if (string.IsNullOrEmpty(updateFavorite.ClientId)) return BadRequest("Client Is Empty");

            var result = await _unitOfWork.CarOffer.FindAsync(x => x.Id == updateFavorite.OfferId);

            if (result is null) return NotFound("Item Not Found");

            var isUpdated = await _unitOfWork.FavoriteOffer.UpsertAsync(updateFavorite.OfferId, updateFavorite.ClientId, updateFavorite.IsFavorite);
            await _unitOfWork.CompleteAsync();
            if (!isUpdated)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Error Processing Request"
                    }
                });
            }

            return Ok(updateFavorite);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("MGetOfferDetails", Name = "MGetOfferDetails")]
        public async Task<IActionResult> MGetOfferDetails([FromQuery] Guid bid, [FromBody] UpdateSummaryDto summaryDto)
        {
            try
            {
                var result = await _unitOfWork.CarOffer.GetMOfferDetailsAsync(bid, summaryDto.ClientId);

                if (result is null) return NotFound("Item Not Found");

                if (summaryDto.UpdateSummary)
                {
                    await _unitOfWork.summeryMostViewed.UpsertAsync(bid);
                }

                await _unitOfWork.CompleteAsync();

                return Ok(new MGetOfferDetailsDto
                {
                    Id = result.Id,
                    Description = result.Description,
                    BrandModel = new BrandModelDto
                    {
                        Id = result.BrandModel.Id,
                        ModelName = result.BrandModel.ModelName,
                        ModelNameAr = result.BrandModel.ModelNameAr,
                        Brand = new BrandDto
                        {
                            Id = result.BrandModel.Brand.Id,
                            Name = result.BrandModel.Brand.Name,
                            NameAr = result.BrandModel.Brand.NameAr,
                            LogoPath = string.IsNullOrEmpty(result.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{result.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                        }
                    },
                    Trim = new TrimDto
                    {
                        Id = result.Trim.Id,
                        TrimName = result.Trim.TrimName,
                        TrimNameAr = result.Trim.TrimNameAr
                    } ?? null,
                    Colors = new ColorsDto
                    {
                        Id = result.Colors.Id,
                        ColorName = result.Colors.ColorName,
                        ColorNameAr = result.Colors.ColorNameAr
                    },
                    Gearbox = new GearboxDto
                    {
                        Id = result.Gearbox.Id,
                        GearboxName = result.Gearbox.GearboxName,
                        GearboxNameAr = result.Gearbox.GearboxNameAr
                    } ?? null,
                    Specs = new SpecsDto
                    {
                        Id = result.Specs.Id,
                        SpecsName = result.Specs.SpecsName,
                        SpecsNameAr = result.Specs.SpecsNameAr
                    } ?? null,
                    Location = new LocationDto
                    {
                        Id = result.Location.Id,
                        LocationName = result.Location.LocationName,
                        LocationNameAr = result.Location.LocationNameAr
                    } ?? null,
                    Year = new YearDto
                    {
                        Id = result.Year.Id,
                        YearName = result.Year.YearName
                    },
                    Seats = result.Seats,
                    Kilometer = result.Kilometer ?? null,
                    Price = result.Price,
                    Currency = new CurrencyDto
                    {
                        Id = result.Currency.Id,
                        CurrencyName = result.Currency.CurrencyName,
                        CurrencyNameAr = result.Currency.CurrencyNameAr
                    },
                    IsActive = result.IsActive,
                    Cartype = new CartypeDto
                    {
                        Id = result.Cartype.Id,
                        TypeName = result.Cartype.TypeName,
                        TypeNameAr = result.Cartype.TypeNameAr,
                        ImgPath = string.IsNullOrEmpty(result.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{result.Cartype.ImgPath.Replace("\\", "/")}",
                    },
                    ImageGallaries = result.ImageGallaries.Select(y => new ImageGallaryDto
                    {
                        Id = y.Id,
                        FilePath = $"{_uriService.GetBaseRoot()}/{y.FilePath.Replace("\\", "/")}",
                        OrderNo = y.OrderNo,
                    }).ToList(),
                    FeaturesTypes = result.CarFeatures.Select(f => new FeaturetypeAllDto
                    {
                        Id = f.Features.FeaturesType.Id,
                        TypeName = f.Features.FeaturesType.TypeName,
                        TypeNameAr = f.Features.FeaturesType.TypeNameAr,
                        Features = f.Features.FeaturesType.Features.Where(c => c.Id == f.FeatureId).Select(x => new FeatureDto
                        {
                            Id = x.Id,
                            FeatureName = x.FeatureName,
                            FeatureNameAr = x.FeatureNameAr,
                        }).ToList(),
                    }).ToList(),
                    YTLink = result.YTLink ?? null,
                    IsFavorite = result.Favorite is null || result.Favorite.Count == 0 ? false : result.Favorite.First().IsFavorite,
                    User = result.AppUser != null ?  new OfferUserResponse
                    {
                        Email = result.AppUser.Email,
                        PhoneNumber = result.AppUser.PhoneNumber,
                    } : null
                });
            }
            catch(Exception ex)
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

        [HttpGet]
        [Route("GetOfferSettings", Name = "GetOfferSettings")]
        public async Task<IActionResult> GetOfferSettings()
        {
            try
            {
                var carType = await _unitOfWork.CarType.GetAllAsync();
                var cartypeDto = carType.Select(x => new CartypeDto
                {
                    Id = x.Id,
                    TypeName = x.TypeName,
                    TypeNameAr = x.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(x.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.ImgPath!.Replace("\\", "/")}",
                }).ToList();
                var brands = await _unitOfWork.Brand.GetAllAsync();
                var brandDto = brands.Select(x => new BrandDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    NameAr = x.NameAr,
                    LogoPath = x.LogoPath,
                }).ToList();
                var models = await _unitOfWork.BrandModel.GetAllAsync();
                var modelDto = models.Select(x => new BrandModelDto
                {
                    Id = x.Id,
                    ModelName = x.ModelName,
                    ModelNameAr = x.ModelNameAr,
                    Brand = new BrandDto
                    {
                        Id = x.Brand.Id,
                        Name = x.Brand.Name,
                        NameAr = x.Brand.NameAr,
                        LogoPath = x.Brand.LogoPath,
                    }
                }).ToList();
                var colors = await _unitOfWork.Colors.GetAllAsync();
                var colorDto = colors.Select(x => new ColorsDto
                {
                    Id = x.Id,
                    ColorName = x.ColorName,
                    ColorNameAr = x.ColorNameAr
                }).ToList();
                var years = await _unitOfWork.Year.GetAllAsync();
                var yearDto = years.Select(x => new YearDto
                {
                    Id = x.Id,
                    YearName = x.YearName
                }).ToList();
                var gearbox = await _unitOfWork.Gearbox.GetAllAsync();
                var GearboxDto = gearbox.Select(x => new GearboxDto
                {
                    Id = x.Id,
                    GearboxName = x.GearboxName,
                    GearboxNameAr = x.GearboxNameAr
                }).ToList();
                var trim = await _unitOfWork.Trim.GetAllAsync();
                var trimDto = trim.Select(x => new TrimDto
                {
                    Id = x.Id,
                    TrimName = x.TrimName,
                    TrimNameAr = x.TrimNameAr
                }).ToList();
                var specs = await _unitOfWork.Specs.GetAllAsync();
                var specsDto = specs.Select(x => new SpecsDto
                {
                    Id = x.Id,
                    SpecsName = x.SpecsName,
                    SpecsNameAr = x.SpecsNameAr
                }).ToList();
                var locations = await _unitOfWork.Location.GetAllAsync();
                var locationDto = locations.Select(x => new LocationDto
                {
                    Id = x.Id,
                    LocationName = x.LocationName,
                    LocationNameAr = x.LocationNameAr
                }).ToList();
                var currency = await _unitOfWork.Currency.GetAllAsync();
                var currencyDto = currency.Select(x => new CurrencyDto
                {
                    Id = x.Id,
                    CurrencyName = x.CurrencyName,
                    CurrencyNameAr = x.CurrencyNameAr
                }).ToList();

                var AllFeaturetypes = await _unitOfWork.FeatureType.GetAllAsync();
                var feature = await _unitOfWork.Features.GetAllAsync();

                var allFeatures = new List<FeaturetypeAllDto>();

                foreach (var featureType in AllFeaturetypes)
                {
                    var features = feature.Where(x => x.FeaturetypeId == featureType.Id).ToList();
                    allFeatures.Add(new FeaturetypeAllDto
                    {
                        Id = featureType.Id,
                        TypeName = featureType.TypeName,
                        TypeNameAr = featureType.TypeNameAr,
                        Features = features.Select(x => new FeatureDto
                        {
                            Id = x.Id,
                            FeatureName = x.FeatureName,
                            FeatureNameAr = x.FeatureNameAr
                        }).ToList()
                    });
                }

                return Ok(new { carTypes = cartypeDto, brands = brandDto, models = modelDto, colors = colorDto, years = yearDto, gearbox = GearboxDto, trims = trimDto, specs = specsDto, locations= locationDto, currencies = currencyDto, features = allFeatures });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        // [RequestSizeLimit(bytes: 5_000_000)]
        [Route("Add")]
        public async Task<IActionResult> Add([FromForm] CarOfferRequestDto carOfferRequestDto)
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

                var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var Added = await _unitOfWork.CarOffer.AddAsync(new CarOffer
                {
                    Description = carOfferRequestDto.Description,//1
                    CarTypeId = carOfferRequestDto.CarTypeId,//2 
                    BrandModelId = carOfferRequestDto.BrandModelId,//3
                    ColorId = carOfferRequestDto.ColorId,//4

                    YearId = carOfferRequestDto.YearId,//5
                    GearboxId = carOfferRequestDto.GearboxId,//6 - op 
                    TrimId = carOfferRequestDto.TrimId,//7
                    Seats = carOfferRequestDto.Seats,//10
                    Kilometer = carOfferRequestDto.Kilometer,//11

                    SpecsId = carOfferRequestDto.SpecsId,//8
                    LocationId = carOfferRequestDto.LocationId,//9
                    CurrencyId = carOfferRequestDto.CurrencyId,//12
                    Price = carOfferRequestDto.Price,//12
                    YTLink = carOfferRequestDto.YTLink,//14
                    IsActive = carOfferRequestDto.IsActive,//13
                    UserId = UserId,
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


                
                for (int i = 0; i < carOfferRequestDto.ImageGallaries.Count; i++)
                {
                    //var pathResult = await CreateImgPath(file.FileName);
                    var file = carOfferRequestDto.ImageGallaries[i];
                    var pathResult = await _imageProcessor.SaveImageAsync(file);
                    if (pathResult is not null)
                    {
                        await _unitOfWork.ImageGallary.AddAsync(new ImageGallary
                        {
                            FilePath = pathResult,
                            CarOfferId = Added.Id,
                            OrderNo = i + 1
                        });
                    }
                }

                foreach (var feature in carOfferRequestDto.CarFeatures)
                {
                    await _unitOfWork.CarFeature.AddAsync(new CarFeatures
                    {
                        CarOfferId = Added.Id,
                        FeatureId = feature.FeatureId
                    });
                }

                await _unitOfWork.CompleteAsync();

                var result = await _unitOfWork.CarOffer.GetAsync(Added.Id);
                var offerResponse = new UpdateCarOfferResponse
                {
                    Id = result!.Id,
                    Description = result.Description,
                    BrandModel = new BrandModelDto
                    {
                        Id = result.BrandModel.Id,
                        ModelName = result.BrandModel.ModelName,
                        ModelNameAr = result.BrandModel.ModelNameAr,
                        Brand = new BrandDto
                        {
                            Id = result.BrandModel.Brand.Id,
                            Name = result.BrandModel.Brand.Name,
                            NameAr = result.BrandModel.Brand.NameAr,
                            LogoPath = string.IsNullOrEmpty(result.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{result.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                        }
                    },
                    Trim = new TrimDto
                    {
                        Id = result.Trim.Id,
                        TrimName = result.Trim.TrimName,
                        TrimNameAr = result.Trim.TrimNameAr
                    } ?? null,
                    Year = new YearDto
                    {
                        Id = result.Year.Id,
                        YearName = result.Year.YearName
                    },
                    Colors = new ColorsDto
                    {
                        Id = result.Colors.Id,
                        ColorName = result.Colors.ColorName,
                        ColorNameAr = result.Colors.ColorNameAr
                    },
                    Cartype = new CartypeDto
                    {
                        Id = result.Cartype.Id,
                        TypeName = result.Cartype.TypeName,
                        TypeNameAr = result.Cartype.TypeNameAr,
                        ImgPath = string.IsNullOrEmpty(result.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{result.Cartype.ImgPath.Replace("\\", "/")}",
                    },
                    Gearbox = new GearboxDto
                    {
                        Id = result.Gearbox.Id,
                        GearboxName = result.Gearbox.GearboxName,
                        GearboxNameAr = result.Gearbox.GearboxNameAr
                    } ?? null,
                    Specs = new SpecsDto
                    {
                        Id = result.Specs.Id,
                        SpecsName = result.Specs.SpecsName,
                        SpecsNameAr = result.Specs.SpecsNameAr
                    } ?? null,
                    Location = new LocationDto
                    {
                        Id = result.Location.Id,
                        LocationName = result.Location.LocationName,
                        LocationNameAr = result.Location.LocationNameAr
                    } ?? null,
                    Seats = result.Seats,
                    Kilometer = result.Kilometer ?? null,
                    Price = result.Price,
                    Currency = new CurrencyDto
                    {
                        Id = result.Currency.Id,
                        CurrencyName = result.Currency.CurrencyName,
                        CurrencyNameAr = result.Currency.CurrencyNameAr
                    },
                    IsActive = result.IsActive,
                    //ImageGallaries = result.ImageGallaries.Select(y => new ImageGallaryDto
                    //{
                    //    Id = y.Id,
                    //    FilePath = $"{_uriService.GetBaseRoot()}/{y.FilePath.Replace("\\", "/")}",
                    //    OrderNo = y.OrderNo,
                    //}).ToList(),
                    //FeaturesTypes = result.CarFeatures.Select(f => new FeaturetypeAllDto
                    //{
                    //    Id = f.Features.FeaturesType.Id,
                    //    TypeName = f.Features.FeaturesType.TypeName,
                    //    TypeNameAr = f.Features.FeaturesType.TypeNameAr,
                    //    Features = f.Features.FeaturesType.Features.Where(c => c.Id == f.FeatureId).Select(x => new FeatureDto
                    //    {
                    //        Id = x.Id,
                    //        FeatureName = x.FeatureName,
                    //        FeatureNameAr = x.FeatureNameAr,
                    //    }).ToList(),
                    //}).ToList(),
                    YTLink = result.YTLink ?? null,
                };

                return CreatedAtRoute("GetOffer", offerResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Error Processing Request"
                    }
                });
            }

        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {
            var IsDeleted = await _unitOfWork.CarOffer.DeleteAsync(bid);

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
        public async Task<IActionResult> Update([FromBody] UpdateCarOfferRequestDto updateCarOfferRequestDto)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var updated = await _unitOfWork.CarOffer.UpdateAsync(new CarOffer
            {
                Id = updateCarOfferRequestDto.Id,
                Description = updateCarOfferRequestDto.Description,
                BrandModelId = updateCarOfferRequestDto.BrandModelId,
                TrimId = updateCarOfferRequestDto.TrimId,
                YearId = updateCarOfferRequestDto.YearId,
                ColorId = updateCarOfferRequestDto.ColorId,
                CarTypeId = updateCarOfferRequestDto.CartTypeId,
                GearboxId = updateCarOfferRequestDto.GearboxId,
                SpecsId = updateCarOfferRequestDto.SpecsId,
                LocationId = updateCarOfferRequestDto.LocationId,
                Seats = updateCarOfferRequestDto.Seats,
                Kilometer = updateCarOfferRequestDto.Kilometer,
                Price = updateCarOfferRequestDto.Price,
                CurrencyId = updateCarOfferRequestDto.CurrencyId,
                IsActive = updateCarOfferRequestDto.IsActive,
                YTLink = updateCarOfferRequestDto.YTLink,
                UserId = UserId,
            }, updateCarOfferRequestDto.Id);


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

            var result = await _unitOfWork.CarOffer.GetAsync(updated.Id);
            var offerResponse = new UpdateCarOfferResponse
            {
                Id = result!.Id,
                Description = result.Description,
                BrandModel = new BrandModelDto
                {
                    Id = result.BrandModel.Id,
                    ModelName = result.BrandModel.ModelName,
                    ModelNameAr = result.BrandModel.ModelNameAr,
                    Brand = new BrandDto
                    {
                        Id = result.BrandModel.Brand.Id,
                        Name = result.BrandModel.Brand.Name,
                        NameAr = result.BrandModel.Brand.NameAr,
                        LogoPath = string.IsNullOrEmpty(result.BrandModel.Brand.LogoPath) ? null : $"{_uriService.GetBaseRoot()}/{result.BrandModel.Brand.LogoPath.Replace("\\", "/")}",
                    }
                },
                Trim = new TrimDto
                {
                    Id = result.Trim.Id,
                    TrimName = result.Trim.TrimName,
                    TrimNameAr = result.Trim.TrimNameAr
                } ?? null,
                Year = new YearDto
                {
                    Id = result.Year.Id,
                    YearName = result.Year.YearName
                },
                Colors = new ColorsDto
                {
                    Id = result.Colors.Id,
                    ColorName = result.Colors.ColorName,
                    ColorNameAr = result.Colors.ColorNameAr
                },
                Cartype = new CartypeDto
                {
                    Id = result.Cartype.Id,
                    TypeName = result.Cartype.TypeName,
                    TypeNameAr = result.Cartype.TypeNameAr,
                    ImgPath = string.IsNullOrEmpty(result.Cartype.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{result.Cartype.ImgPath.Replace("\\", "/")}",
                },
                Gearbox = new GearboxDto
                {
                    Id = result.Gearbox.Id,
                    GearboxName = result.Gearbox.GearboxName,
                    GearboxNameAr = result.Gearbox.GearboxNameAr
                } ?? null,
                Specs = new SpecsDto
                {
                    Id = result.Specs.Id,
                    SpecsName = result.Specs.SpecsName,
                    SpecsNameAr = result.Specs.SpecsNameAr
                } ?? null,
                Location = new LocationDto
                {
                    Id = result.Location.Id,
                    LocationName = result.Location.LocationName,
                    LocationNameAr = result.Location.LocationNameAr
                } ?? null,
                Seats = result.Seats,
                Kilometer = result.Kilometer ?? null,
                Price = result.Price,
                Currency = new CurrencyDto
                {
                    Id = result.Currency.Id,
                    CurrencyName = result.Currency.CurrencyName,
                    CurrencyNameAr = result.Currency.CurrencyNameAr
                },
                IsActive = result.IsActive,
                //ImageGallaries = result.ImageGallaries.Select(y => new ImageGallaryDto
                //{
                //    Id = y.Id,
                //    FilePath = $"{_uriService.GetBaseRoot()}/{y.FilePath.Replace("\\", "/")}",
                //    OrderNo = y.OrderNo,
                //}).ToList(),
                //FeaturesTypes = result.CarFeatures.Select(f => new FeaturetypeAllDto
                //{
                //    Id = f.Features.FeaturesType.Id,
                //    TypeName = f.Features.FeaturesType.TypeName,
                //    TypeNameAr = f.Features.FeaturesType.TypeNameAr,
                //    Features = f.Features.FeaturesType.Features.Where(c => c.Id == f.FeatureId).Select(x => new FeatureDto
                //    {
                //        Id = x.Id,
                //        FeatureName = x.FeatureName,
                //        FeatureNameAr = x.FeatureNameAr,
                //    }).ToList(),
                //}).ToList(),
                YTLink = result.YTLink ?? null,
            };

            return Ok(offerResponse);
        }

    }
}
