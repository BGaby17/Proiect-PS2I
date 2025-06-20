using System;

namespace DateModel
{
    public class ProcessStatusEvent
    {
        // Constructor principal
        public ProcessStatusEvent(ProcessState NewInfo, DateTime StateChangedDate)
        {
            State = NewInfo;
            this.StateChangedDate = StateChangedDate;
            Id = Guid.NewGuid(); // Generăm un ID unic
        }

        // Constructor implicit
        public ProcessStatusEvent()
        {
            Id = Guid.NewGuid(); // Generăm un ID unic
        }

        // Proprietăți
        public Guid Id { get; private set; } // ID unic generat automat
        public ProcessState State { get; set; }
        public DateTime StateChangedDate { get; set; }
    }
}
