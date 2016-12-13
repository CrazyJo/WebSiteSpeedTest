using Core;

namespace UtilitiesPackage
{
    public class LoadTimeManagerOptions
    {
        public IStorage Storage { get; set; }
        public IMeasurementResultDisplayer Displayer { get; set; }
        public bool ParseAll { get; set; }
        public int NumberOfFiles { get; set; }
        public int NumberOfLinks { get; set; }
    }
}
