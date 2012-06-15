using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HeatMapWebApi.Interfaces;
using HeatMapWebApi.Models;
using System.Runtime.Serialization;


namespace HeatMapWebApi.Repositories
{
    public class SourceTree : ISourceTree
    {
        public NodeDto Root { get; set; }
        private int NodeCount;

        public void AddFiles(IEnumerable<SourceItemDto> files, int checkins)
        {
            foreach (var file in files)
            {
                AddFile(file);
            }
            CullNodes(Root, checkins);
        }

        public void AddFile(SourceItemDto file)
        {
            var parts = file.Path.Split('/');
            var depth = parts.Length;

            //sets root to first segment of file path
            if (Root == null)
            {
                Root = new NodeDto
                {
                    name = parts[2],
                    id = NodeCount++.ToString(CultureInfo.InvariantCulture)
                };
            }

            var currentNode = Root;

            //Start at 2 to always skip '$/reed' and the root
            for (var i = 3; i < depth; i++)
            {
                var existingNode = currentNode.children.FirstOrDefault(n => n.name == parts[i]);
                if (existingNode == null)
                {
                    var newNode = new NodeDto
                    {
                        id = NodeCount++.ToString(CultureInfo.InvariantCulture),
                        name = parts[i],

                        //FullPath = file.Path, 
                        //IsFile = Path.GetExtension(parts[i]).Length > 0
                    };
                    newNode.data.checkins = file.NumChanges;
                    currentNode.children.Add(newNode);
                    currentNode = newNode;
                }
                else
                {
                    currentNode = existingNode;
                    currentNode.data.checkins += file.NumChanges;
                }
            }
        }

        public void CullNodes(NodeDto node, int checkins)
        {
            if (node != null && node.children != null)
            {
                foreach (var child in node.children)
                {
                    child.children = child.children
                                        .Where(n => n.data.checkins >= checkins)
                                        .OrderBy(n => n.data.checkins)
                                        .ToList();

                    CullNodes(child, checkins);
                }
            }

        }
    }
}