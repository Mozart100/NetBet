using System.Collections.Concurrent;
using System.Reflection;

namespace NetBet.Services
{
    public class DateRange : IComparable<DateRange>
    {
        public DateOnly Start { get; }
        public DateOnly End { get; }

        public DateRange(DateOnly start, DateOnly end)
        {
            Start = start;
            End = end;
        }

        public bool IsOverlap(DateRange range)
        {
            return Start < range.End && End > range.Start;
        }

        public bool IsOverlap(DateOnly date)
        {
            return Start < date && End > date;
        }

        public int CompareTo(DateRange other)
        {
            // Compare based on the Start property
            return Start.CompareTo(other.Start);
        }
    }

    public interface IRentalDateSlotService
    {
        void Add(int carId, DateOnly from, DateOnly to);
        Task<bool> IsAvailable(int carId, DateOnly from, DateOnly to);
    }

    public class RentalDateSlotService : IRentalDateSlotService
    {
        private readonly ILogger<RentalDateSlotService> _logger;

        public ConcurrentDictionary<int, SortedSet<DateRange>> _rentalDateSlots;

        public RentalDateSlotService(ILogger<RentalDateSlotService> logger)
        {
            _rentalDateSlots = new ConcurrentDictionary<int, SortedSet<DateRange>>();
        }

        public void Add(int carId, DateOnly from, DateOnly to)
        {
            var dateRange = new DateRange(from, to);

            _rentalDateSlots.AddOrUpdate(carId, x =>
            {
                var list = new SortedSet<DateRange>();
                list.Add(dateRange);
                return list;
            },
            (x, existingRanges) =>
            {
                existingRanges.Add(dateRange);
                return existingRanges;
            });
        }

        public async Task<bool> IsAvailable(int carId, DateOnly from, DateOnly to)
        {
            if (_rentalDateSlots.TryGetValue(carId, out var dateRanges))
            {
                var searchRange = new DateRange(from, to);

                foreach (var existingRange in dateRanges)
                {
                    if (searchRange.IsOverlap(existingRange))
                    {
                        // There is an overlap, meaning the requested dates are not available.
                        return false;
                    }
                }

                // No overlap found, meaning the requested dates are available.
            }

            return true;
        }
    }



}
