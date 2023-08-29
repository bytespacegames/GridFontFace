using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TextureMapper.GFFFontRenderer;
using Sys = Cosmos.System;

namespace GFFFontRenderer
{
    public class Kernel : Sys.Kernel
    {
        public static Canvas canvas;
        BallerFont font;
        protected override void BeforeRun()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
            font = FontRenderer.LoadFont(fontBytes);
            canvas.Clear(Color.FromArgb(255, 255, 255));
        }

        [ManifestResourceStream(ResourceName = "GFFFontRenderer.EmbeddedFiles.ballerosfont.gff")]
        public static byte[] fontBytes;

        protected override void Run()
        {
            canvas.Clear(Color.FromArgb(255,255,255));
            FontRenderer.DrawText("Hi. \nHello world from Cosmos.", 5, 5, 3, Color.Black, font);
            canvas.Display();
        }
    }
}
