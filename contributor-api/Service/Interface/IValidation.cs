using contributor_api.Model;

namespace contributor_api.Service.Interface
{
    public interface IValidation
    {
        /// <summary>
        /// Validate forex contribution
        /// </summary>
        /// <param name="Forex price info"></param>
        /// <param name="Validation error when return false"></param>
        /// <returns>Validation successful or not</returns>
        bool ValidateForex(Price price, out string error);
    }
}
