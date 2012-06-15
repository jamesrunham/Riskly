using System.Net;
using System.Web.Http;
using HeatMapWebApi.Interfaces;
using HeatMapWebApi.Models;
using HeatMapWebApi.Repositories;

namespace HeatMapWebApi.Controllers
{
    public class HeatMapController : ApiController
    {
        private static readonly INodeRepository NodeRepository = new SourceTreeRepository();

        public NodeDto Get(string pathContains, string iteration, int depth)
        {
            var nodes = NodeRepository.GetAllNodes(pathContains, iteration, depth);

            if(nodes == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return nodes.Root;
        }
    }
}
