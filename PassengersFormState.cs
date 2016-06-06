namespace ParentChildForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class PassengerFormState
    {
        public PassengerFormState(IEnumerable<Passenger> availablePassengers)
        {
            this.AvailablePassengers = availablePassengers;
        }

        public IEnumerable<Passenger> AvailablePassengers { get; private set; }

        public string PassengerName { get; set; }

        public Passenger SelectedPassenger
        {
            get { return this.AvailablePassengers.SingleOrDefault(p => p.FullName == this.PassengerName); }
        }
    }
}