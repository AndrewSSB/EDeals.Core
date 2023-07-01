namespace EDeals.Core.Domain.Common.ErrorMessages
{
    public static class GenericMessages
    {
        public const string GenericMessage = "Ceva nu a mers, te rog incearca inca odata";
        public const string UserDoesNotExists = "Utilizatorul nu exista";
        public const string InternalError = "Internal error";

        // Authentication
        public const string DigitCodeTimeout = "Trebuie sa astepti un minut";
        public const string InvalidDigitCode = "Codul introdus este invalid";
        public const string AlreadyConfirmed = "Numarul de telefon / emailul au fost deja confirmate";
    }
}
