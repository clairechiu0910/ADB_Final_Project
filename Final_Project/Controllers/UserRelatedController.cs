﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class UserRelatedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}