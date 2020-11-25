using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Hardware;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PrayPal.CameraPreview), typeof(PrayPal.Droid.CameraPreviewRenderer))]
namespace PrayPal.Droid
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, AndroidCameraPreview>
    {
        AndroidCameraPreview _cameraPreview;
        private bool _isDisposed;

        public CameraPreviewRenderer(Context context)
            : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
                //_cameraPreview.Click -= OnCameraPreviewClicked;
                e.OldElement.PropertyChanged -= OnCameraPreviewPropertyChanged;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _cameraPreview = new AndroidCameraPreview(Context);
                    SetNativeControl(_cameraPreview);
                }

                SetCamera(e.NewElement.Camera);

                // Subscribe
                //_cameraPreview.Click += OnCameraPreviewClicked;
                e.NewElement.PropertyChanged += OnCameraPreviewPropertyChanged;
            }
        }

        private static Camera CreateCamera(CameraOptions cameraOptions)
        {
            return Camera.Open(cameraOptions == CameraOptions.Rear ? 0 : 1);
        }

        private void SetCamera(CameraOptions cameraOptions)
        {
            _cameraPreview.Preview = CreateCamera(cameraOptions);
        }

        private void OnCameraPreviewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_cameraPreview == null)
            {
                return;
            }

            if (e.PropertyName == nameof(CameraPreview.IsPreviewing))
            {
                OnIsPrevewingChanged((CameraPreview)sender);
            }
            else if (e.PropertyName == nameof(CameraPreview.Camera))
            {
                OnCameraChanged((CameraPreview)sender);
            }
        }

        private void OnIsPrevewingChanged(CameraPreview cameraPreview)
        {
            if (_isDisposed)
            {
                //SetCamera(cameraPreview.Camera);
                return;
            }

            bool isPreviewing = cameraPreview.IsPreviewing;

            if (isPreviewing)
            {
                _cameraPreview.Preview?.StartPreview();
            }
            else
            {
                _cameraPreview.Preview?.StopPreview();
            }

            _cameraPreview.IsPreviewing = isPreviewing;
        }

        private void OnCameraChanged(CameraPreview cameraPreview)
        {
            if (_isDisposed)
            {
                return;
            }

            _cameraPreview.Preview?.Release();
            SetCamera(cameraPreview.Camera);
        }

        protected override void Dispose(bool disposing)
        {
            _isDisposed = true;

            if (disposing)
            {
                Control.Preview.Release();
            }

            base.Dispose(disposing);
        }
    }
}