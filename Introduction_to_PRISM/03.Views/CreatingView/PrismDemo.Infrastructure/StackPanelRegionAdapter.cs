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
            region.Views.CollectionChanged += (s, a) =>
            {
                if (a.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement element in a.NewItems)
                    {
                        regionTarget.Children.Add(element);
                    }
                }

                if (a.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (FrameworkElement element in a.OldItems)
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
