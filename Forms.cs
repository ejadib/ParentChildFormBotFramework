namespace ParentChildForm
{
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.FormFlow.Advanced;

    public class Forms
    {
        public static IForm<FlightFormState> BuildFlightForm()
        {
            return new FormBuilder<FlightFormState>()
                .Field(nameof(FlightFormState.Flight))
                .Build();
        }

        internal static IForm<PassengerFormState> BuildPassengersForm()
        {
            return new FormBuilder<PassengerFormState>()
                 .Field(new FieldReflector<PassengerFormState>(nameof(PassengerFormState.PassengerName))
                    .SetType(null)
                    .SetPrompt(PerLinePromptAttribute("Please select the passenger you want to check-in: {||}"))
                    .SetDefine((state, field) =>
                    {
                        foreach (var passenger in state.AvailablePassengers)
                        {
                            field
                                .AddDescription(passenger.FullName, passenger.FullName)
                                .AddTerms(passenger.FullName, passenger.FullName);
                        }

                        return Task.FromResult(true);
                    })).Build();
        }

        private static PromptAttribute PerLinePromptAttribute(string pattern)
        {
            return new PromptAttribute(pattern)
            {
                ChoiceStyle = ChoiceStyleOptions.PerLine
            };
        }
    }
}
