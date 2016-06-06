namespace ParentChildForm
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class FlightFormState
    {
        public FlightFormState()
        {
            this.PassengersCheckedIn = new List<PassengerFormState>();  
        }

        public string Flight { get; set; }

        public IList<PassengerFormState> PassengersCheckedIn { get; private set; }
    }
}