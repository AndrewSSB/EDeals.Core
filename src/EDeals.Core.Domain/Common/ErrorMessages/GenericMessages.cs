namespace EDeals.Core.Domain.Common.ErrorMessages
{
    public static class GenericMessages
    {
        public const string GenericMessage = "Something went wrong, please try again";
        public const string UserDoesNotExists = "User does not exists";
        public const string InternalError = "Internal error";

        // Authentication
        public const string DigitCodeTimeout = "You need to wait 1 minute to get another digit code";
        public const string InvalidDigitCode = "Invalid digit code";
        public const string AlreadyConfirmed = "Phone number / email was already confirmed";
    }
}
