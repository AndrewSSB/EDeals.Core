using Microsoft.AspNetCore.Identity;

namespace EDeals.Core.Infrastructure.Identity.Extensions
{
    public static class IdentityExtensions
    {
        // TODO: Create a generic response class and map the success / failure of the identity result to that generic class
        public static string ToApplicationResult(this IdentityResult result) =>
            result.Succeeded ? "Success" : result.Errors.Select(e => e.Description).FirstOrDefault()!;

        //public static string ToApplicationResults(List<IdentityResult> results) =>
        //    results.ForEach(res => );
    }
}
