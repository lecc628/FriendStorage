
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendStorage.DataAccess
{
    public interface IDataService<Item> : IDisposable
    {
        Task<Item> GetItemByIdAsync(int itemId);

        Task SaveItemAsync(Item item);

        Task DeleteItemAsync(int itemId);

        Task<IEnumerable<Item>> GetAllItemsAsync();
    }
}
