using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using GolfScore.Domain;

namespace GolfScore.Services
{
    public class DataService
    {
        public DataService()
        {
        }

        public async Task SaveToDoItem(ToDoItem item)
        {
            await App.MobileService.GetTable<ToDoItem>().InsertAsync(item);
        }
    }
}
