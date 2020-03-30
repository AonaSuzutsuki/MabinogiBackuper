using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabinogiBackuperLib.Setting
{
    public class VideoSettingInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ColorDepth { get; set; }
        
        public string ThemePack { get; set; }
        public string ThemePackColorSet { get; set; }
        public int Gamma { get; set; }
        public int CameraZoomOutMax2 { get; set; }

        public int ExtendSight { get; set; }

        public bool UseWideMode { get; set; }
        public bool ColorCorrection { get; set; }
        public bool VerticalSync { get; set; }
        public bool IsWindowed { get; set; }
    }
}
