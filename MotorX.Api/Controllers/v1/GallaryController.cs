using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Services;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;
using System.Net.Http.Headers;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GallaryController : BaseController
    {
        private readonly IUriService _uriService;
        private readonly IImageProcessor _imageProcessor;
        public GallaryController(IUnitOfWork unitOfWork, IUriService uriService, IImageProcessor imageProcessor) : base(unitOfWork)
        {
            _uriService = uriService;
            _imageProcessor = imageProcessor;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCarOfferGallary", Name = "GetCarOfferGallary")]
        public async Task<IActionResult> GetCarOfferGallary([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.ImageGallary.GetAllGallaryByCarOfferAsync(bid);

            if (result is null) return NotFound("Item Not Found");


            return Ok(result.Select(x => new ImageGallaryDto
            {
                Id = x.Id,
                FilePath = $"{_uriService.GetBaseRoot()}/{x.FilePath.Replace("\\", "/")}",
                OrderNo = x.OrderNo
            }).ToList());
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromForm] ImageGallaryRequestDto imageGallaryDto)
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

            if (imageGallaryDto.FileName is null)
            {
                return BadRequest(new ErrorsDto
                {
                    Errors = new List<string>()
                    {
                        "Image Cannot by Empty"
                    }
                });
            }
            var dbPath = await _imageProcessor.SaveImageAsync(imageGallaryDto.FileName);
            if (dbPath is not null)
            {
                var Added = await _unitOfWork.ImageGallary.AddAsync(new ImageGallary
                {
                    CarOfferId = imageGallaryDto.CarOfferId,
                    OrderNo = await _unitOfWork.ImageGallary.CountAsync() + 1,
                    FilePath = dbPath
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

                var resultGallary = new ImageGallaryDto
                {
                    Id = Added.Id,
                    FilePath = $"{_uriService.GetBaseRoot()}/{Added.FilePath.Replace("\\", "/")}",
                    OrderNo = Added.OrderNo,
                };

                return CreatedAtRoute("GetCarOfferGallary", resultGallary);
            }

            return BadRequest(new ErrorsDto
            {
                Errors = new List<string>()
                {
                    "Image Cannot by Empty"
                }
            });

        }

        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {

            var checkIfResourseExist = await _unitOfWork.ImageGallary.GetAsync(bid);
            if (checkIfResourseExist?.FilePath is not null)
            {
                await _imageProcessor.RemoveImageAsync(checkIfResourseExist.FilePath);
            }
            var IsDeleted = await _unitOfWork.ImageGallary.DeleteAsync(bid);

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

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] List<ImageGallaryDto> images)
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

                for (int i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    await _unitOfWork.ImageGallary.UpdateOrderAsync(image.Id, i + 1);
                }
                await _unitOfWork.CompleteAsync();
                return Ok(images);
            }
            catch (Exception ex)
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
        

    }
}
