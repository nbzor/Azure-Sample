using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace T5.Common
{
    public static class StorageUtils
    {
        static CloudStorageAccount _storageAccount;
        static CloudBlobClient _blobClient;
        static CloudBlobContainer _blobContainer;
        static CloudQueueClient _queueClient;
        static CloudTableClient _tableClient;
        
        public static CloudStorageAccount StorageAccount
        {
            get
            {
                if (_storageAccount == null)
                    _storageAccount = CloudStorageAccount.
                        Parse((string)Properties.Settings.Default["StorageConnectionString"]);
                return _storageAccount;
            }
        }

        public static CloudBlobClient BlobClient
        {
            get
            {
                if (_blobClient == null)
                    _blobClient = StorageAccount.CreateCloudBlobClient();
                return _blobClient;
            }
        }

        public static CloudBlobContainer BlobContainer
        {
            get
            {
                if (_blobContainer == null)
                {
                    _blobContainer = BlobClient.GetContainerReference(
                              (string)Properties.Settings.Default["BlobStorageString"]);
                    _blobContainer.CreateIfNotExists();
                    var permision = _blobContainer.GetPermissions();
                    permision.PublicAccess = BlobContainerPublicAccessType.Blob;
                    _blobContainer.SetPermissions(permision);
                }
                return _blobContainer;
            }
        }

        public static CloudQueueClient QueueClient
        {
            get
            {
                if (_queueClient == null)
                   _queueClient = StorageAccount.CreateCloudQueueClient();
                return _queueClient;
            }
        }

        public static CloudTableClient TableClient
        {
            get
            {
                if (_tableClient == null)
                    _tableClient = StorageAccount.CreateCloudTableClient();
                return _tableClient;
            }
        }        
    }
}
