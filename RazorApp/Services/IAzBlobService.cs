using System;
using Azure.Storage.Blobs.Models;
using Azure;
using System.Runtime.ConstrainedExecution;
using RazorApp.Models;

namespace RazorApp.Services;

	public interface IAzBlobService
	{
		Task<List<Azure.Response<BlobContentInfo>>> uploadFile(ImageUploadModel singleFile);
	}

