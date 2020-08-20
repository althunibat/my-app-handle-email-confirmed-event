using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Godwit.Common.Data.Core;
using Godwit.Common.Data.Core.Repository;
using Godwit.Common.Data.Model;
using Godwit.HandleEmailConfirmedEvent.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace Godwit.HandleEmailConfirmedEvent.Controllers {
    [ApiController]
    [Route("")]
    public class HasuraController : ControllerBase {
        private readonly ILogger _logger;
        private readonly IRepository<Notification, long> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<HasuraEvent> _validator;

        public HasuraController(IValidator<HasuraEvent> validator,
            ILogger<HasuraController> logger, IUnitOfWork unitOfWork,
            IRepository<Notification, long> repository) {
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HasuraEvent model) {
            var validation = _validator.Validate(model);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage)
                );

            try {
                var user = model.Event.Data.NewValue;
                var message =
                    $"Thank you {user.FirstName} {user.LastName} for completing your registration with Keto App";

                var notification = new Notification {
                    CreatedOn = Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Message = message,
                    UserId = user.Id
                };

                await _repository.Add(notification);
                var result = await _unitOfWork.SaveAsync().ConfigureAwait(false);
                if (!result.Succeeded)
                    return Problem(result.Errors.First().ErrorMessages.First().Value);
                return Ok();
            }
            catch (Exception e) {
                _logger.LogError(new EventId(1001, "Exception"), e, "Unable to Save Data!");
                return Problem("Unable to Send Email!, An Exception Occur!");
            }
        }
    }
}