using AutoMapper;
using GenericRepository.Repositories;
using GenericRepository.Services;
using OLAP.API.Models.Entity.Data;
using OLAP.API.Models.Request.Data;

namespace OLAP.API.Managers
{
    public class DataManager : IDataManager
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DataEModel, Guid> _dataRepo;
        private readonly IGenericRepository<CountryEModel, Guid> _dataCountryRepo;
        private readonly IGenericRepository<IndicatorEModel, Guid> _dataIndicatorRepo;

        public DataManager(IUnitOfWork unitOfWork,
                           IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dataRepo = unitOfWork.Repository<DataEModel, Guid>();
            _dataCountryRepo = unitOfWork.Repository<CountryEModel, Guid>();
            _dataIndicatorRepo = unitOfWork.Repository<IndicatorEModel, Guid>();
        }

        public async Task<bool> UploadData(List<DataRequestModel> data, CancellationToken cancellationToken = default)
        {
            var exCounytries = await _dataCountryRepo.GetAllAsync();

            var countries = data.DistinctBy(x => x.CountryCode)
                                .Where(x => !exCounytries.Any(a => a.CountryCode.Equals(x.CountryCode)))
                                .Select(x => new CountryEModel()
                                {
                                    Id = Guid.NewGuid(),
                                    CountryName = x.CountryName,
                                    CountryCode = x.CountryCode
                                })
                                .ToList();

            countries.AddRange(exCounytries);

            var exIndicators = await _dataIndicatorRepo.GetAllAsync();

            var indicator = data.DistinctBy(x => x.IndicatorName)
                                .Where(x => !exIndicators.Any(a => a.IndicatorCode.Equals(x.IndicatorCode)))
                                .Select(x => new IndicatorEModel
                                {
                                    Id = Guid.NewGuid(),
                                    IndicatorCode = x.IndicatorCode,
                                    IndicatorName = x.IndicatorName
                                })
                                .ToList();
            indicator.AddRange(exIndicators);

            var items = data.Select(x => new DataEModel
            {
                Id = Guid.NewGuid(),
                Date = x.Date,
                Value = x.Value,
                Frequency = x.Frequency,
                Country = countries.First(c => c.CountryCode == x.CountryCode),
                Indicator = indicator.First(i => i.IndicatorCode == x.IndicatorCode),
            })
            .ToList();

            await _dataRepo.AddRangeAsync(items, cancellationToken);
            return true;
        }
    }
}
