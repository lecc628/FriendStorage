
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public class FileDataService : IDataService<Friend>
    {
        private const string StorageFile = "Friends.json";

        private static readonly List<Friend> FriendListByDefault = new()
        {
            new Friend
            {
                Id = 1,
                FirstName = "Thomas",
                LastName = "Huber",
                Birthday = new DateTime(1980, 10, 28),
                IsDeveloper = true
            },
            new Friend
            {
                Id = 2,
                FirstName = "Julia",
                LastName = "Huber",
                Birthday = new DateTime(1982, 10, 10)
            },
            new Friend
            {
                Id = 3,
                FirstName = "Anna",
                LastName = "Huber",
                Birthday = new DateTime(2011, 05, 13)
            },
            new Friend
            {
                Id = 4,
                FirstName = "Sara",
                LastName = "Huber",
                Birthday = new DateTime(2013, 02, 25)
            },
            new Friend
            {
                Id = 5,
                FirstName = "Andreas",
                LastName = "Böhler",
                Birthday = new DateTime(1981, 01, 10),
                IsDeveloper = true
            },
            new Friend
            {
                Id = 6,
                FirstName = "Urs",
                LastName = "Meier",
                Birthday = new DateTime(1970, 03, 5),
                IsDeveloper = true
            },
            new Friend
            {
                Id = 7,
                FirstName = "Chrissi",
                LastName = "Heuberger",
                Birthday = new DateTime(1987, 07, 16)
            },
            new Friend
            {
                Id = 8,
                FirstName = "Erkan",
                LastName = "Egin",
                Birthday = new DateTime(1983, 05, 23)
            }
        };

        public Friend GetItemById(int friendId) => GetAllItemsOrDefault().Single(friend => friend.Id == friendId);

        public IEnumerable<Friend> GetAllItems() => GetAllItemsOrDefault();

        public void SaveItem(Friend friend)
        {
            if (friend.Id <= 0)
            {
                InsertItem(friend);
            }
            else
            {
                UpdateItem(friend);
            }
        }

        public void DeleteItem(int friendId)
        {
            var friendList = GetAllItemsOrDefault();
            var friendToDelete = friendList.Single(friend => friend.Id == friendId);

            friendList.Remove(friendToDelete);
            WriteToFileAsync(friendList).Wait();
        }

        public void Dispose()
        {
            // Usually Service-Proxies are disposable. This method is added as demo-purpose
            // to show how to use an IDisposable in the client with a Func<T>. => Look for example at the FriendDataProvider-class.
        }

#pragma warning disable CA1822 // Mark members as static
        private async Task<List<Friend>?> ReadFromFileAsync()
#pragma warning restore CA1822 // Mark members as static
        {
            if (File.Exists(StorageFile))
            {
                var json = await File.ReadAllTextAsync(StorageFile);
                return JsonConvert.DeserializeObject<List<Friend>>(json);
            }

            return null;
        }

#pragma warning disable CA1822 // Mark members as static
        private async Task WriteToFileAsync(List<Friend> friendList)
#pragma warning restore CA1822 // Mark members as static
        {
            var json = JsonConvert.SerializeObject(friendList, Formatting.Indented);
            await File.WriteAllTextAsync(StorageFile, json);
        }

        private List<Friend> GetAllItemsOrDefault()
        {
            List<Friend>? friendList = ReadFromFileAsync().Result;

            if (friendList is null)
            {
                friendList = FriendListByDefault;
                WriteToFileAsync(friendList).Wait();
            }

            return friendList;
        }

        private void InsertItem(Friend friend)
        {
            var friendList = GetAllItemsOrDefault();
            var maxFriendId = (friendList.Count == 0) ? 0 : friendList.Max(friend => friend.Id);

            friend.Id = maxFriendId + 1;
            friendList.Add(friend);
            WriteToFileAsync(friendList).Wait();
        }

        private void UpdateItem(Friend friend)
        {
            var friendList = GetAllItemsOrDefault();
            var friendExisting = friendList.Single(item => item.Id == friend.Id);
            var indexOfExisting = friendList.IndexOf(friendExisting);

            friendList.Insert(indexOfExisting, friend);
            friendList.Remove(friendExisting);
            WriteToFileAsync(friendList).Wait();
        }
    }
}
