using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Transforms
{
    public class Mashup : ContentPage
    {
        SKCanvasView canvasView;
        float xSkew, ySkew;
        float revolveDegrees;

        public Mashup()
        {
            Title = "Fishy";
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
            xSkew = 0.1f;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            new Animation((value) => revolveDegrees = 360 * (float)value).
                Commit(this, "revolveAnimation", length: 10000, repeat: () => true);

            new Animation((value) =>
            {
                ySkew = (float)Math.Sin((float)value * 6.2f);
                canvasView.InvalidateSurface();
            }).Commit(this, "verticalSkew", length: 1000, repeat: () => true);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.AbortAnimation("revolveAnimation");
            this.AbortAnimation("verticalSkew");
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.SpringGreen,
                TextSize = 80
            })
            {
                string text = "<=<";
                SKRect textBounds = new SKRect();
                textPaint.MeasureText(text, ref textBounds);

                canvas.Translate(info.Width / 2, info.Height / 2);
                canvas.RotateDegrees(revolveDegrees);

                float radius = Math.Min(info.Width, info.Height) / 4;
                canvas.Translate(0, radius);

                canvas.Skew(xSkew, ySkew);
                canvas.DrawText(text, 0, -textBounds.Top, textPaint);
            }
        }
    }
}
