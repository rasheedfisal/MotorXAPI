using MotorX.Common;
using MotorX.DataService.Entities;
using MotorX.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace MotorX.Api.DTOs.Requests
{
    public class CarOfferFilterRequestDto
    {
        //[SearchCriteria]
        public string? search { get; set; }
        public string? ClientId { get; set; }
        //[SearchCriteria]
        //public string? BrandName { get; set; }
        //[SearchCriteria]
        //public string? CarTypeName { get; set; }
        //public string? SearchOperator { get; set; } = "ALL";


        public Expression<Func<CarOffer, bool>>? ToExpression()
        {
            Expression<Func<CarOffer, bool>>? result = null;


            if (search.HasValue())
            {
                Expression<Func<CarOffer, bool>> expr = model => (model.BrandModel.ModelName.Like(search) || model.BrandModel.Brand.Name.Like(search) || model.Cartype.TypeName.Like(search));
                result = AppendExpression(result, expr);
            }

            //if (BrandName.HasValue())
            //{
            //    Expression<Func<CarOffer, bool>> expr = model => model.BrandModel.Brand.Name.Like(BrandName);
            //    result = AppendExpression(result, expr);
            //}

            //if (CarTypeName.HasValue())
            //{
            //    Expression<Func<CarOffer, bool>> expr = model => model.Cartype.TypeName.Like(CarTypeName);
            //    result = AppendExpression(result, expr);
            //}

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

        private Expression<Func<CarOffer, bool>> AppendExpression(Expression<Func<CarOffer, bool>>? left, Expression<Func<CarOffer, bool>> right)
        {
            Expression<Func<CarOffer, bool>> result;

            //switch (SearchOperator)
            //{
            //    case "ANY":

            //        //the initial case starts off with a left epxression as null. If that's the case,
            //        //then give the short-circuit operator something to trigger on for the right expression
            //        if (left == null)
            //        { left = model => false; }

            //        result = ExpressionExtension<CarOffer>.OrElse(left, right);
            //        break;
            //    case "ALL":

            //        if (left == null)
            //        { left = model => true; }

            //        result = ExpressionExtension<CarOffer>.AndAlso(left, right);
            //        break;
            //    default:
            //        throw new InvalidOperationException();
            //}
            if (left == null)
            { left = model => true; }

            return ExpressionExtension<CarOffer>.AndAlso(left, right);
        }
    }
}
