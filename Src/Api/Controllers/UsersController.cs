using Coreapi.Api.Attributes;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Features.Clients.Queries.GetUserAreas;
using Coreapi.Application.Features.User.Commands.AddUser;
using Coreapi.Application.Features.User.Commands.AddUserFile;
using Coreapi.Application.Features.User.Commands.DeleteUserFile;
using Coreapi.Application.Features.User.Commands.ResetPassword;
using Coreapi.Application.Features.User.Queries.GetAvatar;
using Coreapi.Application.Features.User.Queries.GetClientUserFiles;
using Coreapi.Application.Features.User.Queries.GetUserBalance;
using Coreapi.Application.Features.User.Queries.GetUserFile;
using Coreapi.Application.Features.User.Queries.GetUserInfo;
using Coreapi.Application.Features.User.Queries.GetUsersForSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Coreapi.Api.Controllers
{
	public class UsersController : BaseController
	{
		private readonly IFileService fileService;
		private readonly IS3Service _s3Service;

		public UsersController(IFileService fileService, IS3Service s3Service)
		{
			this.fileService = fileService;
			_s3Service = s3Service;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserInfo() => Ok(await Mediator.Send(new GetUserInfoQuery()));


		[HttpPost]
		[Authorize(Roles = "Administrator, SuperUser")]
		public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
		{
			await Mediator.Send(command);
			return NoContent();
		}

	

		[HttpPost]
		[Authorize(Roles = "SuperUser")]
		public async Task<IActionResult> AddAnalyzer(AddUserCommand command)
		{
			//command.Role = "Analyzer";
			await Mediator.Send(command);
			return NoContent();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddExeEngByNaCode(AddUserCommand command) => Ok(await Mediator.Send(command));

		[HttpGet]
		public async Task<IActionResult> GetAvatar([FromQuery] GetAvatarQuery query) => Ok(await Mediator.Send(query));

		[HttpGet]
		public async Task<IActionResult> GetAreas([FromQuery] GetClientUserAreasQuery query) => Ok(await Mediator.Send(query));


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserFile([FromQuery] GetUserFileQuery query) =>
			Ok(await Mediator.Send(query));

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserBalance() => Ok(await Mediator.Send(new GetUserBalanceQuery()));

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUsersForSupport() =>
			Ok(await Mediator.Send(new GetUsersForSupportQuery()));

		[HttpGet]
		public IActionResult GetPhysicalFile([FromQuery] string path)
		{
			var filePath = fileService.GetFullPath(path);
			if (!System.IO.File.Exists(filePath))
				return NoContent();
			new FileExtensionContentTypeProvider().TryGetContentType(path, out var contentType);
			contentType ??= "application/octet-stream";
			return PhysicalFile(filePath, contentType);
		}

		[HttpGet]
		public async Task<IActionResult> GetPhysicalFileS3([FromQuery] string path)
		{
			var currentDateTime = DateTime.Now;
			var formattedDateTime = currentDateTime.ToString("yyyyMMdd_HHmmss");

			var imageStream = await _s3Service.GetFullPath(path);

			new FileExtensionContentTypeProvider().TryGetContentType(path, out var contentType);
			contentType ??= "application/octet-stream";

			Response.Headers.Add("Content-Disposition", new ContentDisposition
			{
				FileName = $"File_{formattedDateTime}_{Path.GetFileName(path)}",
				Inline = true // false = prompt the user for downloading; true = browser to try to show the file inline
			}.ToString());

			return File(imageStream, contentType);
		}

		public class FileRequest
		{
			public List<string> FilePaths { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> GetPhysicalZipFileS3([FromBody] FileRequest request)
		{
			if (request.FilePaths == null || !request.FilePaths.Any())
			{
				return BadRequest("No files selected.");
			}

			var tempFileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");

			try
			{
				using (var zip = ZipFile.Open(tempFileName, ZipArchiveMode.Create))
				{
					foreach (var filePath in request.FilePaths)
					{
						var stream = await _s3Service.GetFullPath(filePath); // Assuming this method gets the stream from S3

						if (stream != null)
						{
							var entryName = Path.GetFileName(filePath);
							var entry = zip.CreateEntry(entryName, CompressionLevel.Fastest);

							using (var entryStream = entry.Open())
							{
								await stream.CopyToAsync(entryStream);
							}
							stream.Dispose(); // Dispose the stream after use
						}
						else
						{
							// Handle case where file stream is null (e.g., file not found in S3)
							return NotFound($"File '{filePath}' not found in S3.");
						}
					}
				}

				var zipBytes = System.IO.File.ReadAllBytes(tempFileName);
				System.IO.File.Delete(tempFileName);

				return File(zipBytes, "application/zip", "files.zip");
			}
			catch (Exception ex)
			{
				// Handle any exceptions
				Console.WriteLine($"Error creating zip file: {ex.Message}");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddFile([FromForm] AddUserFileCommand command) => Ok(await Mediator.Send(command));

		[HttpPost]
		public async Task<IActionResult> DeleteFile(DeleteUserFileCommand command)
		{
			await Mediator.Send(command);
			return NoContent();
		}

		[HttpGet]
		public async Task<IActionResult> GetUserFiles([FromQuery] GetClientUserFilesQuery query) =>
			Ok(await Mediator.Send(query));
	}
}
