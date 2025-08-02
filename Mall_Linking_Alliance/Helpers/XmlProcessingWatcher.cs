using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mall_Linking_Alliance.Helpers;
using Mall_Linking_Alliance.Model;
using Mall_Linking_Alliance.Properties;


namespace Mall_Linking_Alliance.Watchers
{
    public class XmlProcessingWatcher
    {
        private string _watchPath;
        private TblSettings _settings;
        private FileSystemWatcher _watcher;

        private readonly ConcurrentQueue<string> _fileQueue = new ConcurrentQueue<string>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public XmlProcessingWatcher(string watchPath, TblSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // ✅ Validate settings and fix paths if needed
            SettingsValidator.ValidateAndFixSettings(settings);

            // Always use updated, validated path
            _watchPath = _settings.BrowseDb ?? throw new ArgumentNullException(nameof(_settings.BrowseDb));
        }

        public void Start()
        {
            Console.WriteLine($"👀 Watching for XML files in: {_watchPath}");

            _watcher = new FileSystemWatcher(_watchPath, "*.xml")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime
            };

            _watcher.Created += OnFileCreated;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;

            Task.Run(ProcessQueueLoop);
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // Sometimes file is not ready yet, add a small delay
            Task.Delay(500).ContinueWith(_ =>
            {
                if (File.Exists(e.FullPath))
                {
                    _fileQueue.Enqueue(e.FullPath);
                    _signal.Release();
                }
            });
        }

        private async Task ProcessQueueLoop()
        {
            while (true)
            {
                await _signal.WaitAsync();

                while (_fileQueue.TryDequeue(out var filePath))
                {
                    try
                    {
                        Console.WriteLine($"🔄 Processing: {Path.GetFileName(filePath)}");

                        string xml = await SafeReadFileAsync(filePath);

                        XmlProcessor.Process(xml, filePath, _settings); // still uses _settings.SaveDb
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Failed to process {filePath}: {ex.Message}");
                    }
                }
            }
        }

        private async Task<string> SafeReadFileAsync(string path, int maxAttempts = 10, int delayMs = 300)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
                catch (IOException)
                {
                    await Task.Delay(delayMs);
                }
            }

            throw new IOException($"File '{path}' could not be read after {maxAttempts} attempts.");
        }
    }
}
