namespace Final_Project.Models
{
    public class Equipment
    {
        public int EID { get; set; }
        public float Aperture { get; set; }
        public float FoV { get; set; }
        public float PixelScale { get; set; }
        public float TrackingAccuracy { get; set; }
        public float LimitingMagnitude { get; set; }
        public float ElevationLimit { get; set; }
        public string MountType { get; set; }
        public string CameraType_1 { get; set; }
        public string CameraType_2 { get; set; }
        public string Johnson_B { get; set; }
        public string Johnson_V { get; set; }
        public string Johnson_R { get; set; }
        public string SDSS_u { get; set; }
        public string SDSS_g { get; set; }
        public string SDSS_r { get; set; }
        public string SDSS_i { get; set; }
        public string SDSS_z { get; set; }
    }
}
