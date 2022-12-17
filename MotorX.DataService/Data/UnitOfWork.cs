using Microsoft.Extensions.Logging;
using MotorX.DataService.IConfiguration;
using MotorX.DataService.IRepository;
using MotorX.DataService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public IUserRepository User { get; private set; }
        public IRefreshTokensRepository RefreshTokens { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IBrandModelRepository BrandModel { get; private set; }
        public ICarOfferRepository CarOffer { get; private set; }
        public ICarTypeRepository CarType { get; private set; }
        public IColorsRepository Colors { get; private set; }
        public ICurrencyRepository Currency { get; private set; }
        public IFeaturesRepository Features { get; private set; }
        public IFeatureTypeRepository FeatureType { get; private set; }
        public IGearboxRepository Gearbox { get; private set; }
        public ILocationRepository Location { get; private set; }
        public ISpecsRepository Specs { get; private set; }
        public ISummeryMostViewedRepository summeryMostViewed { get; private set; }
        public ITrimRepository Trim { get; private set; }
        public IYearRepository Year { get; private set; }
        public IImageGallaryRepository ImageGallary { get; private set; }
        public ICarFeatureRepository CarFeature { get; private set; }
        public ICustomerInfoRepository CustomerInfo { get; private set; }
        public IFavoriteOfferRepository FavoriteOffer { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("db_logs");
            User = new UserRepository(context, _logger);
            RefreshTokens = new RefreshTokensRepository(context, _logger);
            Brand = new BrandRepository(context, _logger);
            BrandModel = new BrandModelRepository(context, _logger);
            CarOffer = new CarOfferRepository(context, _logger);
            CarType = new CarTypeRepository(context, _logger);
            Colors = new ColorsRepository(context, _logger);
            Currency = new CurrencyRepository(context, _logger);
            Features = new FeaturesRepository(context, _logger);
            FeatureType = new FeatureTypeRepository(context, _logger);
            Gearbox = new GearboxRepository(context, _logger);
            Location = new LocationRepository(context, _logger);
            Specs = new SpecsRepository(context, _logger);
            summeryMostViewed = new SummaryMostViewedRepository(context, _logger);
            Trim = new TrimRepository(context, _logger);
            Year = new YearRepository(context, _logger);
            ImageGallary = new ImageGallaryRepository(context, _logger);
            CarFeature = new CarFeaturesRepository(context, _logger);
            CustomerInfo = new CustomerInfoRepository(context, _logger);
            FavoriteOffer = new FavoriteOfferRepository(context, _logger);
            Notification = new NotificationRepository(context, _logger);
        }


        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
