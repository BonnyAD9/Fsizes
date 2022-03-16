namespace Fsizes;

internal class DirSize
{
    public DirectoryInfo Info { get; init; }
    public ulong Size { get; private set; } = 0;
    public ulong FileCount { get; private set; } = 0;

    public FileInfo[] Files { get; private set; } = Array.Empty<FileInfo>();

    public DirSize[] Directories { get; private set; } = Array.Empty<DirSize>();

    public DirSize(DirectoryInfo info) => Info = info;

    public DirSize(string path) => Info = new(path);

    public async Task EnumerateFilesAsync(IProgress<(ulong Files, ulong Size)> progress, CancellationToken cancellationToken) => await EnumerateFilesAsync(progress, 0, 0, cancellationToken);

    private async Task EnumerateFilesAsync(IProgress<(ulong Files, ulong Size)> progress, ulong TotalSize, ulong TotalFiles, CancellationToken cancellationToken)
    {
        DirectoryInfo[] dirs;
        try
        {
            dirs = Info.GetDirectories();
        }
        catch (UnauthorizedAccessException)
        {
            return;
        }
        Files = Info.GetFiles();

        foreach (var fi in Files)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            FileCount++;
            Size += (ulong)fi.Length;
            progress.Report((TotalFiles + FileCount, TotalSize + Size));
        }
        
        Directories = new DirSize[dirs.Length];


        for (int i = 0; i < dirs.Length; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            Directories[i] = new(dirs[i]);
            await Directories[i].EnumerateFilesAsync(progress, TotalSize + Size, TotalFiles + FileCount, cancellationToken);
            FileCount += Directories[i].FileCount;
            Size += Directories[i].Size;
            progress.Report((TotalFiles + FileCount, TotalSize + Size));
        }
    }

    public bool TryGetDir(string directory, out DirSize? dir)
    {
        if (directory == Info.FullName)
        {
            dir = this;
            return true;
        }
        foreach (var d in Directories)
        {
            if (d.Info.Name == directory || d.Info.FullName == directory)
            {
                dir = d;
                return true;
            }
        }
        dir = default;
        return false;
    }
}
