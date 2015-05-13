using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using T5.Common.Models;

namespace T5.Common
{
    public class ManageTable<T> where T: TableEntity
    {
        private CloudTable _table = StorageUtils.TableClient.
            GetTableReference((string)Properties.Settings.Default["TableStorageString"]);

        public int Trys { get; set; } = 15;
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(1);


        public async Task AddMessage(T entity,string guid)
        {
            await _table.CreateIfNotExistsAsync();
            TableOperation op = TableOperation.InsertOrReplace(entity);
            var tableResult = await _table.ExecuteAsync(op);
        }

        public async Task<T> GetMessage(string guid)
        {
            await _table.CreateIfNotExistsAsync();
            int _trys = 0;
            while(_trys<Trys)
            {
                TableOperation op = TableOperation.Retrieve<AzureTableEntity<TaskImage>>("taskdone", guid);
                var tableResult = await _table.ExecuteAsync(op);

                if (tableResult.Result != null)
                    return (T)tableResult.Result;
                _trys++;
                await Task.Delay(Delay);
            }
            return default(T);
        }
    }
}
