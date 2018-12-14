using ApiHessapp.DTO;
using ApiHessapp.ModelClassies;
using ApiHessapp.ModelClassies.Chats;
using ApiHessapp.Models.EntitiyFramework;
using System.Net;
using System.Web.Http;
using System.Linq;
using ApiHessapp.DTO.Chats;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using ApiHessapp.DTO.Groups;

namespace ApiHessapp.Controllers
{
    public class GroupController : BaseController
    {
        [HttpPost]
        public IHttpActionResult Create(CreateGroupRequestDTO model)
        {
             using (HessappEntities ent = new HessappEntities())
                {
                if (ModelState.IsValid)
                {
                    Group grp = new Group();
                    grp.Name = model.groupName;
                    grp.Description = model.description;
                    grp.Moderator = model.moderator;
                    foreach (var item in model.participants)
                    {
                        int last = model.participants.LastIndexOf(item);
                        if (last == model.participants.Count - 1)
                            grp.Participants += item;
                        else
                            grp.Participants += item + "/";
                    }
                    ent.Group.Add(grp);
                    ent.SaveChanges();
                    CreateGroupResponseDTO rsp = new CreateGroupResponseDTO();
                    rsp.groupID = grp.GroupID;
                    rsp.moderator = grp.Moderator;
                    rsp.description = grp.Description;
                    rsp.name = grp.Name;
                    rsp.participants = SplitParticipants(grp.GroupID);
                    return Content(HttpStatusCode.OK, rsp);

                }
                else
                    return BadRequest();
                }
        }

        [HttpPost]
        public IHttpActionResult Delete(GroupDeleteRequestDTO model) // moderator ise
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent =new HessappEntities())
                {
                    var del = ent.Group.Find(model.groupId);
                    GroupDeleteResponseDTO rsp = new GroupDeleteResponseDTO();
                    rsp.groupId = model.groupId;
                    if (del==null)
                    {
                        return NotFound();
                    }
                    if (del.Moderator!=model.nickname)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        var act = ent.Activity.Where(q => q.GroupID == model.groupId).Select(q => q).ToList();
                        var spd = ent.Spends.Where(q => q.GroupID == model.groupId).Select(q => q).ToList();
                        if (act!=null)
                        {
                            foreach (var item in act)
                                ent.Activity.Remove(item);
                        }
                        if (spd!=null)
                        {
                            foreach (var item in spd)
                                ent.Spends.Remove(item);
                        }
                    }
                    ent.Group.Remove(del);
                    ent.SaveChanges();
                    
                return Content(HttpStatusCode.OK,rsp);
                }
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult ListGroups(ListGroupRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                using (HessappEntities ent =new HessappEntities())
                {
                    List<ListGroupResponseDTO> list = (from p in ent.Group
                                where p.Participants.Contains(model.nickname)
                                select new ListGroupResponseDTO
                                {
                                    groupID=p.GroupID,
                                    moderator=p.Moderator,
                                    description=p.Description,
                                    name=p.Name,
                                }).ToList();
                    foreach (var item in list)
                    {
                        var grpId = item.groupID;
                        List<Spends> spn =ent.Spends.Where(q => q.GroupID == grpId).Select(q => q).ToList();
                        item.spends =spn;
                        item.participants = SplitParticipants(item.groupID);
                    }
                    if (list == null)
                        return NotFound();
                    else
                    {
                        GroupListGeneralResponseDTO grps = new GroupListGeneralResponseDTO();
                        grps.groups = list;
                        return Content(HttpStatusCode.OK,grps);

                    }
                }
            }
            else
                return BadRequest();
        }
        

    }
}
