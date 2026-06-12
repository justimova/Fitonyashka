using Fitonyashka.BusinessLogicLayer.Services.Interfaces;
using Fitonyashka.InfrastructureLayer.Interfaces;
using Fitonyashka.Models;
using Fitonyashka.PresentationLayer.ViewModels;
using Fitonyashka.PresentationLayer.ViewModels.Goal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitonyashka.PresentationLayer.Controllers;

[Route("api/[controller]")]
[Authorize]
public class GoalController : Controller
{
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IGoalService _goalService;
    private readonly IWeightService _weightService;

    public GoalController(ICurrentUserContext currentUserContext, IGoalService goalService, IWeightService weightService) {
        _currentUserContext = currentUserContext;
        _goalService = goalService;
        _weightService = weightService;
    }

    [HttpGet]
    [Route("currentGoal")]
    public ActionResult<GoalInfoViewModel> GetCurrentGoal() {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        GoalModel goalDto = _goalService.GetActiveGoalByUserId(currentUserId.Value);
        if (goalDto == null) {
            return Ok();
        }
        var goalInfo = new GoalInfoViewModel {
            Id = goalDto.Id,
            StartDate = goalDto.StartDate,
            InitialWeight = goalDto.InitialWeight,
            TargetWeight = goalDto.TargetWeight,
        };

        return Ok(goalInfo);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<GoalInfoViewModel> GetGoal(int id) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        GoalModel goalDto = _goalService.GetGoalById(id);
        if (goalDto == null) {
            return Ok();
        }
        var goalInfo = new GoalInfoViewModel {
            Id = goalDto.Id,
            StartDate = goalDto.StartDate,
            InitialWeight = goalDto.InitialWeight,
            TargetWeight = goalDto.TargetWeight,
        };

        return Ok(goalInfo);
    }

    [HttpPost]
    public ActionResult<ResultViewModel> Create([FromBody] GoalCreateViewModel goalCreateViewModel) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        var goalDto = new GoalModel {
            StartDate = DateTime.UtcNow,
            InitialWeight = goalCreateViewModel.InitialWeight,
            TargetWeight = goalCreateViewModel.TargetWeight,
            UserId = currentUserId.Value,
        };
        ResultModel result = _goalService.Create(goalDto);
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
    public ActionResult<ResultViewModel> Update([FromBody] GoalUpdateViewModel goalUpdateViewModel) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        var goalDto = new GoalModel {
            Id = goalUpdateViewModel.Id,
            InitialWeight = goalUpdateViewModel.InitialWeight,
            TargetWeight = goalUpdateViewModel.TargetWeight,
            UserId = currentUserId.Value,
        };
        ResultModel result = _goalService.Update(goalDto);
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
    [Route("completeIfNeeded")]
    public ActionResult<bool> CompleteActiveGoalIfNeeded() {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        var currentWeightModel = _weightService.GetLastByUserId(currentUserId.Value);
        var isCompleted = _goalService.CompleteIfNeeded(currentUserId.Value, currentWeightModel.Weight);

        return Ok(isCompleted);
    }

    [HttpDelete("{id}")]
    public ActionResult<ResultViewModel> Delete(int id) {
        int? currentUserId = _currentUserContext.GetCurrentUserId();
        if (currentUserId == null) {
            return Unauthorized();
        }
        ResultModel result = _goalService.Delete(id);
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
