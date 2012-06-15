using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HeatMapWebApi.Interfaces;
using HeatMapWebApi.Models;

namespace HeatMapWebApi.Repositories
{
    public class SourceTreeRepository : INodeRepository
    {
        public ISourceTree GetAllNodes()
        {
            throw new NotImplementedException();
        }

        public ISourceTree GetAllNodes(string pathContains, string iteration, int depth)
        {
            var sourceItems = LogReader.GetSourceItems(pathContains, iteration);
            
            if(sourceItems == null)
                return null;

            var tree = new SourceTree();
            
            tree.AddFiles(sourceItems, depth);

            var colours = new ColourConverter();

            colours.ChildColours(tree.Root, tree.Root.children.FirstOrDefault().data.checkins);

            return tree;
        }

        public NodeDto GetNodes()
        {
            throw new NotImplementedException();
        }
    }
}