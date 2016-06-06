namespace ParentChildForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class FlightDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<Message> message)
        {
            var msg = await message;

            var form = new FormDialog<FlightFormState>(new FlightFormState(), Forms.BuildFlightForm, FormOptions.PromptInStart, null);
            context.Call(form, this.GetPassengersAsync);
        }

        private async Task GetPassengersAsync(IDialogContext context, IAwaitable<FlightFormState> result)
        {
            var flightFormState = await result;

            context.PerUserInConversationData.SetValue("FlightState", flightFormState);

            await this.GetPassengersFormComplete(context, null);
        }

        private async Task GetPassengersFormComplete(IDialogContext context, PassengerFormState passengerFormState)
        {
            var flightFormState = context.PerUserInConversationData.Get<FlightFormState>("FlightState");

            if (passengerFormState != null)
            {
                flightFormState.PassengersCheckedIn.Add(passengerFormState);
                context.PerUserInConversationData.SetValue("FlightState", flightFormState);
            }

            var passengers = this.GetPassengersByFligt(flightFormState.Flight);

            var notCheckedInPassengers = passengers.Where(p => !flightFormState.PassengersCheckedIn.Any(x => x.PassengerName == p.FullName)).ToList();

            if (notCheckedInPassengers == null || !notCheckedInPassengers.Any())
            {
                await this.FlightFormComplete(context, flightFormState);
                return;
            }

            var form = new FormDialog<PassengerFormState>(new PassengerFormState(notCheckedInPassengers), Forms.BuildPassengersForm, FormOptions.PromptInStart);

            context.Call(form, this.GetPassengerFormComplete);
        }

        private async Task GetPassengerFormComplete(IDialogContext context, IAwaitable<PassengerFormState> result)
        {
            var passengerFormState = await result;

            await this.GetPassengersFormComplete(context, passengerFormState);
        }

        private async Task FlightFormComplete(IDialogContext context, FlightFormState flightFormState)
        {
            await context.PostAsync($"Flight '{flightFormState.Flight}' Passengers Checked In: '{flightFormState.PassengersCheckedIn.Count()}'");
        }

        private IEnumerable<Passenger> GetPassengersByFligt(string flight)
        {
            return new List<Passenger>()
            {
                new Passenger() { FullName = "John Doe" },
                new Passenger() { FullName = "Kevin Fabrikam" },
                new Passenger() { FullName = "Sarah Contoso" },
            };
        }
    }
}
