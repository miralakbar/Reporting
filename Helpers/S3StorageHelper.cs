using Application.CustomExceptions;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Enums;
using System;
using System.Threading.Tasks;
using Application.CustomExceptions.SharedExceptions;
using Application.Extensions;
using Sentry;

namespace Application.Helpers
{
    public static class S3StorageHelper
    {
        public static async Task<S3StoredItemDTO> HandleS3StorageAsync(string directoryName, string objectName, string photoBase64, IS3AzCloudService azCloudService, ILogger logger)
        {
            if (string.IsNullOrEmpty(photoBase64))
            {
                string errorMessage = $"{CustomExceptionCodes.ImageForS3IsNotProvided.GetEnumDescription()} \n Destination: {directoryName} - {photoBase64}";
                throw new S3StorageException(errorMessage);
            }

            try
            {
                Convert.FromBase64String(photoBase64);
            }
            catch (Exception ex)
            {
                throw new S3StorageException(ex.Message);
            }

            return await azCloudService.UploadObjectAsync(directoryName, objectName, photoBase64);
        }

        public static async Task<string> GetImageUrlAsync(string objectName, IS3AzCloudService azCloudService, ILogger logger)
        {
            return await azCloudService.GetExistingImageUrlAsync(objectName);
        }

        public static async Task<bool> CopyImageUrlAsync(string operationObjectName, string destinationDirectory,
                                                           string destinationObjectName, IS3AzCloudService azCloudService, ILogger logger)
        {
			try
			{
				await logger.LogConsoleAsync($"Attempting to copy URL for object: {operationObjectName}");

				var isSuccess = await azCloudService.CopyObjectAsync(operationObjectName, destinationDirectory, destinationObjectName);

				if (isSuccess)
				{
					await logger.LogConsoleAsync($"Successfully copied the URL: {destinationObjectName}");
				}
				else
				{
					await logger.LogConsoleAsync("Failed to copy the URL.");
				}

				return isSuccess;
			}
			catch (Exception ex)
			{
				string errorMessage = $"Error copying object: {operationObjectName}. Exception: {ex.Message}";
				await logger.LogConsoleAsync(errorMessage);
				SentrySdk.CaptureException(ex);
				return false;
			}
		}


        public static async Task<bool> DeleteS3StorageAsync(string fullObjectName, IS3AzCloudService azCloudService, ILogger logger)
        {
            if (string.IsNullOrEmpty(fullObjectName))
            {
                string errorMessage = ExceptionMessageBuilder.Build(CustomExceptionCodes.ImageForS3IsNotProvided, null, null, $"Destination: {fullObjectName}");
                throw new S3StorageException(errorMessage);
            }

            return await azCloudService.DeleteObjectAsync(fullObjectName);
        }
    }
}
