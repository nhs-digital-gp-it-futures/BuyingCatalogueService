using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// A view representation of the <see cref="Solution"/> entity that matched a specific ID.
    /// </summary>
    public sealed class SolutionByIdViewModel
    {
        internal SolutionByIdViewModel(Solution solution)
        {
            Id = solution.Id;
            Name = solution.Name;
            MarketingData = new MarketingDataViewModel(solution);
        }

        /// <summary>
        /// Identifier of the solution.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the solution.
        /// </summary>
        public string Name { get; }

        public MarketingDataViewModel MarketingData { get; }
    }

    public class BrowserBasedSection : Section
    {
        internal BrowserBasedSection()
        {
            //if (browserBased == null) return;
            //Sections = new List<Section>();

            ////temp - ignore the solution, return canned data
            //BuildCannedSections();
            //_isComplete = Sections.OfType<BrowsersSupportedSection>().FirstOrDefault()?.Status == "COMPLETE"
            //    && Sections.OfType<PlugInsOrExtensionsSection>().FirstOrDefault()?.Status == "COMPLETE"
            //    && Sections.OfType<ConnectivityAndResolutionSection>().FirstOrDefault()?.Status == "COMPLETE";
        }

        //private void BuildCannedSections()
        //{
        //    Sections.Add(new BrowsersSupportedSection());
        //    Sections.Add(new PlugInsOrExtensionsSection());
        //    Sections.Add(new ConnectivityAndResolutionSection());
        //    Sections.Add(new HardwareRequirementsSection());
        //    Sections.Add(new AdditionalInformationSection());
        //}

        public override string Id => "browser-based";
    }

    //public class BrowsersSupportedSection : Section
    //{
    //    internal BrowsersSupportedSection()
    //    {
    //        Data = new BrowserSupportedSectionData();

    //        _isComplete = Data.GoogleChrome ||
    //                      Data.MicrosoftEdge ||
    //                      Data.MozillaFirefox ||
    //                      Data.Opera ||
    //                      Data.Safari ||
    //                      Data.Chromium ||
    //                      Data.InternetExplorer11 ||
    //                      Data.InternetExplorer10;
    //    }

    //    public BrowserSupportedSectionData Data { get; }

    //    public override string Id => "browsers-supported";
    //}

    //public class BrowserSupportedSectionData
    //{
    //    public bool GoogleChrome => false;
    //    public bool MicrosoftEdge => true;
    //    public bool MozillaFirefox => false;
    //    public bool Opera => true;
    //    public bool Safari => true;
    //    public bool Chromium => false;
    //    public bool InternetExplorer11 => true;
    //    public bool InternetExplorer10 => false;
    //}

    //public class PlugInsOrExtensionsSection : Section
    //{
    //    public override string Id => "plug-ins-or-extensions";
    //}

    //public class ConnectivityAndResolutionSection : Section
    //{
    //    public override string Id => "connectivity-and-resolution";
    //}

    //public class HardwareRequirementsSection : Section
    //{
    //    public override string Id => "hardware-requirements";
    //}

    //public class AdditionalInformationSection : Section
    //{
    //public override string Id => "additional-information";
    //}

    public class NativeMobileSection : Section
    {
        public override string Id => "native-mobile";
    }

    public class NativeDesktopSection : Section
    {
        public override string Id => "native-desktop";
    }
}
