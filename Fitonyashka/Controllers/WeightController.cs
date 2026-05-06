using Fitonyashka.InfrastructureLayer.Interfaces;
using Fitonyashka.ViewModels;
using Fitonyashka.ViewModels.Weight;
using Fitonyashka.DTOs;
using Fitonyashka.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fitonyashka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightController : ControllerBase
    {
        private readonly IWeightService _weightService;
        private readonly ICurrentUserContext _currentUserContext;

        public WeightController(IWeightService weightService, ICurrentUserContext currentUserContext) {
            _weightService = weightService;
            _currentUserContext = currentUserContext;
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<WeightViewModel>> Get() {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            List<WeightDto> weightDtos = _weightService.GetAllByUserId(currentUserId.Value);
            var weightViewModels = weightDtos.Select(w => new WeightViewModel(w.Id, w.Date, w.Weight)).ToList();
            return Ok(weightViewModels);
        }

        [HttpGet("{id:int}")]
        public ActionResult<WeightInfoViewModel> Get(int id) {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            WeightDto weightDto = _weightService.GetById(id);
            var weightInfo = new WeightInfoViewModel { Id = weightDto.Id, Date = weightDto.Date, Weight = weightDto.Weight };
            return Ok(weightInfo);
        }

        [HttpPost]
        public ActionResult<ResultViewModel> Create([FromBody] WeightCreateViewModel weightCreateViewModel) {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            var weightDto = new WeightDto {
                Date = weightCreateViewModel.Date,
                Weight = weightCreateViewModel.Weight,
                UserId = currentUserId.Value,
            };
            ResultDto result = _weightService.Create(weightDto);
            var resultviewModel = new ResultViewModel {
                ErrorMessage = result.ErrorMessage,
                IsSuccess = result.IsSuccess,
            };
            if (!resultviewModel.IsSuccess) {
                return BadRequest(resultviewModel);
            }
            return Ok(resultviewModel);
        }

        [HttpPut]
        public ActionResult<ResultViewModel> Update([FromBody] WeightUpdateViewModel weightUpdateViewModel) {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            var weightDto = new WeightDto {
                Date = weightUpdateViewModel.Date,
                Weight = weightUpdateViewModel.Weight,
                Id = weightUpdateViewModel.Id,
                UserId = currentUserId.Value,
            };
            ResultDto result = _weightService.Update(weightDto);
            var resultviewModel = new ResultViewModel {
                ErrorMessage = result.ErrorMessage,
                IsSuccess = result.IsSuccess,
            };
            if (!resultviewModel.IsSuccess) {
                return BadRequest(resultviewModel);
            }
            return Ok(resultviewModel);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ResultViewModel> Delete(int id) {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            ResultDto result = _weightService.Delete(id);
            var resultviewModel = new ResultViewModel {
                ErrorMessage = result.ErrorMessage,
                IsSuccess = result.IsSuccess,
            };
            if (!resultviewModel.IsSuccess) {
                return BadRequest(resultviewModel);
            }
            return Ok(resultviewModel);
        }
    }

}
