using System.Reflection;
using MongoDB.Driver;
using Storage.Common.Services;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
	public class StorageFileRepository : RepositoryBase<StorageFile>, IStorageFileRepository
	{
		public StorageFileRepository(IMongoDatabase db):base(db)
		{
			
		}
	}
}