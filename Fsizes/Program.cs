global using System.IO;

namespace Fsizes;

class Program
{
    static async Task Main(string[] args)
    {
        string dir;
        if (args.Length == 0)
            dir = Environment.CurrentDirectory;
        else if (Directory.Exists(args[0]))
            dir = args[0];
        else
        {
            Console.WriteLine("Invalid directory");
            return;
        }
        Console.Clear();
        DirSize ds = new(dir);
        Console.WriteLine(ds.Info.FullName);
        Progress<(ulong Files, ulong Size)> prog = new( r =>
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 1;
            Console.Write($"Files: {r.Files}\nSize:  {GetSize(r.Size)}");
        });

        await ds.EnumerateFilesAsync(new TimeThrottledProgress<(ulong Files, ulong Size)>(prog, TimeSpan.FromSeconds(0.05)), new());
        Console.CursorLeft = 0;
        Console.CursorTop = 0;
        Console.Write($"Files: {ds.FileCount}\nSize: {GetSize(ds.Size)}");

        Face(ds, true);
    }

    static string GetSize(ulong size) => size switch
    {
        0 => "0",
        < 1024L => $"{size,-7} B",
        < 1024L * 1024L => $"{size / 1024.0,-7:.00} KiB",
        < 1024L * 1024L * 1024L => $"{size / (1024.0 * 1024.0),-7:.00} MiB",
        < 1024L * 1024L * 1024L * 1024L => $"{size / (1024.0 * 1024.0 * 1024.0),-7:.00} GiB",
        _ => $"{size / (1024.0 * 1024.0 * 1024.0 * 1024.0),-7:.00} TiB"
    };

    static void Face(DirSize ds, bool clear = false)
    {
        DirSize cur = ds;
        DirSize? swap = null;
        bool write = true;
        while (true)
        {
            if (write)
            {
                if (clear)
                    Console.Clear();
                Console.WriteLine(cur.Info.FullName);
                foreach (var s in cur.Directories.OrderBy(p => p.Size))
                    Console.WriteLine($" |- {GetSize(s.Size),-12}   {s.Info.Name}");
                foreach (var s in cur.Files.OrderBy(p => p.Length))
                    Console.WriteLine($" |  {GetSize((ulong)s.Length),-12}   {s.Name}");
                Console.WriteLine($"Total files: {cur.FileCount}");
                Console.WriteLine($"Total size:  {GetSize(cur.Size)}");
            }
            Console.Write(" > ");
            write = true;
            string? val = Console.ReadLine();
            switch (val)
            {
                case ":exit":
                    return;
                case "..":
                    try
                    {
                        if (ds.TryGetDir(Directory.GetParent(cur.Info.FullName)!.FullName, out swap))
                        {
                            cur = swap!;
                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Already at top");
                        write = false;
                    }
                    Console.WriteLine("Couldn't go higher");
                    write = false;
                    break;
                case null:
                    break;
                default:
                    if (cur.TryGetDir(val, out swap))
                    {
                        cur = swap!;
                        break;
                    }
                    Console.WriteLine("Couldn't get that directory");
                    write = false;
                    break;
            }
        }
    }
}