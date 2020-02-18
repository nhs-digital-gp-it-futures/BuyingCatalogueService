using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Serializer supplied by framework")]
    public class UpdateSolutionContactViewModelConverter : JsonConverter<IUpdateSolutionContact>
    {
        public override IUpdateSolutionContact ReadJson(
            JsonReader reader,
            Type objectType,
            IUpdateSolutionContact existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            return (IUpdateSolutionContact)serializer.Deserialize(reader, typeof(UpdateSolutionContactViewModel));
        }

        public override void WriteJson(JsonWriter writer, IUpdateSolutionContact value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(UpdateSolutionContactViewModel));
        }
    }
}
