using System;
using System.Threading.Tasks;
using contributor_api.Model;
using contributor_api.Model.Enum;
using contributor_api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace contributor_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContributorController : ControllerBase
    {
        private readonly ILogger<ContributorController> _logger;
        private readonly IDataStore _dataStore;
        private readonly IAuthorization _authorization;
        private readonly IValidation _validation;

        public ContributorController(IDataStore dataStore, IAuthorization authorization, IValidation valiation, ILogger<ContributorController> logger)
        {
            _logger = logger;
            _dataStore = dataStore;
            _authorization = authorization;
            _validation = valiation;
        }

        [HttpPost("Authorize")]
        public ActionResult<string> Authorize(string userName)
        {
            var token = _authorization.Authorize(userName);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("No such user");
            }

            return token;
        }

        // Improvement: bulk contribute
        [HttpPost("ContributePrice")]
        public async Task<ActionResult> ContributePrice(string baseCurrency, string quoteCurrency, double bid, double ask, string token)
        {
            try
            {
                if (!_authorization.IsValidToken(token))
                {
                    return BadRequest("Invalid authorization token. Please authorize user.");
                }

                string userName = _authorization.GetUserName(token);
                var userPermissions = _authorization.GetUserPermissions(userName);
                if (userPermissions == null || !userPermissions.Contains(UserPermissionEnum.ContributePrice))
                {
                    return BadRequest("No ContirbutePrice permission");
                }

                var price = new Price(baseCurrency, quoteCurrency, bid, ask, userName);
                if (!_validation.ValidateForex(price, out string error))
                {
                    return BadRequest(error);
                }

                await _dataStore.AddPriceAsync(price);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // Doubt: get price in contributor?
        [HttpPost("GetLatestPrice")]
        public async Task<ActionResult<Price>> GetLatestPrice(string baseCurrency, string quoteCurrency, string token)
        {
            try
            {
                if (!_authorization.IsValidToken(token))
                {
                    return BadRequest("Invalid authorization token. Please authorize user.");
                }

                string userName = _authorization.GetUserName(token);
                var userPermissions = _authorization.GetUserPermissions(userName);
                if (userPermissions == null || !userPermissions.Contains(UserPermissionEnum.GetPrice))
                {
                    return BadRequest("No GetPrice permission");
                }

                var currencyPair = new CurrencyPair(baseCurrency, quoteCurrency);

                return await _dataStore.GetLatestPriceAsync(currencyPair);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}