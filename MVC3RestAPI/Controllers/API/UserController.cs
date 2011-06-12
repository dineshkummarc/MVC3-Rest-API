using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC3RestAPI.Models;
using REST;

namespace MVC3RestAPI.Controllers.API
{
    public class UserController : Controller
    {
        public ActionResult List()
        {
            var users = new List<User>
                            {
                                new User
                                    {Id = 1, Email = "ScottGu@microsoft.com", Name = "Scott Gurthie"},
                                new User
                                    {Id = 2, Email = "OmarALZabir@gmail.com", Name = "Omar AL Zabir"}
                            };

            return new ObjectResult<List<User>>(users);
        }

        public ActionResult Read(int id)
        {
            return new ObjectResult<User>(new User
            {
                Id = id,
                Email = "omaralzabir@gmail.com",
                Name = "Omar"
            });
        }

        [ObjectFilter(Param = "users", RootType = typeof(Models.User[]))]
        public ActionResult Update(Models.User[] users)
        {
            foreach (var user in users)
            {
                user.Id = 5;
            }
            return new ObjectResult<User[]>(users);
        }

        [ObjectFilter(Param = "users", RootType = typeof(Models.User[]))]
        public ActionResult Create(Models.User[] users)
        {
            foreach (var user in users)
            {
                user.Id = 15;
            }
            return new ObjectResult<Models.User[]>(users);
        }

        public ActionResult Delete(string id)
        {
            return new BasicActionResult(true);

        }

        public ActionResult ReturnError()
        {
            return new BasicActionResult(false, "Some Error Occured");
        }

    }
}
