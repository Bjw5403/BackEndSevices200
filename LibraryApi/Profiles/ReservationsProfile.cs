﻿using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Services.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Profiles
{
    public class ReservationsProfile : Profile
    {
        public ReservationsProfile()
        {
            CreateMap<Reservation, ReservationDetailResponse>();
            CreateMap<PostReservationRequest, Reservation>();
        }
    }
}
