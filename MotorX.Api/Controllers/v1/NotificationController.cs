using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorX.Api.DTOs.Requests;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Services;
using MotorX.DataService.Entities;
using MotorX.DataService.IConfiguration;

namespace MotorX.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationController : BaseController
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IUriService _uriService;
        public NotificationController(IUnitOfWork unitOfWork, IImageProcessor imageProcessor, IUriService uriService) : base(unitOfWork)
        {
            _imageProcessor = imageProcessor;
            _uriService = uriService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.Notification.GetAllAsync();
            var notifyDto = result.Select(x => new Notification
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImgPath = x.ImgPath//string.IsNullOrEmpty(x.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{x.ImgPath.Replace("\\", "/")}",
            }).ToList();

            return Ok(notifyDto);
        }

        //[HttpPost, DisableRequestSizeLimit]
        [HttpPost]
        //[RequestSizeLimit(bytes: 5_000_000)]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] NotificationRequestDto notification)
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


            var Added = await _unitOfWork.Notification.AddAsync(new Notification
            {
                Title = notification.Title,
                Description = notification.Description,
                ImgPath = notification.ImgPath//await _imageProcessor.SaveImageAsync(notification.ImgPath)//CreateImgPath(brandDto.Logo)
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


            var NotifyResponse = new NotificationResponseDto
            {
                Id = Added.Id,
                Title = Added.Title,
                Description = Added.Description,
                ImgPath = Added.ImgPath//string.IsNullOrEmpty(Added.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{Added.ImgPath.Replace("\\", "/")}",
            };

            return CreatedAtRoute("Get", NotifyResponse);
        }

        [HttpGet]
        [Route("Get", Name = "Get")]
        public async Task<IActionResult> Get([FromQuery] Guid bid)
        {
            var result = await _unitOfWork.Notification.GetAsync(bid);

            if (result is null) return NotFound("Item Not Found");


            return Ok(new NotificationResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                ImgPath = result.ImgPath//string.IsNullOrEmpty(result.ImgPath) ? null : $"{_uriService.GetBaseRoot()}/{result.ImgPath.Replace("\\", "/")}",
            });
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> Remove([FromQuery] Guid bid)
        {

            //var checkIfResourseExist = await _unitOfWork.Notification.GetAsync(bid);
            //if (checkIfResourseExist?.ImgPath is not null)
            //{
            //    await _imageProcessor.RemoveImageAsync(checkIfResourseExist.ImgPath);
            //}
            var IsDeleted = await _unitOfWork.Notification.DeleteAsync(bid);

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
    }
}
