using MotorX.DataService.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IRefreshTokensRepository RefreshTokens { get; }
        IBrandRepository Brand { get; }
        IBrandModelRepository BrandModel { get; }
        ICarOfferRepository CarOffer { get; }
        ICarTypeRepository CarType { get; }
        IColorsRepository Colors { get; }
        ICurrencyRepository Currency { get; }
        IFeaturesRepository Features { get; }
        IFeatureTypeRepository FeatureType { get; }
        IGearboxRepository Gearbox { get; }
        ILocationRepository Location { get; }
        ISpecsRepository Specs { get; }
        ISummeryMostViewedRepository summeryMostViewed { get; }
        ITrimRepository Trim { get; }
        IYearRepository Year { get; }
        IImageGallaryRepository ImageGallary { get; }
        ICarFeatureRepository CarFeature { get; }
        ICustomerInfoRepository CustomerInfo { get; }
        IFavoriteOfferRepository FavoriteOffer { get; }
        Task CompleteAsync();
    }
}
