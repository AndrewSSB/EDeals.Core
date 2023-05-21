namespace EDeals.Core.Domain.Common
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject first, ValueObject second)
        {
            if (first is null ^ second is null)
            {
                return false;
            }

            return ReferenceEquals(first, second) || first!.Equals(second);
        }

        protected static bool NotEqualOperator(ValueObject first, ValueObject second)
        {
            return !EqualOperator(first, second);
        }


    }
}
