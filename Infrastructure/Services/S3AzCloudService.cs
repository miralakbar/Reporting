using Application.CustomExceptions.SharedExceptions;
using Application.Extension;
using Application.Interfaces;
using Application.Settings;
using Domain.DTOs;
using Domain.Enums;
using Minio;
using Minio.DataModel.Args;
using System;
using System.IO;
using System.Threading.Tasks;
using Application.Extensions;
using Microsoft.Identity.Client;

namespace Infrastructure.Services
{
    public class S3AzCloudService : IS3AzCloudService
    {
        private readonly S3AzCloudSettings _s3Settings;
        private readonly IMinioClient _minioClient;
        private readonly ILogger _logger;
        public S3AzCloudService(S3AzCloudSettings s3Settings,
            IMinioClient minioClient,
            ILogger logger)
        {
            _s3Settings = s3Settings;
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<S3StoredItemDTO> UploadObjectAsync(string directoryName, string objectName, string base64Image)
        {
            await _logger.LogConsoleAsync($"We are in the S3StorageService. Object Name (Guid): {objectName}");
            var bucketName = _s3Settings.DefaultBucketName;
            var fileBytes = Convert.FromBase64String(base64Image);
            string extension = ".jpeg";

            string fullObjectName = $"{directoryName}/{objectName + extension}";

            using (MemoryStream stream = new MemoryStream(fileBytes))
            {
                if (stream is null)
                {
                    string errorMessage = CustomExceptionCodes.FileStreamIsNullException.GetEnumDescription();
                    throw new S3StorageException(errorMessage);
                }

                extension = stream.GetFileExtension();

                if (extension is null)
                {
                    string errorMessage = $"{CustomExceptionCodes.ImageExtensionForBase64NotDetermined.GetEnumDescription()} \n Image: {base64Image}";
                    throw new S3StorageException(errorMessage);
                }

                stream.Position = 0;

                PutObjectArgs putObjectArgs = new PutObjectArgs()
                                    .WithBucket(bucketName)
                                    .WithObject(fullObjectName)
                                    .WithStreamData(stream)
                                    .WithObjectSize(stream.Length);
                try
                {
                    await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                    await _logger.LogConsoleAsync($"Successfully uploaded the document photo to S3. Object: {objectName}");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{CustomExceptionCodes.ImageUploadToS3Exception.GetEnumDescription()} \n Original exception message: {ex.Message}";
                    throw new S3StorageException(errorMessage);
                }
            }

            await _logger.LogConsoleAsync("Generating a SAS url for the uploaded document photo...");
            PresignedGetObjectArgs presignedGetObjectArgs = new PresignedGetObjectArgs()
                                                                 .WithBucket(bucketName)
                                                                 .WithObject(fullObjectName)
                                                                 .WithExpiry(60 * 60 * 24 * 7);
            string minioUrl = string.Empty;

            try
            {
                minioUrl = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
            }
            catch (Exception ex)
            {
                string errorMessage = $"{CustomExceptionCodes.PresignedURLRetrievalFromS3Exception.GetEnumDescription()} \n Original exception message: {ex.Message}";
                throw new S3StorageException(errorMessage);
            }

            await _logger.LogConsoleAsync($"Done generating the SAS url for the uploaded document photo. URL: {minioUrl}");

            return new S3StoredItemDTO()
            {
                BucketName = bucketName,
                ObjectName = fullObjectName,
                ObjectTempLink = minioUrl
            };
        }

        public async Task<string> GetExistingImageUrlAsync(string objectName)
        {
            await _logger.LogConsoleAsync($"Retrieving S3 object and converting to Base64. Object Name (Guid): {objectName}");
            var bucketName = _s3Settings.DefaultBucketName;

            try
            {
                // Retrieving the object from S3
                await _logger.LogConsoleAsync("Fetching the object from S3...");
                using var memoryStream = new MemoryStream();
                GetObjectArgs getObjectArgs = new GetObjectArgs()
                                                .WithBucket(bucketName)
                                                .WithObject(objectName)
                                                .WithCallbackStream(stream =>
                                                {
                                                    stream.CopyTo(memoryStream);
                                                });

                await _minioClient.GetObjectAsync(getObjectArgs);
                await _logger.LogConsoleAsync("Successfully retrieved the object from S3.");

                // Converting the object content to Base64
                var base64String = Convert.ToBase64String(memoryStream.ToArray());
                await _logger.LogConsoleAsync("Successfully converted the object to Base64.");

                return base64String;
            }
            catch (Exception ex)
            {
                string errorMessage = $"{CustomExceptionCodes.PresignedURLRetrievalFromS3Exception.GetEnumDescription()} \n Original exception message: {ex.Message}";
                throw new S3StorageException(errorMessage);
            }
        }

        public async Task<bool> CopyObjectAsync(string operationObjectName, string destinationDirectory, string destinationObjectName)
        {
            var bucketName = _s3Settings.DefaultBucketName;
            string sourceFullObjectName = $"{operationObjectName}";
            string destinationFullObjectName = $"{destinationDirectory}/{destinationObjectName}";

            try
            {
                await _logger.LogConsoleAsync($"Attempting to copy object from {sourceFullObjectName} to {destinationFullObjectName}.");

                var copySourceArgs = new CopySourceObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(sourceFullObjectName);

                var copyObjectArgs = new CopyObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(destinationFullObjectName)
                    .WithCopyObjectSource(copySourceArgs);

                await _minioClient.CopyObjectAsync(copyObjectArgs);

                await _logger.LogConsoleAsync($"Successfully copied object to {destinationFullObjectName}.");
                return true;
            }
            catch (Exception ex)
            {
                string errorMessage = $"{CustomExceptionCodes.ImageCopyToS3Exception.GetEnumDescription()} \n Original exception message: {ex.Message}";
                throw new S3StorageException(errorMessage);
            }
        }

        public async Task<bool> DeleteObjectAsync(string fullObjectName)
        {
            var bucketName = _s3Settings.DefaultBucketName;
            try
            {
                await _logger.LogConsoleAsync($"Attempting to delete object: {fullObjectName}");

                RemoveObjectArgs removeObjectArgs = new RemoveObjectArgs()
                                                        .WithBucket(bucketName)
                                                        .WithObject(fullObjectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                await _logger.LogConsoleAsync($"Successfully deleted object: {fullObjectName}");
                return true;
            }
            catch (Exception ex)
            {
                string errorMessage = $"{CustomExceptionCodes.ImageDeleteFromS3Exception.GetEnumDescription()} \n Original exception message: {ex.Message}";
                throw new S3StorageException(errorMessage);
            }
        }
    }
}
