using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeatMapWebApi.Models;

namespace HeatMapWebApi.Interfaces
{
    public interface ISourceTree
    {
        NodeDto Root { get; set; }
        void AddFiles(IEnumerable<SourceItemDto> files, int checkins);
        void AddFile(SourceItemDto file);
    }
}
