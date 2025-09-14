using Core.CoreLib.Services.Azure.Storage;
using Microsoft.AspNetCore.Http;

namespace Core.Backend.Test.Tests
{
    public class StorageServiceTest : TestBase
    {
        [Fact]
        public async Task UploadMediaTest()
        {
            // Scaffold
            var config =
                CreateConfig();

            var storageService =
                new StorageService(config, new BlobService(config));

            Assert.NotNull(storageService);

            //Setup mock file using a memory stream
            var content = "Contents of a fake PDF";
            var fileName = "deleteme.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            var file = 
                new FormFile(stream, 0, stream.Length, "uniqueId", fileName);

            var result =
                await
                storageService
                .UploadMediaAsync(
                    1, 
                    file.FileName,
                    file.OpenReadStream());
            
            Assert.NotNull(result);
            Assert.False(result.Error);

                var deleteResult =
                   await
                   storageService
                   .DeleteMedia(1, file.FileName);
            
            Assert.True(deleteResult);
        }

       // [Fact]
        //private async Task UploadSiteMediaTest()
        //{
        //    // will look much like UploadMediaTest()
        //}
    }
}
