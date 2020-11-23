using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Models;

namespace PrayPal.Content
{
    public abstract class PrayerBase<T> : IPrayer
    {
        protected readonly ICollection<T> _items = new LinkedList<T>();
        private Nusach? _nusach;

        private IList<T> _itemsList;

        public PrayerBase()
        {
            Title = GetTitle();
        }

        protected ILogger Logger { get; private set; }

        public async Task CreateAsync(DayJewishInfo dayInfo, ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            ValidateDayInfo(dayInfo);

            Logger = logger;
            DayInfo = dayInfo;
            _items.Clear();

            await CreateOverrideAsync();

            _itemsList = null;
        }

        protected virtual void ValidateDayInfo(DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }
        }

        protected abstract Task CreateOverrideAsync();

        protected abstract string GetTitle();

        public string Title { get; set; }

        protected virtual IEnumerable<T> GetItems()
        {
            return _items;
        }

        public IList<T> Items
        {
            get
            {
                if (_itemsList == null)
                {
                    IEnumerable<T> items = GetItems();

                    if (items is IList<T>)
                    {
                        _itemsList = (IList<T>)items;
                    }
                    else
                    {
                        _itemsList = new List<T>(items);
                    }
                }

                return _itemsList;
            }
        }

        public bool HasGroups { get; protected set; }

        protected abstract void AddStringFormat(string text, string argument, bool isHighlighted = false, bool isBold = false, string title = null, bool isCollapsible = false);

        protected abstract void Add(string text, string title = null, bool isCollapsible = false);

        protected abstract void Add(ParagraphModel paragraph);

        public virtual Nusach GetNusach()
        {
            if (_nusach == null)
            {
                TypeInfo typeInfo = this.GetType().GetTypeInfo();

                NusachAttribute nusachAtt = typeInfo.GetCustomAttributes<NusachAttribute>().FirstOrDefault();

                if (nusachAtt == null)
                {
                    Logger.LogError(this.GetType().Name + "/GetNusach: Cannot find NusachAttribute.");
                    return Nusach.Sfard;
                }

                _nusach = nusachAtt.Nusach.FirstOrDefault();
            }

            return (Nusach)_nusach;
        }

        public ParagraphModel GetItemAtIndex(int index)
        {
            if (index < 0)
            {
                return null;
            }

            return GetItemAtIndexImpl(index);
        }

        protected abstract ParagraphModel GetItemAtIndexImpl(int index);

        public abstract int GetItemsCount();

        public virtual bool IsDayCritical
        {
            get { return false; }
        }

        public virtual bool UseCompactZoomedOutItems
        {
            get
            {
                return false;
            }
        }

        protected DayJewishInfo DayInfo { get; private set; }
    }

    public interface IPrayer : ITextDocument
    {
        bool IsDayCritical { get; }

        ParagraphModel GetItemAtIndex(int index);

        int GetItemsCount();
    }

    public interface ITextDocument
    {
        Task CreateAsync(DayJewishInfo dayInfo, ILogger logger);

        bool UseCompactZoomedOutItems { get; }

        string Title { get; }

        bool HasGroups { get; }
    }

    public class PrayerMetadata
    {
        [DefaultValue((string)null)]
        public string Name { get; set; }

        [DefaultValue((Nusach[])null)]
        public Nusach[] Nusach { get; set; }
    }
}
