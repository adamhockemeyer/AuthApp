using System;

using Xamarin.Forms;

using FFImageLoading.Svg.Forms;
using FFImageLoading.Transformations;
using System.Linq;

namespace AuthApp.Views
{
    public class SVGImage : SvgCachedImage
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(SVGImage), Color.Default, propertyChanged: ImageColorPropertyChanged);

        public Color Color
        {
            get => (Color)base.GetValue(ColorProperty);
            set => base.SetValue(ColorProperty, value);
        }

        private static void ImageColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SVGImage)bindable;

            if(control.Transformations.Any(p => p.GetType() == typeof(TintTransformation)))
            {
                control.Transformations.RemoveAll(p => p.GetType() == typeof(TintTransformation));
            }

            Color newColor = ((Color)newValue);

            control.Transformations.Add(new TintTransformation { EnableSolidColor = true, HexColor = newColor.GetHexString() });


        }



        /*
         * <ffimageloadingsvg:SvgCachedImage Margin="10,0,0,0" WidthRequest="15" HeightRequest="15" Source="resource://FaxFromPhone.Resources.Images.copy.svg">
                            <ffimage:CachedImage.Transformations>
                                <ffimagetransformation:TintTransformation HexColor="#ffffffff" EnableSolidColor="true" />
                            </ffimage:CachedImage.Transformations>
                        </ffimageloadingsvg:SvgCachedImage>
         */ 



        public SVGImage()
        {
             
        }
    }

    public static class ExtensionMethods
    {
        public static string GetHexString(this Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

            return hex;
        }
    }
}

