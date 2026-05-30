using Fitonyashka.DTOs;
using Fitonyashka.InfrastructureLayer.Interfaces;
using Fitonyashka.Services.Interfaces;
using Fitonyashka.ViewModels;
using Fitonyashka.ViewModels.Goal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitonyashka.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GoalController : Controller
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IGoalService _goalService;

        public GoalController(ICurrentUserContext currentUserContext, IGoalService goalService) {
            _currentUserContext = currentUserContext;
            _goalService = goalService;
        }

        [HttpGet]
        [Route("currentGoal")]
        public ActionResult<GoalInfoViewModel> GetCurrentGoal() {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            GoalDto goalDto = _goalService.GetActiveGoalByUserId(currentUserId.Value);
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
        public IActionResult GetGoal(int id) {
            return Ok();
        }

        [HttpPost]
        public ActionResult<ResultViewModel> Create([FromBody] GoalCreateViewModel goalCreateViewModel) {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            var goalDto = new GoalDto {
                StartDate = DateTime.UtcNow,
                InitialWeight = goalCreateViewModel.InitialWeight,
                TargetWeight = goalCreateViewModel.TargetWeight,
                UserId = currentUserId.Value,
            };
            ResultDto result = _goalService.Create(goalDto);
            var resultViewModel = new ResultViewModel {
                ErrorMessage = result.ErrorMessage,
                IsSuccess = result.IsSuccess,
            };
            if (!resultViewModel.IsSuccess) {
                return BadRequest(resultViewModel);
            }

            return Ok(resultViewModel);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public ActionResult<ResultViewModel> Delete(int id) {
            int? currentUserId = _currentUserContext.GetCurrentUserId();
            if (currentUserId == null) {
                return Unauthorized();
            }
            ResultDto result = _goalService.Delete(id);
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

