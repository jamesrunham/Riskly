using System.Collections.Generic;

namespace HeatMapWebApi.Models
{
    public class NodeDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<NodeDto> children { get; set; }
        public NodeData data { get; set; }

        public NodeDto()
        {
            children = new List<NodeDto>();
            data = new NodeData();
        }
    }

    public class NodeData
    {
        public int checkins { get; set; }
        public string colourHex { get; set; }
        public bool isFolder { get; set; }
        public double percentage { get; set; }

        public NodeData()
        {
            isFolder = true;
        }
    }
}