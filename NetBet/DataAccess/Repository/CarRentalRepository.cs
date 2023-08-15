using NetBet.DataAccess.Models;

namespace NetBet.DataAccess.Repository
{
    public interface ICarRentalRepository : IRepositoryBase<RentDetails>
    {
    }

    public class CarRentalRepository : RepositoryBase<RentDetails>, ICarRentalRepository
    {
        private readonly ILogger<CarRentalRepository> _logger;

        public CarRentalRepository(ILogger<CarRentalRepository> logger)
        {
            _logger = logger;
        }

    }
}
