﻿using System;
using System.Collections.Generic;

namespace CourseManagerment.Models;

public partial class CourseSchedule
{
    public int TeachingScheduleId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? TeachingDate { get; set; }

    public int? Slot { get; set; }

    public int? RoomId { get; set; }

    public string? Description { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<RollCallBook> RollCallBooks { get; set; } = new List<RollCallBook>();

    public virtual Room? Room { get; set; }
}
