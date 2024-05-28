using HotelBookingSystemAPI.Models;

namespace HotelBookingSystemAPI.Repository.Interfaces
{
    public interface IHotelWithAddressRepository : IRepository<int, Hotel>
    {
        public Task<IEnumerable<Hotel>> GetAllWithAddress();
        public Task<Hotel> GetByKeyWithAddress(int key);
    }
}
