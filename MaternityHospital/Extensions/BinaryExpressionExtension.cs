using ModelLibrary.Enums;
using System.Linq.Expressions;

namespace MaternityHospital.Extensions
{
    public static class BinaryExpressionExtension
    {
        public static BinaryExpression? CreateExpression(this IEnumerable<string> values, Expression member)
        {
            BinaryExpression? filter = null;
            foreach (var item in values)
            {
                if (!string.IsNullOrEmpty(item) && Enum.TryParse<CompareEnum>(item.Take(2).ToArray(), out var resultCompareEnum))
                {
                    if (DateTimeOffset.TryParse(item.Skip(2).ToArray(), out var forDate))
                    {
                        if (resultCompareEnum == CompareEnum.ap)
                        {
                            forDate = forDate.AddDays(-1);
                        }
                        ConstantExpression constantExpression = default!;
                        ConstantExpression? constantExpressionMax = null;
                        Expression field = member;
                        var timeOfDay = forDate.TimeOfDay;
                        if (timeOfDay == TimeSpan.Zero)
                        {
                            constantExpression = Expression.Constant(DateTimeToDateOnly(forDate));
                            field = Expression.Call(typeof(BinaryExpressionExtension), nameof(DateTimeToDateOnly), null, member);
                            if (resultCompareEnum == CompareEnum.ap)
                            {
                                constantExpressionMax = Expression.Constant(DateTimeToDateOnly(forDate.AddDays(2)));
                            }
                        }
                        else
                        {
                            constantExpression = Expression.Constant(DateTimeToDateShort(forDate));
                            field = Expression.Call(typeof(BinaryExpressionExtension), nameof(DateTimeToDateShort), null, member);
                            if (resultCompareEnum == CompareEnum.ap)
                            {
                                constantExpressionMax = Expression.Constant(DateTimeToDateShort(forDate.AddDays(2)));
                            }
                        }

                        BinaryExpression? expression = null;
                        switch (resultCompareEnum)
                        {
                            case CompareEnum.eq:
                                {
                                    expression = Expression.Equal(field, constantExpression);
                                }
                                break;
                            case CompareEnum.ne:
                                {
                                    expression = Expression.NotEqual(field, constantExpression);
                                }
                                break;
                            case CompareEnum.sa:
                            case CompareEnum.gt:
                                {
                                    expression = Expression.GreaterThan(field, constantExpression);
                                }
                                break;
                            case CompareEnum.eb:
                            case CompareEnum.lt:
                                {
                                    expression = Expression.LessThan(field, constantExpression);
                                }
                                break;
                            case CompareEnum.ge:
                                {
                                    expression = Expression.GreaterThanOrEqual(field, constantExpression);
                                }
                                break;
                            case CompareEnum.le:
                                {
                                    expression = Expression.LessThanOrEqual(field, constantExpression);
                                }
                                break;
                            case CompareEnum.ap:
                                {
                                    filter = filter.AddBinaryExpression(Expression.GreaterThanOrEqual(field, constantExpression));
                                    if (constantExpressionMax != null)
                                    {
                                        expression = Expression.LessThanOrEqual(field, constantExpressionMax);
                                    }
                                }
                                break;
                        }
                        filter = filter.AddBinaryExpression(expression);

                    }
                }
            }
            return filter;
        }


        public static DateOnly DateTimeToDateOnly(DateTimeOffset date)
        {
            var result = DateOnly.FromDateTime(date.DateTime);
            return result;
        }

        public static DateTime DateTimeToDateShort(DateTimeOffset date)
        {
            var result = DateTime.Parse(date.DateTime.ToString("g"));
            return result;
        }


        public static BinaryExpression? AddBinaryExpression(this BinaryExpression? filter, BinaryExpression? newExpression)
        {
            if (newExpression != null)
                filter = (filter != null) ? Expression.And(filter, newExpression) : newExpression;
            return filter;
        }

    }
}
