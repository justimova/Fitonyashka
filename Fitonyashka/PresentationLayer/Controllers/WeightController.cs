using Fitonyashka.InfrastructureLayer.Interfaces;
using Fitonyashka.ViewModels.Weight;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fitonyashka.Models;
using Fitonyashka.PresentationLayer.ViewModels;
using Fitonyashka.PresentationLayer.ViewModels.Weight;
using Fitonyashka.BusinessLogicLayer.Services.Interfaces;

namespace Fitonyashka.PresentationLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        List<WeightModel> weightDtos = _weightService.GetAllByUserId(currentUserId.Value);
        var weightViewModels = weightDtos.Select(w => new WeightViewModel(w.Id, w.Date, w.Weight)).ToList();

        return Ok(weightViewModels);
    }

    [HttpGet("{id:int}")]
    public ActionResult<WeightInfoViewModel> Get(int id) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        WeightModel weightDto = _weightService.GetById(id);
        var weightInfo = new WeightInfoViewModel { Id = weightDto.Id, Date = weightDto.Date, Weight = weightDto.Weight };

        return Ok(weightInfo);
    }

    [HttpPost]
    public ActionResult<ResultViewModel> Create([FromBody] WeightCreateViewModel weightCreateViewModel) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        var weightDto = new WeightModel {
            Date = weightCreateViewModel.Date,
            Weight = weightCreateViewModel.Weight,
            UserId = currentUserId.Value,
        };
        ResultModel result = _weightService.Create(weightDto);
        var resultViewModel = new ResultViewModel {
            ErrorMessage = result.ErrorMessage,
            IsSuccess = result.IsSuccess,
        };
        if (!resultViewModel.IsSuccess) {
            return BadRequest(resultViewModel);
        }

        return Ok(resultViewModel);
    }

    [HttpPut]
    public ActionResult<ResultViewModel> Update([FromBody] WeightUpdateViewModel weightUpdateViewModel) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        var weightDto = new WeightModel {
            Date = weightUpdateViewModel.Date,
            Weight = weightUpdateViewModel.Weight,
            Id = weightUpdateViewModel.Id,
            UserId = currentUserId.Value,
        };
        ResultModel result = _weightService.Update(weightDto);
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
        ResultModel result = _weightService.Delete(id);
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
