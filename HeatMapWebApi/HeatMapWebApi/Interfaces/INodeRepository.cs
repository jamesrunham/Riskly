using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeatMapWebApi.Models;
using HeatMapWebApi.Repositories;

namespace HeatMapWebApi.Interfaces
{
    public interface INodeRepository
    {
        NodeDto GetNodes();
        ISourceTree GetAllNodes();
        ISourceTree GetAllNodes(string pathContains, string iteration, int depth);
    }
}
