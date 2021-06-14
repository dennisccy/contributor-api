using contributor_api.Model;
using contributor_api.Service.Interface;
using Microsoft.Extensions.Logging;

namespace contributor_api.Service
{
    public class ValidationService : IValidation
    {
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(ILogger<ValidationService> logger)
        {
            _logger = logger;
        }

        public bool ValidateForex(Price price, out string error)
        {
            error = null;

            // e.g. bid >= ask
            if (price.Bid >= price.Ask)
            {
                error = "Bid is larger than or equal to Ask";
                return false;
            }

            // e.g. the spread is too large
            if (price.Bid / price.Ask < 0.9)
            {
                error = "The spread between bid and ask is too large";
                return false;
            }

            return true;
        }
    }
}
