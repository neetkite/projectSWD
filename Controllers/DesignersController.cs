using System.Collections.Generic;
using System.Linq;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeanlancerAPI2.Controllers
{
    [Route("api/Designers")]
    [ApiController]
    public class DesignersController : ControllerBase
    {
        private readonly IDesignerService _designer;
        private readonly ICategoriesService _cate;
        private readonly ICateOfDesignerService _cateO;
        private readonly ISkillService _skill;
        private readonly IUserService _user;
        private readonly ISkillOfDesignerService _skillO;
        public DesignersController(IDesignerService designerService, 
            ICateOfDesignerService cateOfDService, IUserService userService, 
            ISkillService skill, ISkillOfDesignerService skillO, ICategoriesService categories)
        {
            _designer = designerService;
            _cate = categories;
            _cateO = cateOfDService;
            _user = userService;
            _skill = skill;
            _skillO = skillO;
        }

        [HttpGet("{id}")]
        public IActionResult GetDesignerInformationById(string id)
        {
            var user = _user.GetUserByUsername(_designer.GetDesignerById(id).Username);
            if (user == null) return NotFound("Dessigner not found!");
            //.............Tìm List Skill của thg designer
            var listIdSkill = _skillO.GetSkillByDesigner(id);
            List<Skill> listSkill = new List<Skill>();
            for (int i = 0; i < listIdSkill.Count(); i++)
            {
                var skill = _skill.GetSkillById(listIdSkill.ElementAt(i).IdSkill);
                listSkill.Add(skill);
            }
            //..............Tìm List Categories của thg designer
            var listIdCate = _cateO.GetCateByDesigner(id);
            List<Categories> listCate = new List<Categories>();
            for (int i = 0; i < listIdCate.Count(); i++)
            {
                var cate = _cate.GetCategoriesById(listIdCate.ElementAt(i).IdCategories);
                listCate.Add(cate);
            }

            return Ok(new
            {
                Data = user,
                skill = listSkill ,
                categories = listCate
            });
        }





        [HttpGet("categories-name")]
        public IActionResult GetDesignerByCate([FromQuery] int id, [FromQuery] string name)
        {

            if (string.IsNullOrEmpty(name) && id > 0)
            {
                var list = _cateO.GetDesignersByCategories(id);
                List<Designer> list2 = new List<Designer>();
                for (int i = 0; i < list.Count(); i++)
                {
                    list2.Add(_designer.GetDesignerById(list.ElementAt(i).IdDesigner));
                }
                List<User> result = new List<User>();
                for (int i = 0; i < list2.Count(); i++)
                {
                    result.Add(_user.GetUserByUsername(list2.ElementAt(i).Username));
                }
                return Ok(result);
            }

            if (string.IsNullOrEmpty(name) && id == 0) return NotFound("Please choose cagories");

            var users = _user.GetUserByName(name);
            List<string> listIdD = new List<string>();

            for (int i = 0; i < users.Count(); i++)
            {
                var iDesigner = _designer.GetDesignerByUsername(users.ElementAt(i).Username);
                if(iDesigner != null) listIdD.Add(iDesigner.IdDesigner);

            }
            var listCate = _cateO.GetDesignersByCategories(id);
            var listResult = new List<string>();
            for (int i = 0; i < listCate.Count(); i++)
            {
                for (int j = 0; j < listIdD.Count(); j++)
                {
                    if (listCate.ElementAt(i).IdDesigner == listIdD.ElementAt(j))
                        listResult.Add(listIdD.ElementAt(j));
                }
            }
            List<User> finalResult = new List<User>();
            for (int i = 0; i < listResult.Count(); i++)
            {
                finalResult.Add(_user.GetUserByUsername(_designer.GetDesignerById(listResult.ElementAt(i)).Username));
            }
            
            return Ok(finalResult);
            
        }



    }
}