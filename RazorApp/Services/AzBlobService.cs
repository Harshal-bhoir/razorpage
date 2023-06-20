using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RazorApp.Models;

namespace RazorApp.Services;

public class AzBlobService: IAzBlobService
{
	private readonly BlobServiceClient _blobClient;
	private readonly BlobContainerClient _blobContClient;

	public AzBlobService(BlobServiceClient blobServiceClient, BlobContainerClient blobContainerClient)
	{
		_blobClient = blobServiceClient;
		_blobContClient = blobContainerClient;
	}

	public async Task<List<Azure.Response<BlobContentInfo>>> uploadFile(ImageUploadModel singleFile)
	{
		List<IFormFile> files = new List<IFormFile>();
		files.Add(singleFile.imageFile);
		List<Azure.Response<BlobContentInfo>> azResponse = new List<Azure.Response<BlobContentInfo>>();
		foreach(IFormFile file in files)
		{
			string fileName = file.FileName;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				file.CopyTo(memoryStream);
				memoryStream.Position = 0;
                Azure.Response<BlobContentInfo> client = await _blobContClient.UploadBlobAsync(fileName, memoryStream, default);
                azResponse.Add(client);
            }
		}

		return azResponse;
	}
}

