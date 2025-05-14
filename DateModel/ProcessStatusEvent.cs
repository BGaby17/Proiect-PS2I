using System;

namespace DateModel
{
    public class ProcessStatusEvent
    {
        public ProcessStatusEvent(ProcessState NewInfo, DateTime StateChangedDate)
        {
            State = NewInfo;
            this.StateChangedDate = StateChangedDate;
        }

        public ProcessStatusEvent() { }

        public ProcessState State { get; set; }
        public DateTime StateChangedDate { get; set; }
    }
}