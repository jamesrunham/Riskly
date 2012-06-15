using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HeatMapWebApi.Models;

namespace HeatMapWebApi.Repositories
{
    public static class LogReader
    {
        public static IEnumerable<SourceItemDto> GetSourceItems(string pathContains, string iteration)
        {
            var path = PathBuilder(pathContains, iteration);
            
            var items = File
            .ReadAllLines(@"\\STORAGE\Home\Personal\James Runham\tfs.log")
            .Select(l => l.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Last())
            //restrict to certain path
            .Where(l => l.ToLowerInvariant().Contains(path.ToLowerInvariant()))
            //remove unwanted file types
            .Where(l => !Path.GetFullPath(l).Contains("jquery") && (Path.GetExtension(l) == ".js" ||
                               Path.GetExtension(l) == ".cs" ||
                               Path.GetExtension(l) == ".css" ||
                               Path.GetExtension(l) == ".config" ||
                               Path.GetExtension(l) == ".cshtml" ||
                               Path.GetExtension(l) == ".aspx" ||
                               Path.GetExtension(l) == ".ascx"))
            .GroupBy(l => l)
            .Select(g => new { Line = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Select(g => new SourceItemDto { NumChanges = g.Count, Path = g.Line });

            return items.Any() ? items : null;
        }

        private static string PathBuilder(string area, string iteration)
        {
            return "/" + iteration + "/" + area + "/";
        }
    }
}