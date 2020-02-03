using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Prism.Regions;

namespace PrismDemo.Infrastructure
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement element in e.NewItems)
                    {
                        regionTarget.Children.Add(element);
                    }
                }

                // Handle remove (данный блок if не был приведен в видео и работало без него.
                // Здесь этот блок я написал сам).
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (FrameworkElement element in e.OldItems)
                    {
                        regionTarget.Children.Remove(element);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
