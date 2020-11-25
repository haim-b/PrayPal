using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Content
{
    public class CameraPreviewViewModel : SpecialContentViewModelBase
    {
        private bool _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            set { SetProperty(ref _isOn, value); }
        }

        private CameraOptions _camera;

        public CameraOptions Camera
        {
            get { return _camera; }
            set { SetProperty(ref _camera, value); }
        }

    }
}
