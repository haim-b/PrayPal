using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    // Taken from here: https://github.com/851265601/XFormsCameraPage/blob/master/Camerapage/ ////////////// https://docs.microsoft.com/he-il/xamarin/xamarin-forms/app-fundamentals/custom-renderer/view
    public class CameraPreview : View
    {
        public static readonly BindableProperty CameraProperty = BindableProperty.Create(propertyName: "Camera", returnType: typeof(CameraOptions), declaringType: typeof(CameraPreview), defaultValue: CameraOptions.Rear);
        public static readonly BindableProperty IsPreviewingProperty = BindableProperty.Create(nameof(IsPreviewing), typeof(bool), typeof(CameraPreview), false);

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public bool IsPreviewing
        {
            get { return (bool)GetValue(IsPreviewingProperty); }
            set { SetValue(IsPreviewingProperty, value); }
        }
    }

    public enum CameraOptions
    {
        Front, Rear
    }
}
