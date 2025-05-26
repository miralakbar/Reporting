using Domain.DTOs;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IS3AzCloudService
	{
		Task<S3StoredItemDTO> UploadObjectAsync(string directoryName, string objectName, string base64Object);
		Task<string> GetExistingImageUrlAsync(string objectName);
		Task<bool> CopyObjectAsync(string operationObjectName, string destinationDirectory, string destinationObjectName);
		Task<bool> DeleteObjectAsync(string fullObjectName);
	}
}
