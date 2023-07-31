using AutoMapper;
using ESourcing.Core.Entities;
using ESourcing.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<AppUser, AppUserViewModel>().ReverseMap();
        }
    }
}
