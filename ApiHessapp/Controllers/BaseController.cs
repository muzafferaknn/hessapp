using ApiHessapp.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiHessapp.Controllers
{
    public class BaseController : ApiController
    {
        public List<string> SplitParticipants(int groupId)
        {
            using (HessappEntities ent = new HessappEntities())
            {
                var prt = ent.Group.Where(q => q.GroupID == groupId).Select(q => q.Participants).SingleOrDefault();
                string[] split = prt.ToString().Split('/');
                return split.ToList();
            }
        }
    }
}
