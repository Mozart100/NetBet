using NetBet.DataAccess.Models;
using NetBet.Infrastracture;

namespace NetBet.DataAccess.Repository
{
    public interface ICarRepository : IRepositoryBase<Car>
    {

        Task<bool> DecommissionCarAsync(Predicate<Car> selector);
        Task<IEnumerable<Car>> GetAllActiveCarsAsync();
    }
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        private readonly ILogger<CarRepository> _logger;

        public CarRepository(ILogger<CarRepository> logger)
        {
            _logger = logger;
        }

        public async Task<bool> DecommissionCarAsync(Predicate<Car> selector)
        {
            var cars = Models.Where(x => selector(x)).ToArray();

            if (!cars.SafeAny())
            {
                return false;
            }

            foreach (var car in cars)
            {
                car.IsActive = false;
            }

            return true;
        }

        public async Task<IEnumerable<Car>> GetAllActiveCarsAsync()
        {
            var cars = Models.Where(x => x.IsActive).ToArray();
            return cars;
        }
    }


}
