
using System;
using System.Collections.Generic;

namespace FriendStorage.DataAccess
{
    public interface IDataService<Item> : IDisposable
    {
        Item GetItemById(int itemId);

        IEnumerable<Item> GetAllItems();

        void SaveItem(Item item);

        void DeleteItem(int itemId);
    }
}
