namespace EDeals.Core.Domain.Common.ErrorMessages
{
    public enum ErrorCodes
    {
        AccountCreation = 100,
        AssigningRole = 101,

        InvalidUsernameOrPassword = 200,
        InvalidDigitCode = 201,
        DigitCodeTimeout = 202,
        AlreadyConfirmed = 203,

        UserDoesNotExists = 300,

        InternalServer = 500
    }
}
