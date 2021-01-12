namespace Final_Project.Models
{
    public class Project
    {
        public int PID { get; set; }
        public string Title { get; set; }
        public string Project_type { get; set; }
        public int PI { get; set; }
        public string Description { get; set; }
        public int ApertureUpperLimit { get; set; }
        public int ApertureLowerLimit { get; set; }
        public int FoVUpperLimit { get; set; }
        public int FoVLowerLimit { get; set; }
        public float PixelScaleUpperLimit { get; set; }
        public float PixelScaleLowerLimit { get; set; }
        public string MountType { get; set; }
        public string CameraType_1 { get; set; }
        public string CameraType_2 {get; set; }
        public string Johnson_B { get; set; }
        public string Johnson_V { get; set; }
        public string Johnson_R { get; set; }
        public string SDSS_u { get; set; }
        public string SDSS_g { get; set; }
        public string SDSS_r { get; set; }
        public string SDSS_i { get; set; }
        public string SDSS_z { get; set; }
        public string Progress { get; set; }
    }
}
