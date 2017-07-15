using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FileUpload.Controllers
{

    [TransactionLogFilter]
    public class WorkoutController : ApiController
    {
        private List<Workout> _workouts;

        public WorkoutController()
        {
            _workouts = new List<Workout>()
            {
                new Workout()
                {
                    Id = 1,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(1),
                    Location = "Irvine, CA",
                    Notes = "Run",
                    WorkoutType = WorkoutType.Burst
                },
                new Workout()
                {
                    Id = 2,
                    Start = DateTime.Now.AddHours(13),
                    End = DateTime.Now.AddHours(14),
                    Location = "Irvine, CA",
                    Notes = "Walk",
                    WorkoutType = WorkoutType.Cardio
                }
            };
        }

        [HttpPost]
        [Route("api/Workout")]
        public void DoWork(Workout workout)
        {
            workout.Id = _workouts.Max(a => a.Id) + 1;
            _workouts.Add(workout);
        }

        [HttpGet]
        [Route("api/Workout/{id}")]
        public Workout GetWorkoutById(int id)
        {
            return _workouts.Find(w => w.Id == id);
        }

        [HttpGet]
        [Route("api/Workout")]
        public LinkedList<Workout> GetWorkouts()
        {
            return new LinkedList<Workout>(_workouts);
        }
    }

    public class Workout
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Notes { get; set; }
        public WorkoutType WorkoutType { get; set; }
        public string Location { get; set; }
    }

    public enum WorkoutType
    {
        Cardio,
        Burst,
        Other
    }
}
