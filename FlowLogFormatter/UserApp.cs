using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FlowLogFormatter.Configuration;
using FlowLogFormatter.Models;
using FlowLogFormatter.LogWriter.Factory;

namespace FlowLogFormatter
{
    public class UserApp
    {
        public AppSettings Settings { get; }
        BlobContainerClient AzBlobContainerClient { get; }

        public UserApp(AppSettings settings)
        {
            Settings = settings;
            AzBlobContainerClient = GetBlobContainerClient();
        }

        public void Run()
        {
            DisplayAppInfo();
            var nameFilter = GetNameFilter();

            var blobs = AzBlobContainerClient.GetBlobs();
            var hourLimit = SetHourLimit(nameFilter);

            var rawLogContent = FilterBlobs(blobs, nameFilter, hourLimit);
            var logWriter = new LogWriter.Factory.LogWriter().Load((LogWriterName)Enum.Parse(typeof(LogWriterName), Settings.LogOutput), Settings);
            logWriter.ParseContent(rawLogContent, nameFilter);
        }

        List<string> FilterBlobs(Azure.Pageable<BlobItem> blobs, NameFilter nameFilter, int hourLimit)
        {
            var result = new List<string>();

            var blobNames = new List<string>();
            for (int i = 0; i < hourLimit; i++)
            {
                var hourFilter = i < 10 ? $"0{i}" : i.ToString();
                var blobName = $"{Settings.BlobNamePrefix}/y={nameFilter.Year}/m={nameFilter.Month}/d={nameFilter.Day}/h={hourFilter}{Settings.BlobNameSuffix}";
                blobNames.Add(blobName);
            }

            foreach(var blob in blobs)
            {
                if(blobNames.Contains(blob.Name))
                {
                    var blobClient = AzBlobContainerClient.GetBlobClient(blob.Name);
                    BlobDownloadResult blobContent = blobClient.DownloadContent();
                    result.Add(blobContent.Content.ToString());
                }
            }
            return result;
        }

        static int SetHourLimit(NameFilter nameFilter)
        {
            var filterDate = new DateTime(year: int.Parse(nameFilter.Year), month: int.Parse(nameFilter.Month), day: int.Parse(nameFilter.Day));
            if (filterDate == DateTime.Today)
                return DateTime.UtcNow.Hour;
            else
                return 23;
        }

        BlobContainerClient GetBlobContainerClient()
        {
            var blobUri = $"https://{Settings.StorageAccountName}.blob.core.windows.net?{Settings.SASToken}";
            return new BlobServiceClient(new Uri(blobUri), null).GetBlobContainerClient(Settings.FlowLogsContainer);
        }

        static NameFilter GetNameFilter()
        {
            var result = new NameFilter();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n==Log Filter==");
            Console.Write("\nYear: ");
            Console.ResetColor();
            result.Year = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nMonth: ");
            Console.ResetColor();
            result.Month = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nDay: ");
            Console.ResetColor();
            result.Day = Console.ReadLine();

            return result;
        }

        static void DisplayAppInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n[[[[[ Flow Log Formatter v1.0 ]]]]]");
            Console.ResetColor();
        }
    }
}
