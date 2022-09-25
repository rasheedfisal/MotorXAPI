using MotorX.Common;
using MotorX.DataService.Entities;
using MotorX.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace MotorX.Api.DTOs.Requests
{
    public class CustomerFilterRequestDto
    {
        [SearchCriteria]
        public Guid? ModelId { get; set; }
        [SearchCriteria]
        public Guid? BrandId { get; set; }
        [SearchCriteria]
        public DateTime? StartDate { get; set; }
        [SearchCriteria]
        public DateTime? EndDate { get; set; }
        public string SearchOperator { get; set; } = "ALL";


        public Expression<Func<SummaryMostViewed, bool>>? ToExpression()
        {
            Expression<Func<SummaryMostViewed, bool>>? result = null;


            if (ModelId.HasValue)
            {
                Expression<Func<SummaryMostViewed, bool>> expr = model => model.CarOffer.BrandModel.Id == ModelId;
                result = AppendExpression(result, expr);
            }

            if (BrandId.HasValue)
            {
                Expression<Func<SummaryMostViewed, bool>> expr = model => model.CarOffer.BrandModel.Brand.Id == BrandId;
                result = AppendExpression(result, expr);
            }

            if (StartDate.HasValue)
            {
                Expression<Func<SummaryMostViewed, bool>> expr = model => model.AddedDate >= StartDate;
                result = AppendExpression(result, expr);
            }

            if (EndDate.HasValue)
            {
                Expression<Func<SummaryMostViewed, bool>> expr = model => model.AddedDate <= EndDate;
                result = AppendExpression(result, expr);
            }

            return result;
        }

        /// <summary>
        /// Returns true, if this view model has criteria to search against
        /// </summary>        
        public bool HasCriteria()
        {
            //get the properites of this object
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var searchProperties = properties.Where(p => p.CustomAttributes.Select(a => a.AttributeType).Contains(typeof(SearchCriteriaAttribute)));

            return searchProperties.Where(sp => sp.GetValue(this).ToStringInstance().HasValue()).Any();
        }

        private Expression<Func<SummaryMostViewed, bool>> AppendExpression(Expression<Func<SummaryMostViewed, bool>>? left, Expression<Func<SummaryMostViewed, bool>> right)
        {
            Expression<Func<SummaryMostViewed, bool>> result;

            switch (SearchOperator)
            {
                case "ANY":

                    //the initial case starts off with a left epxression as null. If that's the case,
                    //then give the short-circuit operator something to trigger on for the right expression
                    if (left == null)
                    { left = model => false; }

                    result = ExpressionExtension<SummaryMostViewed>.OrElse(left, right);
                    break;
                case "ALL":

                    if (left == null)
                    { left = model => true; }

                    result = ExpressionExtension<SummaryMostViewed>.AndAlso(left, right);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return result;

        }


    }
}
