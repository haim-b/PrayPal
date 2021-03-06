﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Models
{
    public class DocumentModel<T> : BindableBase where T : ITextContainer
    {
        private string _title;
        private Func<IEnumerable<T>> _textsFactory;
        private DateTime? _contentGenerationTime;


        public DocumentModel(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        public bool TrackContentGenerationTime { get; set; }

        public bool IsGrouping { get; set; }

        public IEnumerable<T> Texts
        {
            get
            {
                if (_contentGenerationTime == null)
                {
                    _contentGenerationTime = DateTime.Now;
                }

                return _textsFactory.Invoke();
            }
        }

        public void SetTextsFactory(Func<IEnumerable<T>> textsFactory)
        {
            _textsFactory = textsFactory;
        }

        public void ReGenerateContentIfOlderThan(DateTime time)
        {
            if (TrackContentGenerationTime && _contentGenerationTime != null && _contentGenerationTime.Value.Date < time.Date)
            {
                RaisePropertyChanged(nameof(Texts));
            }
        }
    }
}
