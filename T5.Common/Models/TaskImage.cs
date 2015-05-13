using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace T5.Common.Models
{
    public class Filter
    {
        public string Type { get; set; }
        public string URL { get; set; }
    }

    public class NewTaskImage
    {
        public string OriginalURL { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string GUID { get; set; }
    }

    public class TaskImage
    {
        public string OriginalURL { get; set; }
        public string CurrentURL { get; set; }
        public List<Filter> Filters { get; } = new List<Filter>();
        public string GUID { get; set; }
    }

    public class AzureTableEntity<T> : TableEntity
    {
        private T _entity;
        public AzureTableEntity() { }

        public AzureTableEntity(string guid)
        {
            PartitionKey = "taskdone";
            RowKey = guid;
        }

        public string Data { get; set; }

        [IgnoreProperty]
        public T Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                _entity = value;
                if (value != null)
                    Data = JsonConvert.SerializeObject(value);
            }
        }

    }
}
