using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Filters;
using LibraryApi.Services.Reservations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ResverationsController : ControllerBase
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public ResverationsController(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("reservations")]
        [ValidateModel]
        public async Task<ActionResult> AddReservations([FromBody] PostReservationRequest request)
        {
            var reservation = _mapper.Map<Reservation>(request);
            reservation.status = ReservationStatus.Pending;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<ReservationDetailResponse>(reservation);
            //await Task.Delay(response.Items.Split(',').Count() * 1000);
            //response.AvailableOn = DateTime.Now.AddDays(1);
            return CreatedAtRoute("reservations#getbyid", new { id = response.Id }, response);
        }

        [HttpGet("reservations/{id}", Name ="reservations#getbyid")]
        public async Task<ActionResult> GetReservationById(int id)
        {
            var reservation = await _context.Reservations.
                ProjectTo<ReservationDetailResponse>(_config)
                .SingleOrDefaultAsync(r => r.Id == id);
            return this.Maybe(reservation);
        }

    }






}
